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

        public ConcurrentDictionary<int, List<byte[]>> History { get; } = new();

        public Client(Socket socket, string name, string username, int id)
        {
            Socket = socket;
            Name = name;
            Username = username;
            ID = id;
            TripleDES = new TripleDES();
            History.TryAdd(Constants.SelfChatID, new List<byte[]>());
        }

        public Client(int id, string name, string username)
        {
            Socket = new Socket(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
            Name = name;
            Username = username;
            ID = id;
            TripleDES = new TripleDES();
            History.TryAdd(Constants.SelfChatID, new List<byte[]>());
        }

        public void SendIfOnline(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            bool isOnline;
            lock (IsOnlineLock)
            {
                isOnline = IsOnline;
            }
            if (message.SenderID != ID && isOnline)
            {
                lock (sendLock)
                {
                    SendMessage(message, Socket, TripleDES);
                }
            }
            if (message is ChatMessage userMessage)
            {
                SaveToHistory(userMessage);
            }
        }

        public void SaveToHistory(ChatMessage message)
        {
            if (message is FileMessage fileMessage && fileMessage.Contents.Length != 0)
            {
                return;
            }
            if (!History.TryGetValue(message.ChatID, out List<byte[]>? chatHistory))
            {
                History.TryAdd(message.ChatID, new List<byte[]>());
            }
            byte[] serializedMessage = Message.Serialize(message);
            History[message.ChatID].Add(serializedMessage);
        }

    }
}
