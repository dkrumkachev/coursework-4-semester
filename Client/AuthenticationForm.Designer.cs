namespace Client
{
    partial class AuthenticationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            confirmButton = new Button();
            usernameTextBox = new TextBox();
            headerLabel = new Label();
            usernameLabel = new Label();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            anotherOptionLabel = new Label();
            changeOptionButton = new Button();
            panel1 = new Panel();
            errorLabel = new Label();
            label1 = new Label();
            panel2 = new Panel();
            skipButton = new Button();
            label2 = new Label();
            signUpButton = new Button();
            nameTextBox = new TextBox();
            loadingLabel = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // confirmButton
            // 
            confirmButton.Anchor = AnchorStyles.Left;
            confirmButton.AutoSize = true;
            confirmButton.BackColor = Color.DodgerBlue;
            confirmButton.FlatAppearance.BorderColor = Color.DeepSkyBlue;
            confirmButton.FlatAppearance.BorderSize = 0;
            confirmButton.FlatStyle = FlatStyle.Flat;
            confirmButton.Font = new Font("Futura Bk BT", 18F, FontStyle.Regular, GraphicsUnit.Point);
            confirmButton.ForeColor = Color.Black;
            confirmButton.Location = new Point(0, 313);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new Size(344, 50);
            confirmButton.TabIndex = 3;
            confirmButton.Text = "Sign in";
            confirmButton.UseCompatibleTextRendering = true;
            confirmButton.UseVisualStyleBackColor = false;
            confirmButton.Click += ConfirmButton_Click;
            // 
            // usernameTextBox
            // 
            usernameTextBox.Anchor = AnchorStyles.Left;
            usernameTextBox.BackColor = SystemColors.GrayText;
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Cursor = Cursors.IBeam;
            usernameTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            usernameTextBox.ForeColor = SystemColors.ButtonHighlight;
            usernameTextBox.Location = new Point(0, 129);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(344, 40);
            usernameTextBox.TabIndex = 1;
            // 
            // headerLabel
            // 
            headerLabel.Dock = DockStyle.Top;
            headerLabel.Font = new Font("Futura Md BT", 22.2F, FontStyle.Regular, GraphicsUnit.Point);
            headerLabel.ForeColor = Color.White;
            headerLabel.Location = new Point(0, 0);
            headerLabel.Name = "headerLabel";
            headerLabel.Size = new Size(344, 46);
            headerLabel.TabIndex = 6;
            headerLabel.Text = "Sign in";
            headerLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // usernameLabel
            // 
            usernameLabel.Anchor = AnchorStyles.Left;
            usernameLabel.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            usernameLabel.ForeColor = Color.White;
            usernameLabel.Location = new Point(0, 92);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(344, 34);
            usernameLabel.TabIndex = 7;
            usernameLabel.Text = "Username:";
            usernameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // passwordLabel
            // 
            passwordLabel.Anchor = AnchorStyles.Left;
            passwordLabel.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            passwordLabel.ForeColor = Color.White;
            passwordLabel.Location = new Point(0, 192);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(344, 34);
            passwordLabel.TabIndex = 8;
            passwordLabel.Text = "Password:";
            passwordLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Anchor = AnchorStyles.Left;
            passwordTextBox.BackColor = SystemColors.GrayText;
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.Cursor = Cursors.IBeam;
            passwordTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            passwordTextBox.ForeColor = SystemColors.ButtonHighlight;
            passwordTextBox.Location = new Point(0, 229);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(344, 40);
            passwordTextBox.TabIndex = 2;
            // 
            // anotherOptionLabel
            // 
            anotherOptionLabel.Anchor = AnchorStyles.Left;
            anotherOptionLabel.Font = new Font("Futura Md BT", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            anotherOptionLabel.ForeColor = Color.White;
            anotherOptionLabel.Location = new Point(0, 374);
            anotherOptionLabel.Name = "anotherOptionLabel";
            anotherOptionLabel.Size = new Size(344, 32);
            anotherOptionLabel.TabIndex = 10;
            anotherOptionLabel.Text = "Don't have an account?";
            anotherOptionLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // changeOptionButton
            // 
            changeOptionButton.Anchor = AnchorStyles.Left;
            changeOptionButton.AutoSize = true;
            changeOptionButton.BackColor = Color.Cyan;
            changeOptionButton.FlatAppearance.BorderColor = Color.DeepSkyBlue;
            changeOptionButton.FlatAppearance.BorderSize = 0;
            changeOptionButton.FlatStyle = FlatStyle.Flat;
            changeOptionButton.Font = new Font("Futura Bk BT", 18F, FontStyle.Regular, GraphicsUnit.Point);
            changeOptionButton.ForeColor = Color.Black;
            changeOptionButton.Location = new Point(0, 409);
            changeOptionButton.Name = "changeOptionButton";
            changeOptionButton.Size = new Size(344, 50);
            changeOptionButton.TabIndex = 4;
            changeOptionButton.Text = "Create an account";
            changeOptionButton.UseCompatibleTextRendering = true;
            changeOptionButton.UseVisualStyleBackColor = false;
            changeOptionButton.Click += ChangeOptionButton_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.Controls.Add(errorLabel);
            panel1.Controls.Add(headerLabel);
            panel1.Controls.Add(changeOptionButton);
            panel1.Controls.Add(confirmButton);
            panel1.Controls.Add(anotherOptionLabel);
            panel1.Controls.Add(usernameTextBox);
            panel1.Controls.Add(passwordTextBox);
            panel1.Controls.Add(usernameLabel);
            panel1.Controls.Add(passwordLabel);
            panel1.Location = new Point(856, 125);
            panel1.Name = "panel1";
            panel1.Size = new Size(344, 462);
            panel1.TabIndex = 12;
            panel1.Visible = false;
            // 
            // errorLabel
            // 
            errorLabel.Anchor = AnchorStyles.Left;
            errorLabel.Font = new Font("Futura Md BT", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point);
            errorLabel.ForeColor = Color.Red;
            errorLabel.Location = new Point(0, 49);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(344, 36);
            errorLabel.TabIndex = 11;
            errorLabel.Text = "Error";
            errorLabel.TextAlign = ContentAlignment.TopCenter;
            errorLabel.Visible = false;
            // 
            // label1
            // 
            label1.Font = new Font("Futura Md BT", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(416, 52);
            label1.TabIndex = 12;
            label1.Text = "Please enter your name.";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(skipButton);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(signUpButton);
            panel2.Controls.Add(nameTextBox);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(83, 168);
            panel2.Name = "panel2";
            panel2.Size = new Size(416, 389);
            panel2.TabIndex = 12;
            panel2.Visible = false;
            // 
            // skipButton
            // 
            skipButton.Anchor = AnchorStyles.Left;
            skipButton.AutoSize = true;
            skipButton.BackColor = Color.Cyan;
            skipButton.FlatAppearance.BorderColor = Color.DeepSkyBlue;
            skipButton.FlatAppearance.BorderSize = 0;
            skipButton.FlatStyle = FlatStyle.Flat;
            skipButton.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            skipButton.ForeColor = Color.Black;
            skipButton.Location = new Point(42, 321);
            skipButton.Name = "skipButton";
            skipButton.Size = new Size(332, 46);
            skipButton.TabIndex = 14;
            skipButton.Text = "Skip";
            skipButton.UseCompatibleTextRendering = true;
            skipButton.UseVisualStyleBackColor = false;
            skipButton.Click += SkipButton_Click;
            // 
            // label2
            // 
            label2.Font = new Font("Futura Md BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(0, 44);
            label2.Name = "label2";
            label2.Size = new Size(416, 78);
            label2.TabIndex = 13;
            label2.Text = "It will be displayed to other users.";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // signUpButton
            // 
            signUpButton.Anchor = AnchorStyles.Left;
            signUpButton.AutoSize = true;
            signUpButton.BackColor = Color.DodgerBlue;
            signUpButton.FlatAppearance.BorderColor = Color.DeepSkyBlue;
            signUpButton.FlatAppearance.BorderSize = 0;
            signUpButton.FlatStyle = FlatStyle.Flat;
            signUpButton.Font = new Font("Futura Bk BT", 18F, FontStyle.Regular, GraphicsUnit.Point);
            signUpButton.ForeColor = Color.Black;
            signUpButton.Location = new Point(42, 235);
            signUpButton.Name = "signUpButton";
            signUpButton.Size = new Size(332, 56);
            signUpButton.TabIndex = 12;
            signUpButton.Text = "Sign up";
            signUpButton.UseCompatibleTextRendering = true;
            signUpButton.UseVisualStyleBackColor = false;
            signUpButton.Click += SignUpButton_Click;
            // 
            // nameTextBox
            // 
            nameTextBox.Anchor = AnchorStyles.Left;
            nameTextBox.BackColor = SystemColors.GrayText;
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.Cursor = Cursors.IBeam;
            nameTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            nameTextBox.ForeColor = SystemColors.ButtonHighlight;
            nameTextBox.Location = new Point(42, 150);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(332, 40);
            nameTextBox.TabIndex = 12;
            // 
            // loadingLabel
            // 
            loadingLabel.AutoSize = true;
            loadingLabel.Font = new Font("Futura Md BT", 22.2F, FontStyle.Regular, GraphicsUnit.Point);
            loadingLabel.ForeColor = Color.Gainsboro;
            loadingLabel.Location = new Point(605, 377);
            loadingLabel.Name = "loadingLabel";
            loadingLabel.Size = new Size(190, 45);
            loadingLabel.TabIndex = 12;
            loadingLabel.Text = "Loading...";
            loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AuthenticationForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateGray;
            ClientSize = new Size(1382, 753);
            Controls.Add(loadingLabel);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(5);
            Name = "AuthenticationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sign in";
            Shown += AuthenticationForm_Shown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button confirmButton;
        private TextBox usernameTextBox;
        private Label headerLabel;
        private Label usernameLabel;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Label anotherOptionLabel;
        private Button changeOptionButton;
        private Panel panel1;
        private Label errorLabel;
        private Label label1;
        private Panel panel2;
        private Button signUpButton;
        private TextBox nameTextBox;
        private Button skipButton;
        private Label label2;
        private Label loadingLabel;
    }
}