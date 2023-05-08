using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Message
    {
        public enum Type
        {
            Text, Picture, FileRequest, FileInfo, FileContents, 
            CreateChat, Connect, Disconnect, Register, Authenticate, 
        }

        public Type MessageType { get; }

        public byte[] Contents { get; }

        public int SenderID { get; }

        public int ChatID { get; } 
        
        public DateTime Timestamp { get; set; }

        public Message(Type messageType, byte[] contents, int senderID = 0, int chatID = 0) 
        {
            MessageType = messageType;
            Contents = contents;
            SenderID = senderID;
            ChatID = chatID;
        }
    }

}
