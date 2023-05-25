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
        private readonly ConcurrentDictionary<int, List<Client>> chats = new() {};
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
            //Sql.Execute("ALTER TABLE Users ADD history_key VARBINARY(MAX) NOT NULL");
            //Sql.Execute("CREATE TABLE Files (fileID VARCHAR(64) NOT NULL, contents VARBINARY(MAX) NOT NULL);");
            Sql.DeleteUser("dmitriy");

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
                Console.WriteLine(user.ID + " " + user.Name + " " + user.Username);
                clients.TryAdd(user.ID, new Client(user.ID, user.Name, user.Username));
                clients[user.ID].IsOnline = false;
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
            Client? client = null;
            try
            {
                if (((ServiceMessage)ReceiveMessage(clientSocket)).MessageType != ServiceMessage.Type.Connecting)
                {
                    return;
                }
                TripleDES tripleDES = KeyExchange(clientSocket);
                client = ClientAuthorization(clientSocket, tripleDES);
                while (IsConnected(clientSocket))
                {
                    Message message = ReceiveMessage(clientSocket, tripleDES);
                    HandleMessage(message);
                }
                client.IsOnline = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"client {client?.Username} has disconnected.");
                Console.WriteLine(ex.StackTrace + ex.Message);
            }
            finally
            {
                if (client != null)
                {
                    lock (client.IsOnlineLock)
                    {
                        client.IsOnline = false;
                    }
                }
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

        private Client ClientAuthorization(Socket clientSocket, TripleDES tripleDES)
        {
            while (IsConnected(clientSocket))
            {
                var message = (AuthorizationMessage)ReceiveMessage(clientSocket, tripleDES);
                try
                {
                    Client client = message.IsSigningUp ? SignUp(clientSocket, message, tripleDES) 
                        : SignIn(clientSocket, message, tripleDES);
                    client.IsOnline = true;
                    client.TripleDES = tripleDES;
                    return client;
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

        private Client SignUp(Socket clientSocket, AuthorizationMessage message, TripleDES tripleDES)
        {
            int newUserID = Sql.AddUser(message.Username, message.Password, message.Username, message.HistoryKey);
            var reply = new UserInfoMessage(ServerID, newUserID, message.Username, message.Username);
            SendMessage(reply, clientSocket, tripleDES);
            var client = new Client(clientSocket, message.Username, message.Username, newUserID);
            clients.TryAdd(newUserID, client);
            return client;
        }
        
        private Client SignIn(Socket clientSocket, AuthorizationMessage message, TripleDES tripleDES)
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
            if (client.IsOnline)
            {
                throw new ArgumentException("You have already signed in");
            }
            client.Socket = clientSocket;
            Console.WriteLine($"{client.ID} {client.Name} has signed in.");
            var userInfoMessage = new UserInfoMessage(ServerID, user.ID, client.Username, client.Name, true);
            SendMessage(userInfoMessage, clientSocket, tripleDES);
            message.HistoryKey = user.HistoryKey;
            message.UnreadMessages = client.UnreadMessages.ToList();
            client.UnreadMessages.Clear();
            SendMessage(message, clientSocket, tripleDES);
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
                message.SenderID = ServerID;
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
                message.Created = true;
                foreach (int memberID in message.Members)
                {
                    Client client = clients[memberID];
                    Task.Run(() => { clients[memberID].SendIfOnline(message); });
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
            if (message.Contents.Length > 0)
            {
                Sql.AddFile(message.FileID, message.Contents);
                message.Contents = Array.Empty<byte>();
                HandleChatMessage(message);
            }
            else
            {
                Client client = clients[message.SenderID];
                message.Contents = Sql.GetFile(message.FileID);
                message.SenderID = ServerID;
                client.SendIfOnline(message);
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
                client.Socket.Disconnect(false);
            }
        }

        private void HandleUserInfoMessage(UserInfoMessage message)
        {
            if (message.SenderID == message.UserID)
            {
                ChangeName(message);
            }
            else
            {
                FindUser(message);
            }
        }

        private void ChangeName(UserInfoMessage message)
        {
            Client client = clients[message.SenderID];
            if (client.Name != message.Name)
            {
                lock (client.NameLock)
                {
                    client.Name = message.Name;
                }
                Sql.SetName(client.ID, client.Name);
            }
        }

        private void FindUser(UserInfoMessage message)
        {
            Client client = clients[message.SenderID];
            Sql.UserRecord user;
            if (message.Username != string.Empty)
            {
                user = Sql.GetUserByUsername(message.Username);
            }
            else
            {
                user = Sql.GetUserByID(message.UserID);
            }
            var response = new UserInfoMessage(ServerID, user.Username, user.ID, user.Name);
            if (clients.TryGetValue(user.ID, out var target))
            {
                response.IsOnline = target.IsOnline;
            }
            client.SendIfOnline(response);
        }

        private void SendSuccessMessage(Socket client, TripleDES? tripleDES = null)
        {
            SendMessage(new ServiceMessage(ServerID, ServiceMessage.Type.Success), client, tripleDES);
        }

    }
}