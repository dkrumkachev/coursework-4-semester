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

namespace Common.Messages
{
    [JsonDerivedType(typeof(ChatMessage), typeDiscriminator: "chat")]
    [JsonDerivedType(typeof(MessageWithContents), typeDiscriminator: "withContacts")]
    [JsonDerivedType(typeof(FileMessage), typeDiscriminator: "file")]
    [JsonDerivedType(typeof(FileInfo), typeDiscriminator: "fileInfo")]
    [JsonDerivedType(typeof(ChatCreationMessage), typeDiscriminator: "chatCreation")]
    [JsonDerivedType(typeof(AuthenticationMessage), typeDiscriminator: "authentication")]
    [JsonDerivedType(typeof(ServiceMessage), typeDiscriminator: "service")]
    public abstract class Message
    {
        public int SenderID { get; }

        public DateTime Timestamp { get; set; }

        public Message(int senderID)
        {
            SenderID = senderID;
        }
    }

    public class ChatMessage : Message
    {
        public int ChatID { get; set; }
        
        public ChatMessage(int chatID, int senderID) : base(senderID)
        {
            ChatID = chatID;
        }
    }

    public class MessageWithContents : ChatMessage
    {
        public byte[] Contents { get; }

        public MessageWithContents(byte[] contents, int chatID, int senderID) : base(chatID, senderID)
        {
            Contents = contents;
        }
    }

    public class FileMessage : MessageWithContents
    {
        public string FileName { get; }

        public FileMessage(string fileName, byte[] contents, int chatID, int senderID) 
            : base(contents, chatID, senderID)
        {
            FileName = fileName;
        }
    }

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

    public class ChatCreationMessage : ChatMessage 
    {
        public List<int> Members { get; }

        public (int Count, BigInteger PublicKey)[] PublicKeys { get; }

        public DHParameters DHParams { get; set; }

        public int HopsNumber { get; set; }

        public ChatCreationMessage(List<int> users, int senderID) : base(chatID: 0, senderID)
        {
            Members = users;
            PublicKeys = new (int, BigInteger)[users.Count];
            Array.Fill(PublicKeys, (0, 0));
        }

        /* Client: 
        for (var i = 0; i < PublicKeys.Length; i++)
        {
            if (PublicKeys[i].Count == 0)
            {
                PublicKeys[i].Count = 1;
                PublicKeys[i].PublicKey = g ^ privateKey;
                break;
            }
            if (PublicKeys[i].Count != -1)
            {
                PublicKeys[i].Count += 1;
                if (PublicKeys[i].Count == Users.Count) 
                {
                    CryptoKey = PublicKeys[i].PublicKey ^ privateKey;
                    PublicKeys[i].Count = -1;
                    PublicKeys[i].PublicKey = 0;
                }
                else
                {
                    PublicKeys[i].PublicKey ^= privateKey;
                }
            }
        }*/

    }

    public class AuthenticationMessage : Message
    {
        public bool IsSigningUp { get; }

        public string Email { get; }

        public string Password { get; }

        public AuthenticationMessage(int senderID, bool signingUp, string email, string password) : base(senderID)
        {
            IsSigningUp = signingUp;
            Email = email;
            Password = password;
        }
    }

    public class ServiceMessage : Message
    {
        public enum Type
        {
            Connecting,
            Disconnecting,
            DiffieHellman,
            Success,
            Error
        }

        public DHParameters DHParameters { get; set; }

        public byte[]? AdditionalInfo { get; set; }

        public Type MessageType { get; }

        public ServiceMessage(int senderID, Type messageType, byte[]? additionalInfo = null, 
            DHParameters DHParams = default) : base(senderID)
        {
            MessageType = messageType;
            AdditionalInfo = additionalInfo;
            DHParameters = DHParams;
        }

    }

    public class UserInfoMessage : Message
    {
        public int UserID { get; }
        
        public string Name { get; }

        public bool IsOnline { get; }

        public Dictionary<int, List<int>> Chats { get; }

        public UserInfoMessage(int senderID, int userID, string name = "", bool isOnline = true) : base(senderID)
        {
            UserID = userID;
            Name = name;
            IsOnline = isOnline;
        }
    }



}
