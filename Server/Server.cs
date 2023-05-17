using System.Collections.Concurrent;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Common;
using Common.Encryption;
using Common.Messages;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
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
            return IPAddress.Parse(Constants.ServerIP);
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    item.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
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
                Console.WriteLine($"The server started at: {socket.LocalEndPoint?.ToString()}");
            }
            catch (SocketException)
            {
                Console.WriteLine($"The port {Constants.ServerPort} is unavailable.");
                throw;
            }
        }

        public void Run()
        {
            //Sql.DeleteUser("dkrumkachev");
            try
            {
                BindSocketToThisMachine(server);
            }
            catch (SocketException) 
            {
                return;
            }
            List<Sql.UserRecord> users = Sql.GetAllUsers();
            foreach (Sql.UserRecord user in users)
            {
                clients.TryAdd(user.ID, new Client(user.ID, user.Name));
            }
            server.Listen(MaxConnectionsQueueSize);
            Console.WriteLine("The server is waiting for connections . . .");
            WaitForConnections();
        }

        private void WaitForConnections()
        {
            while (true)
            {
                Socket clientSocket = server.Accept();
                Task.Run(() => CommunicateWithClient(clientSocket));
            }
        }

        private void CommunicateWithClient(Socket clientSocket)
        {
            try
            {
                if (((ServiceMessage)ReceiveMessage(clientSocket)).MessageType != ServiceMessage.Type.Connecting)
                {
                    return;
                }
                TripleDES tripleDES = KeyExchange(clientSocket);
                Client client = ClientAuthentication(clientSocket, tripleDES);
                client.TripleDES = tripleDES;
                client.IsOnline = true;
                while (IsConnected(clientSocket))
                {
                    Message message = ReceiveMessage(clientSocket, client.TripleDES);
                    HandleMessage(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            { 
                clientSocket.Close();
            }
        }

        private TripleDES KeyExchange(Socket client)
        {
            BigInteger privateKey = Encryption.GeneratePrivateKey();
            byte[] serverPublicKey = Encryption.GetPublicKey(privateKey);
            SendMessage(new ECDHMessage(ServerID, serverPublicKey), client);
            var response = (ECDHMessage)ReceiveMessage(client);
            byte[] sharedKey = Encryption.MultiplyByPrivateKey(response.PublicKey, privateKey);
            var tripleDES = new TripleDES { Key = sharedKey };
            SendSuccessMessage(client, tripleDES);
            return tripleDES;
        }

        private Client ClientAuthentication(Socket clientSocket, TripleDES tripleDES)
        {
            while (IsConnected(clientSocket))
            {
                var message = (AuthenticationMessage)ReceiveMessage(clientSocket, tripleDES);
                try
                {
                    return message.IsSigningUp ? SignUp(clientSocket, message, tripleDES) 
                        : SignIn(clientSocket, message, tripleDES);
                }
                catch (ArgumentException e)
                {
                    var error = Encoding.Default.GetBytes(e.Message);
                    var errorMessage = new ServiceMessage(ServerID, ServiceMessage.Type.Error, error);
                    SendMessage(errorMessage, clientSocket, tripleDES);
                }
            }
            throw new Exception();
        }

        private Client SignUp(Socket clientSocket, AuthenticationMessage message, TripleDES tripleDES)
        {
            int newUserID = Sql.AddUser(message.Username, message.Password, message.Username);
            SendMessage(new UserInfoMessage(ServerID, newUserID), clientSocket, tripleDES);
            var userInfo = (UserInfoMessage)ReceiveMessage(clientSocket, tripleDES);
            var client = new Client(clientSocket, userInfo.Name, newUserID);
            clients.TryAdd(newUserID, client);
            SendSuccessMessage(clientSocket, tripleDES);
            return client;
        }
        
        private Client SignIn(Socket clientSocket, AuthenticationMessage message, TripleDES tripleDES)
        {
            Sql.UserRecord user = Sql.GetUserByUsername(message.Username);
            if (user.ID == 0)
            {
                throw new ArgumentException("User not found");
            }
            if (user.Password != message.Password)
            {
                throw new ArgumentException("Incorrect password");
            }
            Client client = clients[user.ID];
            var userInfoMessage = new UserInfoMessage(ServerID, user.ID, client.Name, true);
            SendMessage(userInfoMessage, clientSocket, tripleDES);
            return client;
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

        private bool IsKeyExchangeCompleted((int Count, byte[] PublicKey)[] publicKeys)
        {
            for (var i = 0; i < publicKeys.Length; i++)
            {
                if (publicKeys[i].Count != -1)
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
                    client.SendIfOnline(new ServiceMessage(ServerID, ServiceMessage.Type.Success));
                }
            }
        }

        private void SendSuccessMessage(Socket client, TripleDES? tripleDES = null)
        {
            SendMessage(new ServiceMessage(ServerID, ServiceMessage.Type.Success), client, tripleDES);
        }

    }
}