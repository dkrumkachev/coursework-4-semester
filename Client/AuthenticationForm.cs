using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class AuthenticationForm : Form
    {
        private bool signingUp = false;
        private readonly Client client = new();

        public AuthenticationForm()
        {
            InitializeComponent();
        }

        private void CenterControl(Control control)
        {
            control.Location = new Point((ClientSize.Width - control.Width) / 2,
                (ClientSize.Height - control.Height) / 2);
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
                confirmButton.Text = "Sign in";
                usernameLabel.Text = "Username:";
                passwordLabel.Text = "Password:";
                anotherOptionLabel.Text = "Don't have an account?";
                changeOptionButton.Text = "Create an account";
            }
            usernameTextBox.Text = string.Empty;
            passwordTextBox.Text = string.Empty;
            errorLabel.Visible = false;
        }

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
                CreateUsername();
            }
            else
            {
                OpenMainWindow();
            }
        }

        private void CreateUsername()
        {
            panel1.Visible = false;
            panel2.Visible = true;
            CenterControl(panel2);
        }

        private void OpenMainWindow()
        {
            Close();
            var mainForm = new MainForm();
            mainForm.Show();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            if (!client.SetName(nameTextBox.Text))
            {
                MessageBox.Show("Something went wrong. Try again.");
            }
            else
            {
                OpenMainWindow();
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            OpenMainWindow();
        }

        private void AuthenticationForm_Shown(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (client.ConnectToServer())
                {

                    Invoke(() =>
                    {
                        CenterControl(panel1);
                        loadingLabel.Visible = false;
                        panel1.Visible = true;
                    });
                }
                else
                {
                    Invoke(() =>
                    {
                        loadingLabel.Text = "An error occurred while trying to connect to the server.\n" +
                        "Please restart the application.";
                        CenterControl(loadingLabel);
                    });
                }
            });
        }

    }
}
