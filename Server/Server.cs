using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Common;

namespace Server
{
    internal class Server
    {
        private struct Client
        {
            public Client(Socket socket, string name)
            {
                Socket = socket;
                Name = name;
            }

            public Socket Socket { get; }

            public string Name { get; set; }
        }

        private readonly List<Client> clients = new();
        private readonly Dictionary<int, byte[]> files = new();
        private const int MaxConnectionsQueueSize = 5;
        private readonly Socket server;  


        public Server()
        {
            server = new Socket(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
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
            WaitForConnections(server);
        }

        private void WaitForConnections(Socket server)
        {
            while (true)
            {
                Task.Run(CommunicateWithClient);
            }
        }

        private void CommunicateWithClient()
        {
            Socket client = server.Accept();
            while (SocketMethods.IsConnected(client))
            {
                
            }
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
    }
}