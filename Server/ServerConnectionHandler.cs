using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Common;
using static Common.SocketMethods;

namespace Server
{
    internal class ServerConnectionHandler
    {
        private const int MaxConnectionsQueueSize = 5;
        private readonly Socket server = new(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
        private readonly ChatServer chatServer = new ChatServer();

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
            try
            {
                if (ReceiveMessage(client).MessageType != Message.Type.Connect)
                {
                    return;
                }
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

        private void ClientAuthentication(Socket client)
        {
            while (IsConnected(client))
            {
                Message message = ReceiveMessage(client);
                if (message.MessageType == Message.Type.Register)
                {
                    return;
                }
                else if (message.MessageType == Message.Type.Authenticate)
                {
                    return;
                }
            }   
        }

        

    }
}