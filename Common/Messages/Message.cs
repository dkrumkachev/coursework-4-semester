using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Messages
{
    [JsonDerivedType(typeof(ChatMessage), typeDiscriminator: "chat")]
    [JsonDerivedType(typeof(MessageWithContents), typeDiscriminator: "withContacts")]
    [JsonDerivedType(typeof(FileMessage), typeDiscriminator: "file")]
    [JsonDerivedType(typeof(FileInfo), typeDiscriminator: "fileInfo")]
    [JsonDerivedType(typeof(ChatCreationMessage), typeDiscriminator: "chatCreation")]
    [JsonDerivedType(typeof(AuthenticationMessage), typeDiscriminator: "authentication")]
    [JsonDerivedType(typeof(ConnectionMessage), typeDiscriminator: "connection")]
    public abstract class Message
    {
        /*public enum Type
        {
            Text, Picture, FileRequest, FileInfo, FileContents,
            CreateChat, Connect, Disconnect, Register, Authenticate,
        }

        public Type MessageType { get; }

        public byte[] Contents { get; }

        public int ChatID { get; }*/

        public int SenderID { get; }

        public DateTime Timestamp { get; set; }

        public Message(int senderID)
        {
            SenderID = senderID;
        }

       /* public Message(Type messageType, byte[] contents, int senderID = 0, int chatID = 0)
        {
            MessageType = messageType;
            Contents = contents;
            SenderID = senderID;
            ChatID = chatID;
        }*/
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
        public int FileID { get; }

        public string FileName { get; }

        public FileInfoMessage(int fileID, string filename, int chatID, int senderID)
            : base(chatID, senderID)
        {
            FileID = fileID;
            FileName = filename;
        }
    }

    public class ChatCreationMessage : ChatMessage 
    {
        public List<int> UserIDs { get; }

        public Dictionary<int, int> OpenKeys { get; set; }

        public ChatCreationMessage(List<int> userIDs, Dictionary<int, int> openKeys, int senderID, int chatID = 0) 
            : base(chatID, senderID)
        {
            UserIDs = userIDs;
            OpenKeys = openKeys;
        }
    }

    public class AuthenticationMessage : Message
    {
        public bool IsRegistration { get; }

        public string Login { get; }

        public string Password { get; }

        public AuthenticationMessage(int senderID, bool registration, string login, string password) : base(senderID)
        {
            IsRegistration = registration;
            Login = login;
            Password = password;
        }
    }

    public class ConnectionMessage : Message
    {
        public bool Disconnecting { get; }

        public ConnectionMessage(int senderID, bool disconnecting) : base(senderID)
        {
            Disconnecting = disconnecting;
        }
    }

}
