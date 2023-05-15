﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Common.Encryption;
using Common.Messages;

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
                byte[] buffer = new byte[Constants.TransferBlockSize];
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
            return JsonSerializer.Deserialize<Message>(bytes) ?? throw new InvalidDataException();
        }

        public static int ReceiveSize(Socket socket)
        {
            byte[] size = new byte[sizeof(int)];
            socket.Receive(size);
            return BitConverter.ToInt32(size, 0);
        }

        public static void SendMessage(Message message, Socket socket, TripleDES? tripleDES = null)
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(message);
            if (tripleDES != null)
            {
                bytes = tripleDES.Encrypt(bytes);
            }
            socket.Send(BitConverter.GetBytes(bytes.Length));
            int start = 0;
            while (start + Constants.TransferBlockSize < bytes.Length)
            {
                socket.Send(bytes[start..(start + Constants.TransferBlockSize)]);
                start += Constants.TransferBlockSize;
            }
            socket.Send(bytes[start..]);
        }
    }
}
