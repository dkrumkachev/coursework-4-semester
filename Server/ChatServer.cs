using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class ChatServer
    {
        private struct User
        {
            public User(Socket socket, string name)
            {
                Socket = socket;
                Name = name;
                Contacts = new List<uint>();
            }

            public Socket Socket { get; set; }

            public string Name { get; set; }

            public uint ID { get; set; }

            public List<uint> Contacts { get; }

        }

        private readonly ConcurrentDictionary<uint, User> users = new();
        private readonly ConcurrentDictionary<int, User[]> chats = new();
        private readonly ConcurrentDictionary<string, byte[]> files = new();
        private int chatID;


        
    }
}
