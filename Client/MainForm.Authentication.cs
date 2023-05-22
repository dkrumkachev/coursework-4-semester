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

        private bool Authenticate()
        {
            if (File.Exists(signInFilePath))
            {
                string[] lines = File.ReadAllLines(signInFilePath);
                if (lines.Length == 2 && client.Authentication(lines[0], lines[1], out _, out messageHistoryKey))
                {
                    historyPath = Encryption.SHA256Hash(Encoding.Default.GetBytes(lines[0]));
                    AuthenticationComplete();
                    return true;
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
            if (!client.Authentication(username, ref password, out string error, out messageHistoryKey, signingUp))
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
                    AuthenticationComplete();
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

        private void AuthenticationComplete()
        {
            if (signingUp)
            {
                signingUp = false;
                ChangeContolsText();
            }
            if (historyPath == string.Empty)
            {
                historyPath = Encryption.SHA256Hash(Encoding.Default.GetBytes(usernameTextBox.Text));
            }
            ReadHistoryFromFile();
            usernameTextBox.Text = string.Empty;
            passwordTextBox.Text = string.Empty;
            nameTextBox.Text = string.Empty;
            errorLabel.Visible = false;
            authenticationPanel.Visible = false;
            nameInputPanel.Visible = false;
            mainPanel.Visible = true;
            chatNameLabel.Text = "    " + client.SelfName;
            Text = client.SelfName;
            client.ReceiveMessages();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            if (!client.SetName(nameTextBox.Text))
            {
                MessageBox.Show("Something went wrong. Try again.");
            }
            else
            {
                AuthenticationComplete();
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            AuthenticationComplete();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            client.Logout();
            SaveHistory();
            historyPath = string.Empty;
            chatPanel.Controls.Clear();
            chatMessages.Clear();
            chatMessages.Add(SelfChatName, new List<Panel>());
            history.Clear();
            history.Add(SelfChatName, (0, new List<object>()));
            nextMessageY.Clear();
            nextMessageY.Add(SelfChatName, space * 2);
            lastMessageDate.Clear();
            lastMessageDate.Add(SelfChatName, (0, 0));
            File.Delete(signInFilePath);
            mainPanel.Visible = false;
            loadingLabel.Visible = true;
            initialChatPanelLabel.Text = "Your messages will be displayed here";
            initialChatPanelLabel.Visible = true;
            selectedChat = string.Empty;
            ConnectToServer();
        }
    }
}
