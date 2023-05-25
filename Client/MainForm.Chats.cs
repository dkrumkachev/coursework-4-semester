using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Client
{
    public partial class MainForm : Form
    {
        private Point chatButtonLocation = new();
        private readonly int buttonsGap = 6;
        private Size buttonSize = new(250, 50);
        private readonly Font font = new("Segoe UI", (float)13.8);
        private readonly Color buttonColor = Color.DarkCyan;

        private readonly Dictionary<Button, int> chats = new();
        private readonly List<string> newChatUsers = new();

        private void NewChatButton_Click(object sender, EventArgs e)
        {
            chooseChatTypePanel.Visible = true;
            leftPanel.Visible = false;
        }

        private void PrivateChatButton_Click(object sender, EventArgs e)
        {
            leftPanel.Visible = false;
            chooseChatTypePanel.Visible = false;
            createSingleChatPanel.Visible = true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            findUserErrorLabel1.Visible = false;
            findUserErrorLabel2.Visible = false;
            createSingleChatPanel.Visible = false;
            chooseChatTypePanel.Visible = false;
            inputChatNamePanel.Visible = false;
            addMembersPanel.Visible = false;
            leftPanel.Visible = true;
            userInfoTextBox.Text = string.Empty;
            chatNameTextBox.Text = string.Empty;
            memberNameTextBox.Text = string.Empty;
            chatNameErrorLabel.Visible = false;
            addedMembersLabel.Text = "Added members:";
            newChatUsers.Clear();
        }

        private void UserInfoTextBox_TextChanged(object sender, EventArgs e)
        {
            findUserErrorLabel1.Visible = false;
            findUserErrorLabel2.Visible = false;
        }

        private void CreateSingleChatButton_Click(object sender, EventArgs e)
        {

            string username = userInfoTextBox.Text;
            string? displayName = client.FindUser(username);
            if (username == client.SelfUsername)
            {
                findUserErrorLabel1.Text = "you cannot create a chat with yourself";
                findUserErrorLabel1.Visible = true;
            }
            else if (displayName == null) 
            {
                findUserErrorLabel1.Text = "user not found";
                findUserErrorLabel1.Visible = true;
            }
            else
            {
                createSingleChatPanel.Visible = false;
                leftPanel.Visible = true;
                userInfoTextBox.Text = string.Empty;
                client.CreateSingleChat(username);
                AddButtonForChat(displayName);
            }
        }

        private Button AddButtonForChat(string name)
        {
            var button = new Button
            {
                Text = name,
                Font = font,
                ForeColor = Color.White,
                BackColor = buttonColor,
                Location = chatButtonLocation,
                Size = buttonSize,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += SelectChatButton_Click;
            Invoke(() =>
            {
                leftPanel.Controls.Add(button);
                chatButtonLocation.Y += buttonSize.Height + buttonsGap;
            });
            return button;
        }


        private void SelectChatButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (chats.TryGetValue(button, out int id) && id != selectedChat)
                {
                    chatNameLabel.Text = "    " + button.Text;
                    initialChatPanelLabel.Visible = false;
                    chatPanel.AutoScrollPosition = new Point(0, 0);
                    chatPanel.Controls.Clear();
                    messageTextBox.Visible = true;
                    fileButton.Visible = true;
                    messageTextBox.Focus();
                    selectedChat = id;
                    DisplayChat(id);
                }
                else
                {
                    chatNameLabel.Text = "    " + button.Text;
                    initialChatPanelLabel.Visible = false;
                    chatPanel.AutoScrollPosition = new Point(0, 0);
                    chatPanel.Controls.Clear();
                    messageTextBox.Visible = true;
                    fileButton.Visible = true;
                    messageTextBox.Focus();
                    DisplayChatWaitingForUsers();
                }
            }
        }

        public void ChatCreated(string name, int id)
        {
            Button? button = null;
            foreach (Control control in leftPanel.Controls)
            {
                if (control.Text == name)
                {
                    button = (Button)control;
                    break;
                }
            }
            if (button == null)
            {
                button = AddButtonForChat(name);
            }
            chats.Add(button, id);
            chatMessages.Add(id, new List<Panel>());
            history.Add(id, new List<DisplayedMessage>());
            nextMessageY.Add(id, space * 2);
            lastMessageDate.Add(id, (0, 0));
        }

        private void GroupChatButton_Click(object sender, EventArgs e)
        {
            chooseChatTypePanel.Visible = false;
            inputChatNamePanel.Visible = true;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            if (chatNameTextBox.Text == string.Empty)
            {
                chatNameErrorLabel.Visible = true;
            }
            else
            {
                inputChatNamePanel.Visible = false;
                addMembersPanel.Visible = true;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string username = memberNameTextBox.Text;
            string? displayName = client.FindUser(username);
            if (displayName == null)
            {
                findUserErrorLabel2.Visible = true;
            }
            else if (displayName != client.SelfName)
            {
                newChatUsers.Add(username);
                memberNameTextBox.Text = string.Empty;
                addedMembersLabel.Visible = true;
                addedMembersLabel.Text += $"\n{username}"; 
            }
        }

        private void CreateGroupChatButton_Click(object sender, EventArgs e)
        {
            if (newChatUsers.Count != 0)
            {
                client.CreateGroupChat(chatNameTextBox.Text, newChatUsers);
                AddButtonForChat(chatNameTextBox.Text);
            }
            newChatUsers.Clear();
            chatNameTextBox.Text = string.Empty;
            findUserErrorLabel2.Visible = false;
            addMembersPanel.Visible = false;
            chatNameErrorLabel.Visible = false;
            addedMembersLabel.Text = "Added members:";
            leftPanel.Visible = true;
        }

        private void ChatNameTextBox_TextChanged(object sender, EventArgs e)
        {
            chatNameErrorLabel.Visible = false;
        }


    }
}
