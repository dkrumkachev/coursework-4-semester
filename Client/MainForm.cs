using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = textBox1.Text;

            var b = 70;
            while (b < a.Length)
            {
                a = a.Insert(b, "\n");
                b += 70;
            }

            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            richTextBox1.SelectedText = a + "\n";
            richTextBox1.ScrollToCaret();
            richTextBox1.SelectedText = "\n";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var a = textBox1.Text;

            var b = 70;
            while (b < a.Length)
            {
                a = a.Insert(b, "\n");
                b += 70;
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            richTextBox1.SelectedText = a + "\n";
            richTextBox1.ScrollToCaret();
            richTextBox1.SelectedText = "\n";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            IDataObject? previousDataObject = Clipboard.GetDataObject();
            Image image = Image.FromFile("2.jpg");
            int maxWidth = 300;
            int maxHeight = 300;
            Size newSize = CalculateImageSize(image.Size, maxWidth, maxHeight);
            Image resizedImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
            }
            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Bitmap, resizedImage);
            Clipboard.SetImage(resizedImage);
            DataFormats.Format imageFormat = DataFormats.GetFormat(DataFormats.Bitmap);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            richTextBox1.ReadOnly = false;
            richTextBox1.Paste(imageFormat);
            richTextBox1.ReadOnly = true;
            if (previousDataObject != null)
            {
                Clipboard.SetDataObject(previousDataObject);
            }
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectedText = "\n";
            richTextBox1.ScrollToCaret();

        }
        private Size CalculateImageSize(Size originalSize, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / originalSize.Width;
            double ratioY = (double)maxHeight / originalSize.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalSize.Width * ratio);
            int newHeight = (int)(originalSize.Height * ratio);

            return new Size(newWidth, newHeight);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IDataObject? previousDataObject = Clipboard.GetDataObject();
            Image image = Image.FromFile("2.jpg");
            int maxWidth = 300;
            int maxHeight = 300;
            Size newSize = CalculateImageSize(image.Size, maxWidth, maxHeight);
            Image resizedImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
            }
            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Bitmap, resizedImage);
            Clipboard.SetImage(resizedImage);
            DataFormats.Format imageFormat = DataFormats.GetFormat(DataFormats.Bitmap);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            richTextBox1.ReadOnly = false;
            richTextBox1.Paste(imageFormat);
            richTextBox1.ReadOnly = true;
            if (previousDataObject != null)
            {
                Clipboard.SetDataObject(previousDataObject);
            }
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectedText = "\n";
            richTextBox1.ScrollToCaret();
        }
    }
}