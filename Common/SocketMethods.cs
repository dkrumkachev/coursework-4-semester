using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Common.Encryption;
using Common.Messages;
using Org.BouncyCastle.Crypto.Paddings;

namespace Common
{
    public static class SocketMethods
    {

        public static bool IsConnected(Socket socket)
        {
            const int TimeToWait = 500;
            return !(socket.Poll(TimeToWait, SelectMode.SelectRead) && socket.Available == 0);
        }

        public static Message ReceiveMessage(Socket socket, TripleDES? tripleDES = null)
        {
            int size = ReceiveSize(socket);
            byte[] bytes = new byte[size];
            int received = 0;
            while (received < size)
            {
                byte[] buffer = new byte[Math.Min(Constants.TransferBlockSize, size)];
                int bytesRead = socket.Receive(buffer);
                if (bytesRead == 0)
                {
                    throw new InvalidOperationException();
                }
                Buffer.BlockCopy(buffer, 0, bytes, received, bytesRead);
                received += bytesRead;
            } 
            if (tripleDES != null)
            {
                bytes = tripleDES.Encrypt(bytes, decrypt: true);
            }
            return Message.Deserialize(bytes);
        }

        public static int ReceiveSize(Socket socket)
        {
            byte[] bytes = new byte[sizeof(int)];
            if (socket.Receive(bytes) == 0) 
            {
                throw new SocketException();
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void SendMessage(Message message, Socket socket, TripleDES? tripleDES = null)
        {
            byte[] bytes = Message.Serialize(message);
            if (tripleDES != null)
            {
                bytes = tripleDES.Encrypt(bytes);
            }
            socket.Send(BitConverter.GetBytes(bytes.Length));
            var bytesLeftToTransmit = bytes.Length;
            int start = 0;
            while (bytesLeftToTransmit > 0)
            {
                var dataToSend = Math.Min(Constants.TransferBlockSize, bytesLeftToTransmit);
                var sendBuffer = bytes[start..(start + dataToSend)];
                start += dataToSend;
                bytesLeftToTransmit -= dataToSend;
                var offset = 0;
                while (dataToSend > 0)
                {
                    var bytesSent = socket.Send(sendBuffer, offset, dataToSend, SocketFlags.None);
                    dataToSend -= bytesSent;
                    offset += bytesSent;
                }
            }
            /* while (start + Constants.TransferBlockSize < bytes.Length)
             {
                 socket.Send(bytes[start..(start + Constants.TransferBlockSize)]);
                 start += Constants.TransferBlockSize;
             }
             socket.Send(bytes[start..]);*/
        }

    }
}
