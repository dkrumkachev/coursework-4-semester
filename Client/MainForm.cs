using Common.Encryption;
using Common.Messages;
using Org.BouncyCastle.Asn1.Cms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Image = System.Drawing.Image;

namespace Client
{
    public partial class MainForm : Form
    {
        private const string SelfChatName = Client.SelfChatName;
        private readonly string signInFilePath = "save.txt";
        private string historyPath = string.Empty;
        private readonly Client client;
        private string selectedChat = string.Empty;
        private const int space = 5;
        private Color ownMessageColor = Color.DodgerBlue;
        private Color othersMessageColor = Color.FromArgb(16, 53, 53);
        private Size maxImageSize = new(600, 600);
        private byte[] messageHistoryKey = Array.Empty<byte>();
        private readonly Dictionary<string, List<Panel>> chatMessages = new() { { SelfChatName, new List<Panel>() } };
        private Dictionary<string, (int id, List<object> Messages)> history = new() {
            { SelfChatName, (0, new List<object>()) }
        };
        private readonly Dictionary<string, int> nextMessageY = new() { { SelfChatName, space * 2 } };
        private readonly Dictionary<string, (int Day, int Month)> lastMessageDate = new() { { SelfChatName, (0, 0) } };

        public MainForm()
        {
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
                if (!Authenticate())
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
            chatPanel.AutoScroll = false;
            chatPanel.HorizontalScroll.Enabled = false;
            chatPanel.HorizontalScroll.Visible = false;
            chatPanel.HorizontalScroll.Maximum = 0;
            chatPanel.AutoScroll = true;
        }
#pragma warning disable SYSLIB0011
        private void ReadHistoryFromFile()
        {
            if (File.Exists(historyPath))
            {
                chatMessages.Clear();
                chatIDs.Clear();
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
                history = (Dictionary<string, (int, List<object>)>)serializer.Deserialize(stream);
                RestoreHistory();
                client.Restore(historyPath + ".chats");
            }
            else
            {
                AddButtonForChat(SelfChatName);
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
                client.Save(historyPath + ".chats");
            }
        }
#pragma warning restore SYSLIB0011

        private void RestoreHistory()
        {
            foreach (var keyValue in history)
            {
                string chatName = keyValue.Key;
                chatMessages.Add(chatName, new List<Panel>());
                chatIDs.Add(chatName, keyValue.Value.id);
                nextMessageY.Add(chatName, space * 2);
                lastMessageDate.Add(chatName, (0, 0));
                foreach (object msg in keyValue.Value.Messages)
                {
                    try
                    {
                        var message = (DisplayedMessage)msg;
                        if (message.MessageType == DisplayedMessage.Type.Text && message.Text != null)
                        {
                            CreatePanel(message.Sender, message.Text, message.IsIncoming, chatName);
                        }
                        else if (message.MessageType == DisplayedMessage.Type.Image && message.Image != null)
                        {
                            CreatePanel(message.Sender, message.Image, message.IsIncoming, chatName);
                        }
                        else if (message.MessageType == DisplayedMessage.Type.File &&
                            message.Filename != null && message.FileID != null)
                        {
                            //CreatePanel(message.Sender, message.Filename, message.FileID, message.IsIncoming, chatName);
                        }
                    }
                    catch { }
                }
                AddButtonForChat(chatName);
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
            control.Text = text;
            control.Location = new Point(x, y);
        }

        private PictureBox CreatePictureBox(Image image)
        {
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

        private Panel CreateDatePanel(string date, string chatName)
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
            panel.Location = new Point(0, nextMessageY[chatName]);
            nextMessageY[chatName] += panel.Height + space;
            return panel;
        }

        private (Panel?, Panel) CreatePanel(Control control, string sender, string text, bool isIncoming, string chatName)
        {
            Panel? datePanel = null;
            if (lastMessageDate[chatName].Day < DateTime.Today.Day ||
                lastMessageDate[chatName].Month < DateTime.Today.Month)
            {
                datePanel = CreateDatePanel(DateTime.Now.ToShortDateString(), chatName);
                lastMessageDate[chatName] = (DateTime.Today.Day, DateTime.Today.Month);
                chatMessages[chatName].Add(datePanel);
            }
            Color backColor = isIncoming ? othersMessageColor : ownMessageColor;
            var panel = new Panel()
            {
                BackColor = backColor,
                BorderStyle = BorderStyle.None,
            };
            Label nameLabel = CreateLabel(backColor, 12);
            Label timeLabel = CreateLabel(backColor, 9);
            AddControl(panel, nameLabel, sender, space, space);
            AddControl(panel, control, text, space, space + nameLabel.Height);
            AddControl(panel, timeLabel, DateTime.Now.ToShortTimeString(),
                space, space + nameLabel.Height + control.Height);
            int x = Math.Max(control.Width, nameLabel.Width) + space * 2;
            int y = nameLabel.Height + control.Height + timeLabel.Height + space;
            if (control is not PictureBox)
            {
                y += space;
            }
            panel.Size = new Size(x, y);
            x = isIncoming ? 30 : chatPanel.Width - panel.Width - 30;
            panel.Location = new Point(x, nextMessageY[chatName]);
            nextMessageY[chatName] += panel.Height + space;
            chatMessages[chatName].Add(panel);
            return (datePanel, panel);
        }

        private (Panel?, Panel) CreatePanel(string sender, string message, bool isIncoming, string chatName)
        {
            Color backColor = isIncoming ? othersMessageColor : ownMessageColor;
            Label msgLabel = CreateLabel(backColor);
            return CreatePanel(msgLabel, sender, message, isIncoming, chatName);
        }

        private (Panel?, Panel) CreatePanel(string sender, Image image, bool isIncoming, string chatName)
        {
            PictureBox pictureBox = CreatePictureBox(image);
            return CreatePanel(pictureBox, sender, "", isIncoming, chatName);
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

        private void DisplayChat(string chatName)
        {
            int membersCount = client.GetChatMembersCount(chatIDs[chatName]);
            if (membersCount > 2)
            {
                chatNameLabel.Text = "    " + chatName + $" ({membersCount} members)";
            }
            else
            {
                chatNameLabel.Text = "    " + chatName;
            }
            waitingLabel.Visible = false;
            messageTextBox.Enabled = true;
            foreach (Panel panel in chatMessages[chatName])
            {
                DisplayMessage(panel);
            }
            ScrollChatDown();
        }

        private void DisplayChatWaitingForUsers(string chatName)
        {
            waitingLabel.Visible = true;
            waitingLabel.Text = "Waiting for users to connect...";
            int membersCount = 0;
            if (chatIDs.ContainsKey(chatName))
            {
                client.GetChatMembersCount(chatIDs[chatName]);
            }
            if (membersCount > 2)
            {
                chatNameLabel.Text = "    " + chatName + $" ({membersCount} members)";
            }
            else if (membersCount == 2)
            {
                waitingLabel.Text = "Waiting for the user to connect...";
            }
            else
            {
                chatNameLabel.Text = "    " + chatName;
            }
            messageTextBox.Enabled = false;
        }

        private void Send()
        {
            string message = messageTextBox.Text;
            if (message != string.Empty)
            {
                history[selectedChat].Messages.Add(new DisplayedMessage("You", message, false));
                (Panel?, Panel) panels = CreatePanel("You", message, false, selectedChat);
                if (panels.Item1 != null)
                {
                    DisplayMessage(panels.Item1);
                }
                DisplayMessageAndScroll(panels.Item2);
                if (selectedChat != SelfChatName)
                {
                    client.SendText(chatIDs[selectedChat], message);
                }
            }
            messageTextBox.Text = string.Empty;
            messageTextBox.Focus();
        }

        private void IncomingTextMessage(string chatName, string sender, string text)
        {
            Invoke(() =>
            {
                history[chatName].Messages.Add(new DisplayedMessage(sender, text, true));
                (Panel?, Panel) panels = CreatePanel(sender, text, true, chatName);
                if (chatName == selectedChat)
                {
                    if (panels.Item1 != null)
                    {
                        DisplayMessage(panels.Item1);
                    }
                    DisplayMessageAndScroll(panels.Item2);
                }
            });
        }

        private void IncomingImageMessage(string chatName, string sender, Image image)
        {
            Invoke(() =>
            {
                history[chatName].Messages.Add(new DisplayedMessage(sender, image, true));
                (Panel?, Panel) panels = CreatePanel(sender, image, true, chatName);
                if (chatName == selectedChat || selectedChat == sender)
                {
                    if (panels.Item1 != null)
                    {
                        DisplayMessage(panels.Item1);
                    }
                    DisplayMessageAndScroll(panels.Item2);
                }
            });
        }

        private void IncomingFileMessage(string chatName, string sender, string filename, string fileID)
        {
            Invoke(() =>
            {
                history[chatName].Messages.Add(new DisplayedMessage(sender, filename, fileID, true));
            });

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
                    history[selectedChat].Messages.Add(new DisplayedMessage("You", image, false));
                    (Panel?, Panel) panels = CreatePanel("You", image, false, selectedChat);
                    if (panels.Item1 != null)
                    {
                        DisplayMessage(panels.Item1);
                    }
                    DisplayMessageAndScroll(panels.Item2);
                    if (selectedChat != SelfChatName)
                    {
                        Task.Run(() => client.SendImage(chatIDs[selectedChat], bytes));
                    }
                }
                catch (OutOfMemoryException)
                {
                    //client.SendFile(chatIDs[selectedChat], bytes, Path.GetFileName(openFileDialog.FileName));
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveHistory();
        }

    }
}