using Common.Encryption;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Client
{
    public partial class MainForm : Form
    {
        private bool signingUp = false;

        private bool Authorization()
        {
            if (File.Exists(signInFilePath))
            {
                string[] lines = File.ReadAllLines(signInFilePath);
                if (lines.Length != 2)
                {
                    File.Delete(signInFilePath);
                    return false;
                }
                string username = lines[0];
                string passwordHash = lines[1];
                if (client.Authorization(username, passwordHash, out string error, out messageHistoryKey))
                {
                    historyPath = $"history\\{Encryption.SHA256Hash(Encoding.Default.GetBytes(username))}";
                    AuthorizationComplete();
                    return true;
                }
                else if (error == "You have already signed in")
                {
                    return false;
                }
                else
                {
                    File.Delete(signInFilePath);
                }
            }
            return false;
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            if (!client.Authorization(username, ref password, out string error, out messageHistoryKey, signingUp))
            {
                errorLabel.Text = error;
                errorLabel.Visible = true;
            }
            else
            {
                SaveAuthenticationData(username, password);
                if (signingUp)
                {
                    InputName();
                }
                else
                {
                    AuthorizationComplete();
                }
            }
        }

        private void SaveAuthenticationData(string username, string password)
        {
            if (rememberMeCheckBox.Checked)
            {
                File.WriteAllLines(signInFilePath, new string[] { username, password });
            }
        }

        private void ChangeOptionButton_Click(object sender, EventArgs e)
        {
            signingUp = !signingUp;
            ChangeContolsText();
        }

        private void ChangeContolsText()
        {
            if (signingUp)
            {
                headerLabel.Text = "Sign up";
                usernameLabel.Text = "Create a username:";
                passwordLabel.Text = "Create a password:";
                confirmButton.Text = "Continue";
                anotherOptionLabel.Text = "Already have an account?";
                changeOptionButton.Text = "Sign in";
            }
            else
            {
                headerLabel.Text = "Sign in";
                usernameLabel.Text = "Username:";
                passwordLabel.Text = "Password:";
                confirmButton.Text = "Sign in";
                anotherOptionLabel.Text = "Don't have an account?";
                changeOptionButton.Text = "Create an account";
            }
            usernameTextBox.Text = string.Empty;
            passwordTextBox.Text = string.Empty;
            errorLabel.Visible = false;
        }

        private void InputName()
        {
            authenticationPanel.Visible = false;
            nameInputPanel.Visible = true;
        }

        private void AuthorizationComplete()
        {
            if (historyPath == string.Empty)
            {
                byte[] username = Encoding.Default.GetBytes(usernameTextBox.Text);
                historyPath = $"history\\{Encryption.SHA256Hash(username)}";
            }
            ReadHistoryFromFile();
            if (signingUp)
            {
                signingUp = false;
                ChangeContolsText();
            }
            usernameTextBox.Text = string.Empty;
            passwordTextBox.Text = string.Empty;
            nameTextBox.Text = string.Empty;
            errorLabel.Visible = false;
            authenticationPanel.Visible = false;
            nameInputPanel.Visible = false;
            mainPanel.Visible = true;
            chatNameLabel.Text = "    " + client.SelfName;
            Text = client.SelfName;
            messageTextBox.Visible = false;
            fileButton.Visible = false;
            client.ReceiveMessages();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == string.Empty || client.SetName(nameTextBox.Text))
            {
                AuthorizationComplete();
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            AuthorizationComplete();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            client.Logout();
            SaveHistory();
            historyPath = string.Empty;
            leftPanel.Controls.Clear();
            leftPanel.Controls.Add(newChatButton);
            chatPanel.Controls.Clear();
            chatMessages.Clear();
            chatMessages.Add(0, new List<Panel>());
            history.Clear();
            history.Add(0, new List<DisplayedMessage>());
            nextMessageY.Clear();
            nextMessageY.Add(0, space * 2);
            lastMessageDate.Clear();
            lastMessageDate.Add(0, (0, 0));
            chatButtonLocation = chatPanel.Location;
            File.Delete(signInFilePath);
            mainPanel.Visible = false;
            loadingLabel.Visible = true;
            waitingLabel.Visible = false;
            initialChatPanelLabel.Text = "Your messages will be displayed here";
            initialChatPanelLabel.Visible = true;
            selectedChat = -1;
            ConnectToServer();
        }
    }
}
