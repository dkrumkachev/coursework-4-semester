using Common;
using Common.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Common.Methods;

namespace Server
{
    internal class User
    {
        public bool IsOnline { get; set; }

        public Socket Socket { get; set; }

        public string Name { get; set; }

        public int ID { get; }

        private int newChatID;

        public ConcurrentDictionary<int, List<byte[]>> History { get; }


        public User(Socket socket, string name, int id)
        {
            Socket = socket;
            IsOnline = true;
            Name = name;
            ID = id;
            newChatID = 1;
            History = new ConcurrentDictionary<int, List<byte[]>>();
            History.TryAdd(Constants.SelfChatID, new List<byte[]>());
        }

        public void SendIfOnline(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            if (message.SenderID != ID && IsOnline)
            {
                SendMessage(message, Socket);
            }
            if (message is ChatMessage userMessage)
            {
                SaveToHistory(userMessage);
            }
        }

        public void SaveToHistory(ChatMessage message)
        {
            if (History.TryGetValue(message.ChatID, out List<byte[]>? chatHistory))
            {
                chatHistory.Add(JsonSerializer.SerializeToUtf8Bytes(message));
            }
        }

    }
}
