using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly Dictionary<string, int> chatIDs = new() { { SelfChatName, 0 } };
        private readonly List<string> newChatUsers = new List<string>();

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
            addedMembersLabel.Visible = false;
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
            if (displayName == null) 
            {
                findUserErrorLabel1.Visible = true;
            }
            else if (displayName == client.SelfName || chatIDs.ContainsKey(displayName))
            {
                createSingleChatPanel.Visible = false;
                leftPanel.Visible = true;
                userInfoTextBox.Text = string.Empty;
            }
            else
            {
                client.CreateSingleChat(username);
                AddButtonForChat(displayName);
                createSingleChatPanel.Visible = false;
                leftPanel.Visible = true;
                userInfoTextBox.Text = string.Empty;
            }
            
        }

        private void AddButtonForChat(string name)
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
            Invoke(() => leftPanel.Controls.Add(button) );
            chatButtonLocation.Y += buttonSize.Height + buttonsGap;
        }


        private void SelectChatButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button button && button.Text != selectedChat)
            {
                selectedChat = button.Text;
                initialChatPanelLabel.Visible = false;
                chatPanel.AutoScrollPosition = new Point(0, 0);
                chatPanel.Controls.Clear();
                messageTextBox.Visible = true;
                fileButton.Visible = true;
                messageTextBox.Focus();
                if (chatIDs.ContainsKey(button.Text))
                {
                    DisplayChat(button.Text);
                }
                else
                {
                    DisplayChatWaitingForUsers(button.Text);
                }
            }
        }

        public void ChatCreated(string name, int id)
        {
            bool isButtonFound = false;
            foreach (Control control in leftPanel.Controls)
            {
                if (control.Text == name)
                {
                    isButtonFound = true;
                    break;
                }
            }
            if (!isButtonFound)
            {
                AddButtonForChat(name);
            }
            chatIDs.Add(name, id);
            chatMessages.Add(name, new List<Panel>());
            history.Add(name, (id, new List<object>()));
            nextMessageY.Add(name, space * 2);
            lastMessageDate.Add(name, (0, 0));
        }

        private void GroupChatButton_Click(object sender, EventArgs e)
        {
            chooseChatTypePanel.Visible = false;
            inputChatNamePanel.Visible = true;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            if (chatIDs.ContainsKey(chatNameLabel.Text))
            {
                chatNameErrorLabel.Visible = true;
                chatNameTextBox.Text = string.Empty;
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
            if (newChatUsers.Count == 1)
            {
                string username = newChatUsers[0];
                string? displayName = client.FindUser(username);
                if (displayName != null && displayName != client.SelfName) 
                {
                    client.CreateSingleChat(username);
                    AddButtonForChat(displayName);
                }
            }
            else if (newChatUsers.Count > 1)
            {
                client.CreateGroupChat(chatNameTextBox.Text, newChatUsers);
                AddButtonForChat(chatNameTextBox.Text);
            }
            newChatUsers.Clear();
            chatNameTextBox.Text = string.Empty;
            findUserErrorLabel2.Visible = false;
            addMembersPanel.Visible = false;
            addedMembersLabel.Visible = false;
            addedMembersLabel.Text = "Added members:";
            leftPanel.Visible = true;
        }

        private void chatNameTextBox_TextChanged(object sender, EventArgs e)
        {
            chatNameErrorLabel.Visible = false;
        }


    }
}
