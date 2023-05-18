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

namespace Client
{
    internal class Client
    {
        private readonly Socket server = new(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
        private int selfID;
        public string SelfName { get; set; } = string.Empty;
        private readonly TripleDES server3DES = new();
        private readonly Dictionary<int, Chat> chats = new();

        public void DisconnectFromServer()
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        public void WaitForConnectionToServer() 
        {
            bool success = false;
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

        public bool Authentication(string username, string password, out string error, bool registration = false)
        {
            error = "";
            password = Encryption.SHA256Hash(Encoding.Default.GetBytes(password));
            SendMessage(new AuthenticationMessage(0, registration, username, password), server, server3DES);
            Message message = ReceiveMessage(server, server3DES);
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
            return true;
        }

        public bool SetName(string name)
        {
            SendMessage(new UserInfoMessage(selfID, selfID, name), server, server3DES);
            var successMessage = (ServiceMessage)ReceiveMessage(server, server3DES);
            if (successMessage.MessageType == ServiceMessage.Type.Success)
            {
                SelfName = name;
                return true;
            }
            return false;
        }

        public void ReceiveMessages()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var message = ReceiveMessage(server, server3DES);
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
            else if (message is FileInfoMessage)
            {
                HandleFileInfoMessage((FileInfoMessage)message);
            }
            else if (message is ChatCreationMessage)
            {
                HandleChatCreationMessage((ChatCreationMessage)message);
            }
        }

        private void HandleMessageWithContents(MessageWithContents msg)
        {
            Chat chat = chats[msg.ChatID];
            byte[] contents = chat.TripleDES.Encrypt(msg.Contents, true);
            if (msg.ContentsType == MessageWithContents.Type.Image)
            {
                Image image = Image.FromStream(new MemoryStream(contents));
            }
            else if (msg.ContentsType == MessageWithContents.Type.Text)
            {
                string text = Encoding.Default.GetString(contents);
            }
            else if (msg.ContentsType == MessageWithContents.Type.File)
            {
                string filename = ((FileMessage)msg).FileName;
            }
        }

        private void HandleChatCreationMessage(ChatCreationMessage message)
        {
            if (!chats.ContainsKey(message.ChatID))
            {
                chats.Add(message.ChatID, new Chat(message.ChatID, message.Members));
            }
            ChatKeyExchange(message);
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
                        chat.TripleDES.Key = multiplied;
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

        private void HandleFileInfoMessage(FileInfoMessage msg)
        {
            string filename = msg.FileName;
            string fileID = msg.FileID;
        }

    }
}
