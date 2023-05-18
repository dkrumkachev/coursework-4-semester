namespace Client
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainPanel = new Panel();
            rightPanel = new Panel();
            leftPanel = new Panel();
            centerPanel = new Panel();
            richTextBox1 = new RichTextBox();
            button4 = new Button();
            textBox1 = new TextBox();
            button3 = new Button();
            button1 = new Button();
            button2 = new Button();
            nameInputPanel = new Panel();
            skipButton = new Button();
            label2 = new Label();
            signUpButton = new Button();
            nameTextBox = new TextBox();
            label1 = new Label();
            loadingLabel = new Label();
            authenticationPanel = new Panel();
            changeOptionButton = new Button();
            confirmButton = new Button();
            anotherOptionLabel = new Label();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            usernameLabel = new Label();
            passwordLabel = new Label();
            errorLabel = new Label();
            headerLabel = new Label();
            mainPanel.SuspendLayout();
            centerPanel.SuspendLayout();
            nameInputPanel.SuspendLayout();
            authenticationPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(rightPanel);
            mainPanel.Controls.Add(leftPanel);
            mainPanel.Controls.Add(centerPanel);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1582, 854);
            mainPanel.TabIndex = 0;
            mainPanel.Visible = false;
            // 
            // rightPanel
            // 
            rightPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rightPanel.Location = new Point(1327, 7);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(250, 841);
            rightPanel.TabIndex = 11;
            // 
            // leftPanel
            // 
            leftPanel.Location = new Point(5, 7);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(250, 841);
            leftPanel.TabIndex = 10;
            // 
            // centerPanel
            // 
            centerPanel.Anchor = AnchorStyles.Top;
            centerPanel.Controls.Add(richTextBox1);
            centerPanel.Controls.Add(button4);
            centerPanel.Controls.Add(textBox1);
            centerPanel.Controls.Add(button3);
            centerPanel.Controls.Add(button1);
            centerPanel.Controls.Add(button2);
            centerPanel.Location = new Point(261, 7);
            centerPanel.Name = "centerPanel";
            centerPanel.Size = new Size(1060, 841);
            centerPanel.TabIndex = 9;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.WindowFrame;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.BulletIndent = 300;
            richTextBox1.EnableAutoDragDrop = true;
            richTextBox1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox1.ForeColor = SystemColors.ButtonHighlight;
            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.Size = new Size(1060, 728);
            richTextBox1.TabIndex = 0;
            richTextBox1.TabStop = false;
            richTextBox1.Text = "";
            // 
            // button4
            // 
            button4.BackColor = Color.DeepSkyBlue;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            button4.ForeColor = Color.Black;
            button4.Location = new Point(932, 803);
            button4.Name = "button4";
            button4.Size = new Size(128, 38);
            button4.TabIndex = 5;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.WindowFrame;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.ForeColor = SystemColors.ButtonHighlight;
            textBox1.Location = new Point(41, 734);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(912, 34);
            textBox1.TabIndex = 1;
            // 
            // button3
            // 
            button3.BackColor = Color.DeepSkyBlue;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            button3.ForeColor = Color.Black;
            button3.Location = new Point(0, 734);
            button3.Name = "button3";
            button3.Size = new Size(35, 34);
            button3.TabIndex = 4;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.DeepSkyBlue;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(0, 803);
            button1.Name = "button1";
            button1.Size = new Size(145, 38);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.DeepSkyBlue;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            button2.ForeColor = Color.Black;
            button2.Location = new Point(959, 734);
            button2.Name = "button2";
            button2.Size = new Size(101, 34);
            button2.TabIndex = 3;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = false;
            // 
            // nameInputPanel
            // 
            nameInputPanel.Controls.Add(skipButton);
            nameInputPanel.Controls.Add(label2);
            nameInputPanel.Controls.Add(signUpButton);
            nameInputPanel.Controls.Add(nameTextBox);
            nameInputPanel.Controls.Add(label1);
            nameInputPanel.Location = new Point(1588, 12);
            nameInputPanel.Name = "nameInputPanel";
            nameInputPanel.Size = new Size(416, 389);
            nameInputPanel.TabIndex = 13;
            nameInputPanel.Visible = false;
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
            skipButton.TabIndex = 19;
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
            label2.TabIndex = 18;
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
            signUpButton.TabIndex = 15;
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
            nameTextBox.TabIndex = 16;
            // 
            // label1
            // 
            label1.Font = new Font("Futura Md BT", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(416, 52);
            label1.TabIndex = 17;
            label1.Text = "Please enter your name.";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // loadingLabel
            // 
            loadingLabel.AutoSize = true;
            loadingLabel.Font = new Font("Futura Md BT", 22.2F, FontStyle.Regular, GraphicsUnit.Point);
            loadingLabel.ForeColor = Color.Gainsboro;
            loadingLabel.Location = new Point(1588, 413);
            loadingLabel.Name = "loadingLabel";
            loadingLabel.Size = new Size(190, 45);
            loadingLabel.TabIndex = 13;
            loadingLabel.Text = "Loading...";
            loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // authenticationPanel
            // 
            authenticationPanel.Anchor = AnchorStyles.None;
            authenticationPanel.Controls.Add(changeOptionButton);
            authenticationPanel.Controls.Add(confirmButton);
            authenticationPanel.Controls.Add(anotherOptionLabel);
            authenticationPanel.Controls.Add(usernameTextBox);
            authenticationPanel.Controls.Add(passwordTextBox);
            authenticationPanel.Controls.Add(usernameLabel);
            authenticationPanel.Controls.Add(passwordLabel);
            authenticationPanel.Controls.Add(errorLabel);
            authenticationPanel.Controls.Add(headerLabel);
            authenticationPanel.Location = new Point(1588, 483);
            authenticationPanel.Name = "authenticationPanel";
            authenticationPanel.Size = new Size(344, 462);
            authenticationPanel.TabIndex = 14;
            authenticationPanel.Visible = false;
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
            changeOptionButton.TabIndex = 19;
            changeOptionButton.Text = "Create an account";
            changeOptionButton.UseCompatibleTextRendering = true;
            changeOptionButton.UseVisualStyleBackColor = false;
            changeOptionButton.Click += ChangeOptionButton_Click;
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
            confirmButton.TabIndex = 18;
            confirmButton.Text = "Sign in";
            confirmButton.UseCompatibleTextRendering = true;
            confirmButton.UseVisualStyleBackColor = false;
            confirmButton.Click += ConfirmButton_Click;
            // 
            // anotherOptionLabel
            // 
            anotherOptionLabel.Anchor = AnchorStyles.Left;
            anotherOptionLabel.Font = new Font("Futura Md BT", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            anotherOptionLabel.ForeColor = Color.White;
            anotherOptionLabel.Location = new Point(0, 374);
            anotherOptionLabel.Name = "anotherOptionLabel";
            anotherOptionLabel.Size = new Size(344, 32);
            anotherOptionLabel.TabIndex = 20;
            anotherOptionLabel.Text = "Don't have an account?";
            anotherOptionLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // usernameTextBox
            // 
            usernameTextBox.Anchor = AnchorStyles.Left;
            usernameTextBox.BackColor = SystemColors.GrayText;
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Cursor = Cursors.IBeam;
            usernameTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            usernameTextBox.ForeColor = SystemColors.ButtonHighlight;
            usernameTextBox.Location = new Point(0, 122);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(344, 40);
            usernameTextBox.TabIndex = 14;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Anchor = AnchorStyles.Left;
            passwordTextBox.BackColor = SystemColors.GrayText;
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.Cursor = Cursors.IBeam;
            passwordTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            passwordTextBox.ForeColor = SystemColors.ButtonHighlight;
            passwordTextBox.Location = new Point(0, 222);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(344, 40);
            passwordTextBox.TabIndex = 15;
            // 
            // usernameLabel
            // 
            usernameLabel.Anchor = AnchorStyles.Left;
            usernameLabel.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            usernameLabel.ForeColor = Color.White;
            usernameLabel.Location = new Point(0, 85);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(344, 34);
            usernameLabel.TabIndex = 16;
            usernameLabel.Text = "Username:";
            usernameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // passwordLabel
            // 
            passwordLabel.Anchor = AnchorStyles.Left;
            passwordLabel.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            passwordLabel.ForeColor = Color.White;
            passwordLabel.Location = new Point(0, 185);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(344, 34);
            passwordLabel.TabIndex = 17;
            passwordLabel.Text = "Password:";
            passwordLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // errorLabel
            // 
            errorLabel.Anchor = AnchorStyles.Left;
            errorLabel.Font = new Font("Futura Md BT", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point);
            errorLabel.ForeColor = Color.Red;
            errorLabel.Location = new Point(0, 46);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(344, 36);
            errorLabel.TabIndex = 13;
            errorLabel.Text = "Error";
            errorLabel.TextAlign = ContentAlignment.MiddleCenter;
            errorLabel.Visible = false;
            // 
            // headerLabel
            // 
            headerLabel.Dock = DockStyle.Top;
            headerLabel.Font = new Font("Futura Md BT", 22.2F, FontStyle.Regular, GraphicsUnit.Point);
            headerLabel.ForeColor = Color.White;
            headerLabel.Location = new Point(0, 0);
            headerLabel.Name = "headerLabel";
            headerLabel.Size = new Size(344, 46);
            headerLabel.TabIndex = 12;
            headerLabel.Text = "Sign in";
            headerLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.DarkSlateGray;
            ClientSize = new Size(1924, 1055);
            Controls.Add(authenticationPanel);
            Controls.Add(nameInputPanel);
            Controls.Add(loadingLabel);
            Controls.Add(mainPanel);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Messenger";
            Shown += MainForm_Shown;
            mainPanel.ResumeLayout(false);
            centerPanel.ResumeLayout(false);
            centerPanel.PerformLayout();
            nameInputPanel.ResumeLayout(false);
            nameInputPanel.PerformLayout();
            authenticationPanel.ResumeLayout(false);
            authenticationPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox2;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel mainPanel;
        private Panel centerPanel;
        private RichTextBox richTextBox1;
        private Button button4;
        private TextBox textBox1;
        private Button button3;
        private Button button1;
        private Button button2;
        private Panel nameInputPanel;
        private Button skipButton;
        private Label label2;
        private Button signUpButton;
        private TextBox nameTextBox;
        private Label label1;
        private Label loadingLabel;
        private Panel authenticationPanel;
        private Label errorLabel;
        private Label headerLabel;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Label usernameLabel;
        private Label passwordLabel;
        private Button changeOptionButton;
        private Button confirmButton;
        private Label anotherOptionLabel;
    }
}