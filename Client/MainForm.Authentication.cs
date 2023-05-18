using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class MainForm : Form
    {
        private bool signingUp = false;
        private readonly Client client = new(); 

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            if (!client.Authentication(username, password, out string error, signingUp))
            {
                errorLabel.Text = error;
                errorLabel.Visible = true;
            }
            else if (signingUp)
            {
                InputName();
            }
            else
            {
                ShowMainPanel();
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

        private void ShowMainPanel()
        {
            
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            if (!client.SetName(nameTextBox.Text))
            {
                MessageBox.Show("Something went wrong. Try again.");
            }
            else
            {
                ShowMainPanel();
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            ShowMainPanel();
        }

        private void AuthenticationForm_Shown(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                client.WaitForConnectionToServer();
                Invoke(() =>
                {
                    loadingLabel.Visible = false;
                    authenticationPanel.Visible = true;
                });
            });
        }
    }
}
