using Common.Encryption;
using Common.Messages;
using System.Net.Sockets;
using Message = Common.Messages.Message;
using static Common.SocketMethods;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;

namespace Client
{
    [Serializable]
    internal class Chat
    {
        public readonly object filesLock = new object();

        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<int> Members { get; }

        public TripleDES TripleDES { get; set; } = new();

        public Dictionary<string, byte[]> Files { get; set; } = new();

        public BigInteger PrivateKey { get; set; } = Encryption.GeneratePrivateKey();

        public Chat(int id, List<int> members, string name = "") 
        {
            ID = id;
            Members = members;
            Name = name;
        }

    }
}
