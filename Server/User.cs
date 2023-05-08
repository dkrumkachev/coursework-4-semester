using Common;
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
    internal class User
    {
        public bool IsOnline { get; set; }

        public Socket Socket { get; set; }

        public string Name { get; set; }

        public int ID { get; }

        public ConcurrentDictionary<int, List<User>> Chats { get; }

        private int newChatID;

        public ConcurrentDictionary<int, List<byte[]>> History { get; }


        public User(Socket socket, string name, int id)
        {
            Socket = socket;
            IsOnline = true;
            Name = name;
            ID = id;
            Chats = new ConcurrentDictionary<int, List<User>>();
            Chats.TryAdd(Constants.SelfChatID, new List<User>() { this });
            newChatID = 1;
            History = new ConcurrentDictionary<int, List<byte[]>>();
            History.TryAdd(Constants.SelfChatID, new List<byte[]>());
        }

        public void SendIfOnline(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            if (message.ChatID != Constants.SelfChatID && IsOnline)
            {
                SendMessage(message, Socket);
            }
            SaveToHistory(message);
        }

        public void SaveToHistory(Message message)
        {
            if (History.TryGetValue(message.ChatID, out List<byte[]>? chatHistory))
            {
                chatHistory.Add(JsonSerializer.SerializeToUtf8Bytes(message));
            }
        }

        public int CreateNewChat(List<User> chatMembers)
        {
            int chatID = newChatID;
            Chats.TryAdd(chatID, chatMembers);
            foreach (var chatMember in chatMembers) 
            {
                
                chatMember.Chats.TryAdd(chatID);   
            }
            return Interlocked.Increment(ref newChatID);
        }
    }
}
