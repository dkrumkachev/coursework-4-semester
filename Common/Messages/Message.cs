using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Common.Encryption.Encryption;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Paddings;
using System.Xml.Serialization;
using Org.BouncyCastle.Crypto.Tls;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common.Messages 
{
    [Serializable]
    public abstract class Message
    {
        public int SenderID { get; set; }

        public DateTime Timestamp { get; set; }

        public Message(int senderID)
        {
            SenderID = senderID;
        }

#pragma warning disable SYSLIB0011
        public static byte[] Serialize(Message message)
        {
            var serializer = new BinaryFormatter();
            using var stream = new MemoryStream();
            serializer.Serialize(stream, message);
            return stream.ToArray();
        }

        public static Message Deserialize(byte[] bytes)
        {
            var serializer = new BinaryFormatter();
            using var stream = new MemoryStream(bytes);
            return (Message)serializer.Deserialize(stream);
        }
#pragma warning restore SYSLIB0011

    }

    [Serializable]
    public class ChatMessage : Message
    {
        public int ChatID { get; set; }
        
        public ChatMessage(int chatID, int senderID) : base(senderID)
        {
            ChatID = chatID;
        }
    }

    [Serializable]
    public class MessageWithContents : ChatMessage
    {
        public enum Type
        {
            Text, Image, File
        }

        public Type ContentsType { get; }

        public byte[] Contents { get; set; }

        public MessageWithContents(byte[] contents, Type contentsType, int chatID, int senderID) 
            : base(chatID, senderID)
        {
            Contents = contents;
            ContentsType = contentsType;
        }
    }

    [Serializable]
    public class FileMessage : MessageWithContents
    {
        public string FileName { get; }

        public string FileID { get; set; }

        public FileMessage(int chatID, int senderID, byte[] contents, string fileName, string fileID) 
            : base(contents, Type.File, chatID, senderID)
        {
            FileID = fileID;
            FileName = fileName;
        }

        public FileMessage(int chatID, int senderID, string fileName, string fileID)
            : base(Array.Empty<byte>(), Type.File, chatID, senderID)
        {
            FileID = fileID;
            FileName = fileName;
        }
    }

    [Serializable]
    public class ChatCreationMessage : ChatMessage 
    {
        public bool Created { get; set; } = false;
        
        public List<int> Members { get; }

        public string ChatName { get; set; }

        public (int Count, byte[] PublicKey)[] PublicKeys { get; }

        public int HopsNumber { get; set; }

        public ChatCreationMessage(List<int> users, int senderID, string name = "") : base(chatID: 0, senderID)
        {
            Members = users;
            PublicKeys = new (int, byte[])[users.Count];
            Array.Fill(PublicKeys, (0, Array.Empty<byte>()));
            ChatName = name;
        }
    }

    [Serializable]
    public class AuthorizationMessage : Message
    {
        public bool IsSigningUp { get; }

        public string Username { get; }

        public string Password { get; }

        public byte[] HistoryKey { get; set; }

        public List<ChatMessage> UnreadMessages { get; set; } = new();

        public AuthorizationMessage(int senderID, bool signingUp, string login, string password, byte[] historyKey) 
            : base(senderID)
        {
            IsSigningUp = signingUp;
            Username = login;
            Password = password;
            HistoryKey = historyKey;
        }
    }

    [Serializable]
    public class ServiceMessage : Message
    {
        public enum Type
        {
            Connecting,
            Disconnecting,
            Success,
            Error
        }

        public byte[]? AdditionalInfo { get; set; }

        public Type MessageType { get; }

        public ServiceMessage(int senderID, Type messageType, byte[]? additionalInfo = null) : base(senderID)
        {
            MessageType = messageType;
            AdditionalInfo = additionalInfo;
        }

    }

    [Serializable]
    public class ECDHMessage : Message 
    {
        public byte[] PublicKey { get; set; }

        public ECDHMessage (int senderID, byte[] publicKey) : base(senderID)
        {
            PublicKey = publicKey;
        }
    }

    [Serializable]
    public class UserInfoMessage : Message
    {
        public int UserID { get; }

        public string Username { get; }

        public string Name { get; }

        public bool IsOnline { get; set; } = true;

        public UserInfoMessage(int senderID, int userID, string username = "", string name = "", bool isOnline = true) 
            : base(senderID)
        {
            UserID = userID;
            Name = name;
            Username = username;
            IsOnline = isOnline;
        }

        public UserInfoMessage(int senderID, string username, int userID = 0, string name = "", bool isOnline = true) 
            : base(senderID)
        {
            UserID = userID;
            Name = name;
            Username = username;
            IsOnline = isOnline;
        }
    }

    [Serializable]
    public class MultipleUserInfoMessage : Message
    {
        public List<UserInfoMessage> Users { get; set; }

        public MultipleUserInfoMessage(int senderID, List<UserInfoMessage> users) : base(senderID)
        {
            Users = users;
        }
    }
}
