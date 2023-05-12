using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Text;
using Common;
using Common.Encryption;
using Common.Messages;
using static Common.SocketMethods;

namespace Server
{
    internal class Server
    {
        private const int MaxConnectionsQueueSize = 5;
        private readonly Socket server = new(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
        private const int ServerID = Constants.ServerID;

        private readonly ConcurrentDictionary<int, Client> clients = new();
        private readonly ConcurrentDictionary<int, List<Client>> chats = new();
        private int lastCreatedChat = 0;
        private readonly ConcurrentDictionary<string, byte[]> files = new();


        private IPAddress GetLocalIPv4()
        {
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((item.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                    item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address;
                        }
                    }
                }
            }
            return IPAddress.Loopback;
        }

        private void BindSocketToThisMachine(Socket socket)
        {
            IPAddress ipAddress = GetLocalIPv4();
            try
            {
                socket.Bind(new IPEndPoint(ipAddress, Constants.ServerPort));
            }
            catch (SocketException)
            {
                Console.WriteLine($"The port {Constants.ServerPort} is unavailable.");
                throw;
            }
        }

        public void Run()
        {
            try
            {
                BindSocketToThisMachine(server);
            }
            catch (SocketException) 
            {
                return;
            }
            server.Listen(MaxConnectionsQueueSize);
            Console.WriteLine("The server is waiting for connections . . .");
            WaitForConnections();
        }

        private void WaitForConnections()
        {
            while (true)
            {
                Task.Run(CommunicateWithClient);
            }
        }

        private void CommunicateWithClient()
        {
            Socket client = server.Accept();
            try
            {
                if (((ServiceMessage)ReceiveMessage(client)).MessageType != ServiceMessage.Type.Connecting)
                {
                    return;
                }
                KeyExchange(client);
                ClientAuthentication(client);
                while (IsConnected(client))
                {
                    Message message = ReceiveMessage(client);
                    HandleMessage(message);
                }
            }
            catch {}
            finally
            {
                client.Close();
            }

        }

        private void KeyExchange(Socket client)
        {

        }

        private void ClientAuthentication(Socket client)
        {
            while (IsConnected(client))
            {
                var message = (AuthenticationMessage)ReceiveMessage(client);
                if (message.IsSigningUp)
                {
                    SignUp(client, message);
                }
                else
                {
                    SignIn(client, message);
                }
            }
        }

        private void SignUp(Socket client, AuthenticationMessage message)
        {
            try
            {
                int newUserID = Sql.AddUser(message.Email, message.Password);
                if (newUserID == -1)
                {
                    SendMessage(new ServiceMessage(ServerID, ServiceMessage.Type.Error), client);
                }
                else
                {
                    SendMessage(new UserInfoMessage(ServerID, newUserID), client);
                    var userInfo = (UserInfoMessage)ReceiveMessage(client);
                    clients.TryAdd(newUserID, new Client(client, userInfo.Name, newUserID));
                    SendSuccessMessage(client);
                }
            }
            catch (ArgumentException e)
            {
                var error = Encoding.Default.GetBytes(e.Message);
                var errorMessage = new ServiceMessage(ServerID, ServiceMessage.Type.Error, error);
                SendMessage(errorMessage, client);
            }
        }

        private void SignIn(Socket client, AuthenticationMessage message)
        {
            try
            {
                int userID = Sql.FindUser(message.Email, message.Password);
                var userInfoMessage = new UserInfoMessage(ServerID, userID, clients[userID].Name);
                SendMessage(userInfoMessage, client);
                // send chats and keys
                SendSuccessMessage(client);
            }
            catch (ArgumentException e)
            {
                var error = Encoding.Default.GetBytes(e.Message);
                var errorMessage = new ServiceMessage(ServerID, ServiceMessage.Type.Error, error);
                SendMessage(errorMessage, client);
            }
        }

        private bool HandleMessage(Message message)
        {
            try
            {
                if (message is UserInfoMessage)
                {
                    HandleUserInfoMessage((UserInfoMessage)message);
                }
                else if (message is ServiceMessage)
                {
                    HandleServiceMessage((ServiceMessage)message);
                }
                else if (message is ChatCreationMessage)
                {
                    HandleChatCreationMessage((ChatCreationMessage)message);
                }
                else if (message is FileInfoMessage)
                {
                    HandleFileInfoMessage((FileInfoMessage)message);
                }
                else if (message is FileMessage)
                {
                    HandleFileMessage((FileMessage)message);
                }
                else if (message is ChatMessage)
                {
                    HandleChatMessage((ChatMessage)message);
                }
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;

        }

        private void HandleChatMessage(ChatMessage message)
        {
            List<Client> receivers = chats[message.ChatID];
            foreach (var receiver in receivers)
            {
                receiver.SendIfOnline(message);
            }
        }

        private void HandleChatCreationMessage(ChatCreationMessage message)
        {
            if (message.ChatID == 0)
            {
                CreateNewChat(message);
                message.HopsNumber = 2 * message.Members.Count - 1;
            }
            if (message.HopsNumber > 0)
            {
                message.HopsNumber -= 1;
                int nextClientIndex = (2 * message.Members.Count - 2 - message.HopsNumber) % message.Members.Count;
                int nextClientID = message.Members[nextClientIndex];
                clients[nextClientID].SendIfOnline(message); 
            }
            else if (IsKeyExchangeCompleted(message.PublicKeys))
            {
                var confirmationMessage = new ServiceMessage(ServerID, ServiceMessage.Type.Success);
                foreach (int memberID in message.Members)
                {
                    Task.Run(() => { clients[memberID].SendIfOnline(confirmationMessage); });
                }
            }      
        }

        private void CreateNewChat(ChatCreationMessage message)
        {
            var chatMembers = new List<Client>();
            foreach (int id in message.Members)
            {
                chatMembers.Add(clients[id]);
            }
            int chatID = Interlocked.Increment(ref lastCreatedChat);
            chats.TryAdd(chatID, chatMembers);
            message.ChatID = chatID;
        }

        private bool IsKeyExchangeCompleted((int Count, BigInteger PublicKey)[] publicKeys)
        {
            for (var i = 0; i < publicKeys.Length; i++)
            {
                if (publicKeys[i].Count != -1 || publicKeys[i].PublicKey != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void HandleFileMessage(FileMessage message)
        {
            string fileID = Encryption.SHA256Hash(message.Contents);
            files.TryAdd(fileID, message.Contents);
            var fileInfoMessage = new FileInfoMessage(fileID, message.FileName, message.ChatID, message.SenderID);
            HandleChatMessage(fileInfoMessage);
        }

        private void HandleFileInfoMessage(FileInfoMessage message)
        {
            if (files.TryGetValue(message.FileID, out var fileContents))
            {
                var fileMessage = new FileMessage(message.FileName, fileContents, message.ChatID, ServerID);
                clients[message.SenderID].SendIfOnline(fileMessage);
            }
        }

        private void HandleServiceMessage(ServiceMessage message)
        {
            if (message.MessageType == ServiceMessage.Type.Disconnecting)
            {
                Client client = clients[message.SenderID];
                lock (client.IsOnlineLock)
                {
                    client.IsOnline = false;
                }
            }
        }

        private void HandleUserInfoMessage(UserInfoMessage message)
        {
            if (message.SenderID == message.UserID)
            {
                Client client = clients[message.SenderID];
                if (client.Name != message.Name)
                {
                    lock (client.NameLock)
                    {
                        client.Name = message.Name;
                    }
                    SendSuccessMessage(client.Socket);
                }
            }
        }


        private void SendSuccessMessage(Socket client)
        {
            SendMessage(new ServiceMessage(ServerID, ServiceMessage.Type.Success), client);
        }

    }
}