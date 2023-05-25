using Common;
using Common.Encryption;
using Common.Messages;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Common.SocketMethods;

namespace Server
{
    internal class Client
    {
        public readonly object IsOnlineLock = new();
        public readonly object NameLock = new();
        private static readonly object sendLock = new();

        public bool IsOnline { get; set; } = true;

        public Socket Socket { get; set; }

        public string Name { get; set; }

        public string Username { get; }

        public int ID { get; }

        public TripleDES TripleDES { get; set; }

        public List<int> Chats { get; } = new();

        public ConcurrentBag<ChatMessage> UnreadMessages { get; } = new();

        public Client(Socket socket, string name, string username, int id)
        {
            Socket = socket;
            Name = name;
            Username = username;
            ID = id;
            TripleDES = new TripleDES();
        }

        public Client(int id, string name, string username)
        {
            Socket = new Socket(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
            Name = name;
            Username = username;
            ID = id;
            TripleDES = new TripleDES();
        }

        public void SendIfOnline(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            bool isOnline;
            lock (IsOnlineLock)
            {
                isOnline = IsOnline;
            }
            if (message.SenderID != ID)
            {
                if (isOnline)
                {
                    lock (sendLock)
                    {
                        SendMessage(message, Socket, TripleDES);
                    }
                }
                else if (message is ChatMessage chatMessage) 
                {
                    SaveMessage(chatMessage);
                }
            }
        }

        public void SaveMessage(ChatMessage message)
        {
            if (message is FileMessage fileMessage && fileMessage.Contents.Length != 0)
            {
                return;
            }
            UnreadMessages.Add(message);
        }

    }
}
