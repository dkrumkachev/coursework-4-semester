using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Common.SocketMethods;
using Common.Messages;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Tls;
using Common.Encryption;
using Org.BouncyCastle.Math.EC;
using Message = Common.Messages.Message;
using Org.BouncyCastle.Crypto.Paddings;
using System.Text.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Drawing;

namespace Client
{
    internal class Client
    {
        private readonly object sendLock = new();
        public const string SelfChatName = "Saved Messages";
        private Socket server = new(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
        private readonly TripleDES server3DES = new();
        private ConcurrentDictionary<int, Chat> chats = new();
        private readonly ConcurrentDictionary<string, (int ID, string Name)> users = new();
        private readonly ConcurrentDictionary<int, string> usersNames = new();
        private int selfID;
        private string selfUsername = string.Empty;
        public string SelfName { get; private set; } = string.Empty;

        private readonly Action<string, int> chatCreated;
        private readonly Action<string, string, string> incomingText;
        private readonly Action<string, string, Image> incomingImage;
        private readonly Action<string, string, string, string> incomingFile;

        public Client(Action<string, int> chatCreated, Action<string, string, string> incomingText, 
            Action<string, string, Image> incomingImage, Action<string, string, string, string> incomingFile) 
        {
            this.chatCreated = chatCreated;
            this.incomingText = incomingText;
            this.incomingImage = incomingImage;
            this.incomingFile = incomingFile;
        }

#pragma warning disable SYSLIB0011

        [Serializable]
        public struct SerializedFields
        {
            public List<KeyValuePair<int, Chat>> Chats;
            public List<KeyValuePair<string, (int, string)>> Users;
            public List<KeyValuePair<int, string>> UsersNames;
            public int SelfID;
            public string SelfUsername;
            public string SelfName;
        }

        public void Save(string filename)
        {
            var serializer = new BinaryFormatter();
            using var stream = new MemoryStream();
            serializer.Serialize(stream, chats.ToList());
            File.WriteAllBytes(filename, stream.ToArray());
        }

        public void Restore(string filename)
        {
            byte[] bytes = File.ReadAllBytes(filename);
            var serializer = new BinaryFormatter();
            using var stream = new MemoryStream(bytes);
            var chatList = (List<KeyValuePair<int, Chat>>)serializer.Deserialize(stream);
            chats = new ConcurrentDictionary<int, Chat>(chatList);
        }

#pragma warning restore SYSLIB0011

        private void SendWithLock(Message message, Socket server, TripleDES tripleDES)
        {
            lock (sendLock)
            {
                SendMessage(message, server, tripleDES);
            }
        }

        public void WaitForConnectionToServer() 
        {
            bool success = false;
            server = new(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
            while (!success)
            {
                try
                {
                    server.Connect(IPAddress.Parse(Constants.ServerIP), Constants.ServerPort);
                    SendMessage(new ServiceMessage(0, ServiceMessage.Type.Connecting), server);
                    success = KeyExchange(server);
                }
                catch
                {
                    success = false;
                }
            }
        }

        private bool KeyExchange(Socket server)
        {
            var ecdhMessage = (ECDHMessage)ReceiveMessage(server);
            BigInteger privateKey = Encryption.GeneratePrivateKey();
            server3DES.Key = Encryption.MultiplyByPrivateKey(ecdhMessage.PublicKey, privateKey);
            ecdhMessage.PublicKey = Encryption.GetPublicKey(privateKey);
            SendMessage(ecdhMessage, server);
            var successMessage = (ServiceMessage)ReceiveMessage(server, server3DES);
            return (successMessage.MessageType == ServiceMessage.Type.Success);
        }

        public bool Authentication(string username, ref string password, out string error, 
            out byte[] messageHistoryKey, bool registration = false)
        {
            password = Encryption.SHA256Hash(Encoding.Default.GetBytes(password));
            return Authentication(username, password, out error, out messageHistoryKey, registration);
        }

        public bool Authentication(string username, string passwordHash, out string error, 
            out byte[] messageHistoryKey, bool registration = false)
        {
            error = "";
            messageHistoryKey = new TripleDES().GenerateKey();
            Message message = new AuthenticationMessage(0, registration, username, passwordHash, messageHistoryKey);
            SendMessage(message, server, server3DES);
            message = ReceiveMessage(server, server3DES);
            if (message is ServiceMessage serviceMessage &&
                serviceMessage.MessageType == ServiceMessage.Type.Error &&
                serviceMessage.AdditionalInfo != null)
            {
                error = Encoding.Default.GetString(serviceMessage.AdditionalInfo);
                return false;
            }
            var userInfoMessage = (UserInfoMessage)message;
            selfID = userInfoMessage.UserID;
            SelfName = userInfoMessage.Name;
            selfUsername = username;
            users.TryAdd(username, (selfID, SelfName));
            usersNames.TryAdd(selfID, SelfName);
            var selfChat = new Chat(0, new List<int> { selfID }, SelfChatName);
            selfChat.TripleDES.GenerateKey();
            chats.TryAdd(0, selfChat);
            if (!registration && ReceiveMessage(server, server3DES) is AuthenticationMessage authenticationMessage)
            {
                messageHistoryKey = authenticationMessage.HistoryKey;
            }
            return true;
        }

        public bool SetName(string name)
        {
            SendWithLock(new UserInfoMessage(selfID, selfID, name), server, server3DES);
            var successMessage = (ServiceMessage)ReceiveMessage(server, server3DES);
            if (successMessage.MessageType == ServiceMessage.Type.Success)
            {
                SelfName = name;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            //SendWithLock(new ServiceMessage(selfID, ServiceMessage.Type.Disconnecting), server, server3DES);
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        public string? FindUser(string username)
        {
            if (users.TryGetValue(username, out var user)) 
            {
                return user.Name;
            }
            SendWithLock(new UserInfoMessage(selfID, username), server, server3DES);
            for (var i = 0; i < 5; i++)
            {
                if (users.TryGetValue(username, out var value))
                {
                    return value.Name;
                }
                Thread.Sleep(50);
            }
            return null;
        }

        private string? FindUser(int id)
        {
            if (usersNames.TryGetValue(id, out var name))
            {
                return name;
            }
            SendWithLock(new UserInfoMessage(selfID, id), server, server3DES);
            for (var i = 0; i < 4; i++)
            {
                if (usersNames.TryGetValue(id, out name))
                {
                    return name;
                }
                Thread.Sleep(50);
            }
            return null;
        }

        public bool CreateSingleChat(string username)
        {
            try
            {
                if (users[username].ID == selfID)
                {
                    return true;
                }
                List<int> chatMembers = new() { users[username].ID, selfID };
                SendWithLock(new ChatCreationMessage(chatMembers, selfID, SelfName), server, server3DES);
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;
        }

        public bool CreateGroupChat(string chatName, List<string> members)
        {
            members.Add(selfUsername);
            try
            {
                List<int> memberIDs = members.Select(i => users[i].ID).ToList();
                SendWithLock(new ChatCreationMessage(memberIDs, selfID, chatName), server, server3DES);
            }
            catch (KeyNotFoundException)
            { 
                return false; 
            }
            return true;
        }

        public int GetChatMembersCount(int chatID)
        {
            if (chats.TryGetValue(chatID, out var chat))
            {
                return chat.Members.Count;
            }
            return 0;
        }

        public void SendText(int chatID, string text)
        {
            if (chats.TryGetValue(chatID, out var chat))
            {
                byte[] contents = Encoding.Default.GetBytes(text);
                contents = chat.TripleDES.Encrypt(contents);
                var message = new MessageWithContents(contents, MessageWithContents.Type.Text, chatID, selfID);
                SendWithLock(message, server, server3DES);
            }
        }

        public void SendImage(int chatID, byte[] image)
        {
            if (chats.TryGetValue(chatID, out var chat))
            {
                image = chat.TripleDES.Encrypt(image);
                var message = new MessageWithContents(image, MessageWithContents.Type.Image, chatID, selfID);
                SendWithLock(message, server, server3DES);
            }
        }

        public void SendFile(int chatID, byte[] fileContents, string filename)
        {
            if (chats.TryGetValue(chatID, out var chat))
            {
                fileContents = chat.TripleDES.Encrypt(fileContents);
                var message = new FileMessage(chatID, selfID, fileContents, filename);
                SendWithLock(message, server, server3DES);
            }
        }

        public void ReceiveMessages()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Message message;
                    try
                    {
                        message = ReceiveMessage(server, server3DES);
                    }
                    catch (SocketException)
                    {
                        return;
                    }
                    HandleMessage(message);
                }
            });
        }

        private void HandleMessage(Message message)
        {
            if (message is MessageWithContents)
            {
                HandleMessageWithContents((MessageWithContents)message);
            }
            else if (message is ChatCreationMessage)
            {
                HandleChatCreationMessage((ChatCreationMessage)message);
            }
            else if (message is UserInfoMessage)
            {
                HandleUserInfoMessage((UserInfoMessage)message);
            }
        }

        private void HandleMessageWithContents(MessageWithContents msg)
        {
            Chat chat = chats[msg.ChatID];
            byte[] contents = chat.TripleDES.Encrypt(msg.Contents, true);
            if (!usersNames.TryGetValue(msg.SenderID, out _))
            {
                FindUser(msg.SenderID);
            }
            if (msg.ContentsType == MessageWithContents.Type.Image)
            {
                Image image = Image.FromStream(new MemoryStream(contents));
                incomingImage(chat.Name, usersNames[msg.SenderID],  image);
            }
            else if (msg.ContentsType == MessageWithContents.Type.Text)
            {
                string text = Encoding.Default.GetString(contents);
                incomingText(chat.Name, usersNames[msg.SenderID], text);
            }
            else if (msg.ContentsType == MessageWithContents.Type.File)
            {
                string filename = ((FileMessage)msg).FileName;
                string fileID = ((FileMessage)msg).FileID;
                incomingFile(chat.Name, usersNames[msg.SenderID], filename, fileID);

            }
        }

        private void HandleChatCreationMessage(ChatCreationMessage message)
        {
            if (message.Created)
            {
                chatCreated(chats[message.ChatID].Name, message.ChatID);
                return;
            }
            if (!chats.ContainsKey(message.ChatID))
            {
                foreach (int id in message.Members)
                {
                    if (!usersNames.TryGetValue(id, out _))
                    {
                        FindUser(id);
                    }
                }
                chats.TryAdd(message.ChatID, new Chat(message.ChatID, message.Members, message.ChatName));
                if (message.Members.Count == 2)
                {
                    message.ChatName = SelfName;
                }
            }
            ChatKeyExchange(message);
            SendWithLock(message, server, server3DES);
        }

        private void ChatKeyExchange(ChatCreationMessage message)
        {
            Chat chat = chats[message.ChatID];
            for (var i = 0; i < message.PublicKeys.Length; i++)
            {
                if (message.PublicKeys[i].Count == 0)
                {
                    message.PublicKeys[i].Count = 1;
                    message.PublicKeys[i].PublicKey = Encryption.GetPublicKey(chat.PrivateKey);
                    break;
                }
                if (message.PublicKeys[i].Count != -1)
                {
                    message.PublicKeys[i].Count += 1;
                    byte[] multiplied = 
                        Encryption.MultiplyByPrivateKey(message.PublicKeys[i].PublicKey, chat.PrivateKey);
                    if (message.PublicKeys[i].Count == message.Members.Count) 
                    {
                        chat.TripleDES.Key = Encryption.CropTheKey(multiplied);
                        message.PublicKeys[i].Count = -1;
                        message.PublicKeys[i].PublicKey = Array.Empty<byte>();
                    }
                    else
                    {
                        message.PublicKeys[i].PublicKey = multiplied;
                    }
                }
            }
        }

        private void HandleUserInfoMessage(UserInfoMessage message)
        {
            if (message.UserID != 0)
            {
                users.TryAdd(message.Username, (message.UserID, message.Name));
                usersNames.TryAdd(message.UserID, message.Name);
            }
        }

    }
}
