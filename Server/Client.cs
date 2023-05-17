﻿using Common;
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

        public int ID { get; }

        public TripleDES TripleDES { get; set; }

        public List<int> Chats { get; } = new();

        public ConcurrentDictionary<int, List<byte[]>> History { get; } = new();

        public Client(Socket socket, string name, int id)
        {
            Socket = socket;
            Name = name;
            ID = id;
            TripleDES = new TripleDES();
            History.TryAdd(Constants.SelfChatID, new List<byte[]>());
        }

        public Client(int id, string name)
        {
            Socket = new Socket(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
            Name = name;
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
            if (message is not FileMessage && History.TryGetValue(message.ChatID, out List<byte[]>? chatHistory))
            {
                chatHistory.Add(JsonSerializer.SerializeToUtf8Bytes(message));
            }
        }

    }
}
