using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Common.SocketMethods;
using static Common.Messages.Message;
using Common.Messages;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Tls;
using Common.Encryption;
using Org.BouncyCastle.Math.EC;

namespace Client
{
    internal class Client
    {
        private readonly Socket server = new(Constants.AddressFamily, Constants.SocketType, Constants.ProtocolType);
        private int selfID;
        private TripleDES server3DES = new();

        private Dictionary<int, Chat> chats = new();

        public bool ConnectToServer()
        {
            try
            {
                server.Connect(new IPEndPoint(IPAddress.Parse(Constants.ServerIP), Constants.ServerPort));
                SendMessage(new ServiceMessage(0, ServiceMessage.Type.Connecting), server);
                if (!KeyExchange(server))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            return true;
        }

        private bool KeyExchange(Socket server)
        {
            var ecdhMessage = (ECDHMessage)ReceiveMessage(server);
            BigInteger privateKey = Encryption.GeneratePrivateKey();
            ECPoint sharedKey = ecdhMessage.PublicKey.Multiply(privateKey);
            ecdhMessage.PublicKey = ecdhMessage.DomainParameters.G.Multiply(privateKey);
            server3DES.Key = Encryption.GetSharedSecretBytes(sharedKey, TripleDES.KeySize);
            var successMessage = (ServiceMessage)ReceiveMessage(server, server3DES);
            return (successMessage.MessageType == ServiceMessage.Type.Success);
        }
    }
}
