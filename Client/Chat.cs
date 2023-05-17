using Common.Encryption;
using Common.Messages;
using System.Net.Sockets;
using Message = Common.Messages.Message;
using static Common.SocketMethods;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;

namespace Client
{
    internal class Chat
    {
        public int ID { get; set; }

        public List<int> Members { get; }

        public TripleDES TripleDES { get; set; } = new();

        public Dictionary<string, string> Files { get; set; } = new();

        public BigInteger PrivateKey { get; set; } = Encryption.GeneratePrivateKey();

        public Chat(int id, List<int> members) 
        {
            ID = id;
            Members = members;
        }

        public void Send(ChatMessage message, Socket server)
        {
            message.ChatID = ID;
            SendMessage(message, server, TripleDES);
        }

    }
}
