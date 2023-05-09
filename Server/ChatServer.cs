using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Messages;
using static Common.Methods;


namespace Server
{
    internal class ChatServer
    {
        
        private readonly ConcurrentDictionary<int, User> users = new();
        private readonly ConcurrentDictionary<int, List<User>> chats = new();
        private int lastCreatedChat = 0;
        private readonly ConcurrentDictionary<string, byte[]> files = new();

        private void HandleMessage(Message message)
        {
            if (message is ConnectionMessage)
            {

            }
            else if (message is ChatCreationMessage)
            {
                HandleChatCreationMessage((ChatCreationMessage)message);
            }
            else if (message is FileInfoMessage)
            {

            }
            else if (message is FileMessage)
            {

            }
            else if (message is ChatMessage)
            {
                HandleUserMessage((ChatMessage)message);
            }       
        }

        private bool HandleUserMessage(ChatMessage message)
        {
            try
            {
                User sender = users[message.SenderID];
                List<User> receivers = chats[message.ChatID];
                foreach (var receiver in receivers)
                {
                    receiver.SendIfOnline(message);
                }
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;
        }

        private bool HandleChatCreationMessage(ChatCreationMessage message)
        {
            try
            {
                User sender = users[message.SenderID];
                if (message.ChatID == 0)
                {
                    List<User> chatMembers = new List<User>();
                    foreach (var userID in message.UserIDs)
                    {
                        chatMembers.Add(users[userID]);
                    }
                    int chatID = Interlocked.Increment(ref lastCreatedChat);
                    chats.TryAdd(chatID, chatMembers);
                    message.ChatID = chatID;
                }
                foreach (var member in chats[message.ChatID])
                {
                    member.SendIfOnline(message);
                }
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;
        }

    }
}
