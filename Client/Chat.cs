using Common.Encryption;
using Common.Messages;
using System.Net.Sockets;
using Message = Common.Messages.Message;
using static Common.SocketMethods;

namespace Client
{
    internal class Chat
    {
        public int ID { get; set; }

        public TripleDES TripleDES { get; set; } = new();

        public Dictionary<string, string> Files { get; set; } = new();

        public Chat(int id) 
        {
            ID = id;
        }

        public void Send(ChatMessage message, Socket server)
        {
            message.ChatID = ID;
            SendMessage(message, server, TripleDES);
        }

    }
}
