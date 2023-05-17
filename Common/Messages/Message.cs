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

namespace Common.Messages 
{
    [Serializable]
    public abstract class Message
    {
        public int SenderID { get; }

        public DateTime Timestamp { get; set; }

        public Message(int senderID)
        {
            SenderID = senderID;
        }
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

        public byte[] Contents { get; }

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

        public FileMessage(string fileName, byte[] contents, int chatID, int senderID) 
            : base(contents, Type.File, chatID, senderID)
        {
            FileName = fileName;
        }
    }

    [Serializable]
    public class FileInfoMessage : ChatMessage
    {
        public string FileID { get; }

        public string FileName { get; }

        public FileInfoMessage(string fileID, string filename, int chatID, int senderID)
            : base(chatID, senderID)
        {
            FileID = fileID;
            FileName = filename;
        }
    }

    [Serializable]
    public class ChatCreationMessage : ChatMessage 
    {
        public List<int> Members { get; }

        public (int Count, byte[] PublicKey)[] PublicKeys { get; }

        public int HopsNumber { get; set; }

        public ChatCreationMessage(List<int> users, int senderID) : base(chatID: 0, senderID)
        {
            Members = users;
            PublicKeys = new (int, byte[])[users.Count];
            Array.Fill(PublicKeys, (0, Array.Empty<byte>()));
        }
    }

    [Serializable]
    public class AuthenticationMessage : Message
    {
        public bool IsSigningUp { get; }

        public string Username { get; }

        public string Password { get; }

        public AuthenticationMessage(int senderID, bool signingUp, string login, string password) : base(senderID)
        {
            IsSigningUp = signingUp;
            Username = login;
            Password = password;
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

        public string Name { get; }

        public bool IsOnline { get; } = true;

        public UserInfoMessage(int senderID, int userID, string name = "", bool isOnline = true) : base(senderID)
        {
            UserID = userID;
            Name = name;
            IsOnline = isOnline;
        }
    }
}
