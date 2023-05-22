using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [Serializable]
    public class DisplayedMessage
    {
        public enum Type
        {
            Text, Image, File
        }

        public string Sender { get; set; }

        public string? Text { get; set; }

        public Image? Image { get; set; }

        public string? Filename { get; set; }

        public string? FileID { get; set; }

        public Type MessageType { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public bool IsIncoming { get; set; }

        public DisplayedMessage(string sender, string text, bool isIncoming)
        {
            Sender = sender;
            Text = text;
            MessageType = Type.Text;
            IsIncoming = isIncoming;
        }

        public DisplayedMessage(string sender, Image image, bool isIncoming) 
        {
            Sender = sender;
            Image = image;
            MessageType = Type.Image;
            IsIncoming = isIncoming;
        }
        public DisplayedMessage(string sender, string filename, string fileID, bool isIncoming)
        {
            Sender = sender;
            Filename = filename;
            FileID = fileID;
            MessageType = Type.File;
            IsIncoming = isIncoming;
        }
    }
}
