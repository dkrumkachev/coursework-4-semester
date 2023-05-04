using System.Net.Sockets;

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
        private const int ChunkSize = 1024;
        private 


        static void Main(string[] args)
        {
            
        }
    }
}