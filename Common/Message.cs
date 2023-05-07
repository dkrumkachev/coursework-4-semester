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
            Connect, Disconnect, Register, Authenticate
        }

        public Type MessageType { get; }

        public string Text { get; }

        public byte[] Contents { get; }

        public uint Sender { get; }

        public uint Chat { get; } 
        
        public Message(Type messageType, byte[] contents, string text = "", uint sender = 0, uint chat = 0) 
        {
            MessageType = messageType;
            Text = text;
            Contents = contents;
            Sender = sender;
            Chat = chat;
        }
    }

}
