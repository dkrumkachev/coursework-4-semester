using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using static Common.SocketMethods;


namespace Server
{
    internal class ChatServer
    {
        private readonly ConcurrentDictionary<int, User> users = new();
        private readonly ConcurrentDictionary<string, byte[]> files = new();


        private void HandleMessage(Message message)
        {
            switch (message.MessageType)
            {
                case Message.Type.Text:
                case Message.Type.Picture:
                    break;
                case Message.Type.FileContents:
                    break;
                case Message.Type.FileRequest:
                    break;
                case Message.Type.Disconnect:
                    break;
            }
        }

        private bool HandleTextAndPictureMessage(Message message)
        {
            try
            {
                User sender = users[message.SenderID];
                List<User> receivers = sender.Chats[message.ChatID];
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

        private bool HandleCreateChatMessage(Message message)
        {
            try
            {
                User sender = users[message.SenderID];
                List<User> chatMembers = new List<User>();
                for (var i = 0; i < message.Contents.Length; i += sizeof(int))
                {
                    int userID = BitConverter.ToInt32(message.Contents, i);
                    chatMembers.Add(users[userID]);
                }
                sender.CreateNewChat(chatMembers);
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;
        }

    }
}
