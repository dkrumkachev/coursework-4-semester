using Common.Encryption;
using Common.Messages;
using Org.BouncyCastle.Asn1.Cms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Image = System.Drawing.Image;

namespace Client
{
    public partial class MainForm : Form
    {
        private readonly Client client;
        private const string SelfChatName = Client.SelfChatName;
        private readonly string signInFilePath = "save.txt";
        private string historyPath = string.Empty;
        private int selectedChat = -1;
        private const int space = 5;
        private Color ownMessageColor = Color.DodgerBlue;
        private Color othersMessageColor = Color.FromArgb(16, 53, 53);
        private Image fileImage = Image.FromFile("64.png");
        private byte[] messageHistoryKey = Array.Empty<byte>();
        private readonly Dictionary<int, List<Panel>> chatMessages = new() { { 0, new List<Panel>() } };
        private Dictionary<int, List<DisplayedMessage>> history = new() { { 0, new List<DisplayedMessage>() } };
        private readonly Dictionary<int, int> nextMessageY = new() { { 0, space * 2 } };
        private readonly Dictionary<int, (int Day, int Month)> lastMessageDate = new() { { 0, (0, 0) } };

        public MainForm()
        {
            if (!Directory.Exists("history"))
            {
                Directory.CreateDirectory("history");
            }
            client = new Client(ChatCreated, IncomingTextMessage, IncomingImageMessage, IncomingFileMessage);
            if (!File.Exists(signInFilePath))
            {
                File.Create(signInFilePath);
            }
            InitializeComponent();
            Size = new Size(1600, 900);
        }

        private void CenterControlOnForm(Control control)
        {
            control.Location = new Point((ClientSize.Width - control.Width) / 2,
                (ClientSize.Height - control.Height) / 2);
        }

        private void CenterControl(Control control, Control container)
        {
            control.Location = new Point((container.Width - control.Width) / 2,
                (container.Height - control.Height) / 2);
        }

        private void ConnectToServer()
        {
            client.WaitForConnectionToServer();
            Invoke(() =>
            {
                loadingLabel.Visible = false;
                if (!Authorization())
                {
                    authenticationPanel.Visible = true;
                }
            });
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Task.Run(ConnectToServer);
            mainPanel.Controls.Add(chooseChatTypePanel);
            mainPanel.Controls.Add(createSingleChatPanel);
            createSingleChatPanel.Location = leftPanel.Location;
            chooseChatTypePanel.Location = leftPanel.Location;
            inputChatNamePanel.Location = leftPanel.Location;
            addMembersPanel.Location = leftPanel.Location;
            addMembersPanel.Height = leftPanel.Height;
            addedMembersLabel.Height = addMembersPanel.Height - addedMembersLabel.Top;
            chatButtonLocation = chatPanel.Location;
            CenterControl(initialChatPanelLabel, chatPanel);
            CenterControlOnForm(authenticationPanel);
            CenterControlOnForm(nameInputPanel);
            CenterControlOnForm(loadingLabel);
            CenterControlOnForm(waitingLabel);
            AddScrollBar(chatPanel);
            AddScrollBar(leftPanel);
            AddScrollBar(membersPanel);
            AddScrollBar(filesPanel);
        }

        private void AddScrollBar(Panel panel)
        {
            panel.AutoScroll = false;
            panel.HorizontalScroll.Enabled = false;
            panel.HorizontalScroll.Visible = false;
            panel.HorizontalScroll.Maximum = 0;
            panel.AutoScroll = true;
        }

#pragma warning disable SYSLIB0011
        private void ReadHistoryFromFile()
        {
            if (File.Exists(historyPath))
            {
                client.Restore($"{historyPath}chats");
                chatMessages.Clear();
                chats.Clear();
                nextMessageY.Clear();
                lastMessageDate.Clear();
                byte[] chatHistories = File.ReadAllBytes(historyPath);
                var tripleDES = new TripleDES()
                {
                    Key = messageHistoryKey,
                };
                chatHistories = tripleDES.Encrypt(chatHistories, true);
                var serializer = new BinaryFormatter();
                using var stream = new MemoryStream(chatHistories);
                history = (Dictionary<int, List<DisplayedMessage>>)serializer.Deserialize(stream);
                RestoreHistory();
            }
            else
            {
                chats.Add(AddButtonForChat(SelfChatName), 0);
            }
        }

        private void SaveHistory()
        {
            if (historyPath != string.Empty)
            {
                var serializer = new BinaryFormatter();
                using var stream = new MemoryStream();
                serializer.Serialize(stream, history);
                byte[] bytes = stream.ToArray();
                var tripleDES = new TripleDES()
                {
                    Key = messageHistoryKey,
                };
                bytes = tripleDES.Encrypt(bytes);
                File.WriteAllBytes(historyPath, bytes);
                client.Save($"{historyPath}chats");
            }
        }
#pragma warning restore SYSLIB0011

        private void RestoreHistory()
        {
            foreach (var keyValue in history)
            {
                int chatID = keyValue.Key;
                string chatname = client.GetChatName(chatID) ?? string.Empty;
                chatMessages.Add(chatID, new List<Panel>());
                nextMessageY.Add(chatID, space * 2);
                lastMessageDate.Add(chatID, (0, 0));
                foreach (object msg in keyValue.Value)
                {
                    try
                    {
                        var message = (DisplayedMessage)msg;
                        if (message.MessageType == DisplayedMessage.Type.Text && message.Text != null)
                        {
                            CreatePanel(message.Sender, message.Text, message.IsIncoming, chatID);
                        }
                        else if (message.MessageType == DisplayedMessage.Type.Image && message.Image != null)
                        {
                            CreatePanel(message.Sender, message.Image, message.IsIncoming, chatID);
                        }
                        else if (message.MessageType == DisplayedMessage.Type.File &&
                            message.Filename != null && message.FileID != null)
                        {
                            CreatePanel(message.Sender, message.Filename, message.FileID, message.IsIncoming, chatID);
                        }
                    }
                    catch { }
                }
                chats.Add(AddButtonForChat(chatname), chatID);
            }
        }

        private Label CreateLabel(Color backColor, double fontSize = 13.8)
        {
            var label = new Label
            {
                BackColor = backColor,
                BorderStyle = BorderStyle.None,
                Size = new Size(space, space),
                MaximumSize = new Size(500, 600),
                MinimumSize = new Size(0, 20),
                Font = new Font("Segoe UI", (float)fontSize)
            };
            return label;
        }

        private void AddControl(Control parent, Control control, string text, int x, int y)
        {
            parent.Controls.Add(control);
            if (control is Label label)
            {
                label.AutoSize = true;
            }
            if (text != string.Empty)
            {
                control.Text = text;
            }
            control.Location = new Point(x, y);
        }

        private PictureBox CreatePictureBox(Image image)
        {
            Size maxImageSize = new(600, 600);
            PictureBoxSizeMode sizemode;
            if (image.Size.Width <= maxImageSize.Width && image.Size.Height <= maxImageSize.Height)
            {
                sizemode = PictureBoxSizeMode.AutoSize;
            }
            else
            {
                sizemode = PictureBoxSizeMode.Zoom;
            }
            var pictureBox = new PictureBox()
            {
                SizeMode = sizemode,
                ClientSize = maxImageSize,
                BorderStyle = BorderStyle.None,
                Image = image,
            };
            pictureBox.MouseClick += PictureBox_MouseClick;
            return pictureBox;
        }

        private void PictureBox_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is PictureBox pictureBox && e.Button == MouseButtons.Left)
            {
                saveFileDialog.Filter = "JPEG Image(*.jpg)|*.jpg|All files(*.*)|*.*";
                saveFileDialog.DefaultExt = ".png";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

        private PictureBox CreatePictureBoxForFile(string fileID)
        {
            var pictureBox = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.AutoSize,
                BorderStyle = BorderStyle.None,
                Image = (Image)fileImage.Clone(),
                Name = fileID,
                Cursor = Cursors.Hand,
            };
            pictureBox.MouseClick += FilePictureBox_MouseClick;
            return pictureBox;
        }

        private void FilePictureBox_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is PictureBox pictureBox && pictureBox.Parent != null)
            {
                try
                {
                    var parentPanel = (Panel)pictureBox.Parent;
                    var filenameLabel = (Label)parentPanel.Controls[2];
                    string filename = filenameLabel.Text;
                    filename = filename.Remove(filename.LastIndexOf(" "));
                    string fileID = pictureBox.Name;
                    string? path = ShowSaveFileDialog(filename);
                    if (path != null)
                    {
                        pictureBox.Enabled = false;
                        Task.Run(() =>
                        {
                            byte[] file = client.GetFile(selectedChat, filename, fileID);
                            File.WriteAllBytes(path, file);
                        });
                        pictureBox.Enabled = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Error downloading file");
                }
            }
        }

        private Panel CreateDatePanel(string date, int chat)
        {
            var panel = new Panel()
            {
                BackColor = Color.DarkSlateGray,
                BorderStyle = BorderStyle.None,
            };
            Label dateLabel = CreateLabel(Color.DarkSlateGray, 13.8);
            dateLabel.Font = new Font("Futura Md Bt", (float)13.8);
            dateLabel.MaximumSize = new Size(chatPanel.Width, 25);
            panel.Controls.Add(dateLabel);
            dateLabel.AutoSize = false;
            dateLabel.Size = dateLabel.MaximumSize;
            dateLabel.TextAlign = ContentAlignment.MiddleCenter;
            dateLabel.Text = date;
            dateLabel.Location = new Point(0, 0);
            panel.Size = dateLabel.Size;
            panel.Location = new Point(0, nextMessageY[chat]);
            nextMessageY[chat] += panel.Height + space;
            return panel;
        }

        private (Panel?, Panel) CreatePanel(Control control, string sender, string text, bool isIncoming, int chat)
        {
            Panel? datePanel = null;
            if (lastMessageDate[chat].Day < DateTime.Today.Day ||
                lastMessageDate[chat].Month < DateTime.Today.Month)
            {
                datePanel = CreateDatePanel(DateTime.Now.ToShortDateString(), chat);
                lastMessageDate[chat] = (DateTime.Today.Day, DateTime.Today.Month);
                chatMessages[chat].Add(datePanel);
            }
            Color backColor = isIncoming ? othersMessageColor : ownMessageColor;
            var panel = new Panel()
            {
                BackColor = backColor,
                BorderStyle = BorderStyle.None,
            };
            Label nameLabel = CreateLabel(backColor, 12);
            Label timeLabel = CreateLabel(backColor, 9);
            Label textLabel = CreateLabel(backColor);
            AddControl(panel, nameLabel, sender, space, space);
            AddControl(panel, control, "", space, space + nameLabel.Height + space);
            int width = Math.Max(nameLabel.Width, control.Width);
            int y = space * 2 + nameLabel.Height + control.Height;
            if (text != string.Empty)
            {
                AddControl(panel, textLabel, text, space, y);
                y += textLabel.Height;
                width = Math.Max(width, textLabel.Width);
            }
            AddControl(panel, timeLabel, DateTime.Now.ToShortTimeString(), space, y);
            width = Math.Max(width, timeLabel.Width) + space * 2;
            int height = y + timeLabel.Height + space * 2;
            panel.Size = new Size(width, height);
            int x = isIncoming ? 30 : chatPanel.Width - panel.Width - 30;
            panel.Location = new Point(x, nextMessageY[chat]);
            nextMessageY[chat] += panel.Height + space;
            chatMessages[chat].Add(panel);
            return (datePanel, panel);
        }

        private (Panel?, Panel) CreatePanel(string sender, string message, bool isIncoming, int chat)
        {
            Color backColor = isIncoming ? othersMessageColor : ownMessageColor;
            Label msgLabel = CreateLabel(backColor);
            msgLabel.Text = message;
            return CreatePanel(msgLabel, sender, "", isIncoming, chat);
        }

        private (Panel?, Panel) CreatePanel(string sender, Image image, bool isIncoming, int chat)
        {
            PictureBox pictureBox = CreatePictureBox(image);
            return CreatePanel(pictureBox, sender, "", isIncoming, chat);
        }

        private (Panel?, Panel) CreatePanel(string sender, string filename, string fileID, bool isIncoming, int chat)
        {
            PictureBox pictureBox = CreatePictureBoxForFile(fileID);
            return CreatePanel(pictureBox, sender, filename, isIncoming, chat);
        }

        private void ScrollChatDown()
        {
            using var control = new Control() { Parent = chatPanel, Dock = DockStyle.Bottom };
            chatPanel.ScrollControlIntoView(control);
            control.Parent = null;
        }

        private void DisplayMessage(Panel panel)
        {
            panel.Location = new Point(panel.Location.X, panel.Location.Y + chatPanel.AutoScrollPosition.Y);
            chatPanel.Controls.Add(panel);
        }

        private void DisplayMessageAndScroll(Panel panel)
        {
            DisplayMessage(panel);
            ScrollChatDown();
        }

        private void DisplayChat(int chatID)
        {
            List<(string Username, string Name)> members = client.GetChatMembers(chatID);
            if (members.Count == 1)
            {
                chatInfoLabel.Text = string.Empty;
            }
            else if (members.Count == 2)
            {
                (string Username, string Name) user = members.Find(i => i.Username != client.SelfUsername);
                chatInfoLabel.Text = $"{user.Name}\n\nusername: {user.Username}";
            }
            else
            {
                chatNameLabel.Text += $" ({members.Count} members)";
                var membersInfo = new StringBuilder();
                foreach ((string Username, string Name) member in members)
                {
                    membersInfo.Append($"{member.Name} ({member.Username})\n");
                }
                chatInfoLabel.Text = membersInfo.ToString();
            }
            waitingLabel.Visible = false;
            messageTextBox.Enabled = true;
            fileButton.Enabled = true;
            chatInfoLabel.Visible = true;
            foreach (Panel panel in chatMessages[chatID])
            {
                DisplayMessage(panel);
            }
            ScrollChatDown();
        }

        private void DisplayChatWaitingForUsers()
        {
            waitingLabel.Visible = true;
            waitingLabel.Text = "Waiting for all users to connect...";
            messageTextBox.Enabled = false;
            fileButton.Enabled = false;
            chatInfoLabel.Visible = false;
        }

        private void DisplayPanels((Panel?, Panel) panels)
        {
            if (panels.Item1 != null)
            {
                DisplayMessage(panels.Item1);
            }
            DisplayMessageAndScroll(panels.Item2);
        }

        private void Send()
        {
            string message = messageTextBox.Text;
            if (message != string.Empty)
            {
                history[selectedChat].Add(new DisplayedMessage("You", message, false));
                (Panel?, Panel) panels = CreatePanel("You", message, false, selectedChat);
                DisplayPanels(panels);
                if (selectedChat != 0)
                {
                    Task.Run(() => client.SendText(selectedChat, message));
                }
            }
            messageTextBox.Text = string.Empty;
            messageTextBox.Focus();
        }

        private void IncomingTextMessage(int chat, string sender, string text)
        {
            Invoke(() =>
            {
                history[chat].Add(new DisplayedMessage(sender, text, true));
                (Panel?, Panel) panels = CreatePanel(sender, text, true, chat);
                if (chat == selectedChat)
                {
                    DisplayPanels(panels);
                }
            });
        }

        private void IncomingImageMessage(int chat, string sender, Image image)
        {
            Invoke(() =>
            {
                history[chat].Add(new DisplayedMessage(sender, image, true));
                (Panel?, Panel) panels = CreatePanel(sender, image, true, chat);
                if (chat == selectedChat)
                {
                    DisplayPanels(panels);
                }
            });
        }

        private void IncomingFileMessage(int chat, string sender, string filename, string fileID)
        {
            Invoke(() =>
            {
                history[chat].Add(new DisplayedMessage(sender, filename, fileID, true));
                (Panel?, Panel) panels = CreatePanel(sender, filename, fileID, true, chat);
                if (chat == selectedChat)
                {
                    DisplayPanels(panels);
                }
            });
        }

        public static string FileSizeToString(long fileSize)
        {
            string[] sizeSuffixes = { "B", "KB", "MB", "GB", "TB" };
            const int baseSize = 1024;
            if (fileSize == 0)
            {
                return "0" + sizeSuffixes[0];
            }
            var bytes = Math.Abs(fileSize);
            var suffixIndex = (int)Math.Log(bytes, baseSize);
            var normalizedSize = Math.Round(bytes / Math.Pow(baseSize, suffixIndex), 1);
            return (Math.Sign(fileSize) * normalizedSize).ToString() + sizeSuffixes[suffixIndex];
        }


        private void MessageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Send();
            }
        }

        private void FileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] bytes = File.ReadAllBytes(openFileDialog.FileName);

                try
                {
                    Image image = Image.FromFile(openFileDialog.FileName);
                    if (selectedChat != 0)
                    {
                        Task.Run(() => client.SendImage(selectedChat, bytes));
                    }
                    history[selectedChat].Add(new DisplayedMessage("You", image, false));
                    (Panel?, Panel) panels = CreatePanel("You", image, false, selectedChat);
                    DisplayPanels(panels);
                }
                catch (OutOfMemoryException)
                {
                    var filename = $"{Path.GetFileName(openFileDialog.FileName)} ({FileSizeToString(bytes.Length)})";
                    string fileID = client.SendFile(selectedChat, bytes, filename);
                    (Panel?, Panel) panels = CreatePanel("You", filename, fileID, false, selectedChat);
                    DisplayPanels(panels);
                    history[selectedChat].Add(new DisplayedMessage("You", filename, fileID, false));
                }
                messageTextBox.Text = string.Empty;
                messageTextBox.Focus();
            }
        }

        private string? ShowSaveFileDialog(string filename)
        {
            int dotIndex = filename.LastIndexOf('.');
            int sizeIndex = filename.LastIndexOf(' ');
            string extension = dotIndex == -1 ? "" : filename.Substring(dotIndex, sizeIndex - dotIndex);
            bool dangerousExtension = extension == ".exe" || extension == ".bin" || extension == "msi"
                || extension == ".jar" || extension == ".cmd" || extension == ".bat";
            if (dangerousExtension && MessageBox.Show($"This file has the extension {extension}.\n" +
                    $"It may harm your computer.\nAre you sure you want to download it?",
                    "Potentially dangerous file", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return null;
            }
            saveFileDialog.FileName = filename;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveHistory();
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}