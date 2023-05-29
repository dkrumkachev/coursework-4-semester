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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            mainPanel = new Panel();
            rightPanel = new Panel();
            filesPanel = new Panel();
            downloadFileButton = new Button();
            filesListBox = new ListBox();
            label10 = new Label();
            membersPanel = new Panel();
            chatInfoLabel = new Label();
            label12 = new Label();
            leftPanel = new Panel();
            newChatButton = new Button();
            centerPanel = new Panel();
            messageTextBox = new TextBox();
            fileButton = new Button();
            logoutButton = new Button();
            chatPanel = new Panel();
            initialChatPanelLabel = new Label();
            chatNameLabel = new Label();
            addMembersPanel = new Panel();
            usersComboBox = new ComboBox();
            label11 = new Label();
            label9 = new Label();
            addedMembersLabel = new Label();
            createGroupChatButton = new Button();
            cancelButton4 = new Button();
            addMemberButton = new Button();
            findUserErrorLabel2 = new Label();
            chooseChatTypePanel = new Panel();
            label4 = new Label();
            cancelButton1 = new Button();
            groupChatButton = new Button();
            privateChatButton = new Button();
            inputChatNamePanel = new Panel();
            chatNameErrorLabel = new Label();
            label8 = new Label();
            cancelButton3 = new Button();
            chatNameTextBox = new TextBox();
            label5 = new Label();
            continueButton = new Button();
            createSingleChatPanel = new Panel();
            usernameComboBox = new ComboBox();
            label7 = new Label();
            cancelButton2 = new Button();
            findUserErrorLabel1 = new Label();
            label3 = new Label();
            createSingleChatButton = new Button();
            nameInputPanel = new Panel();
            skipButton = new Button();
            label2 = new Label();
            signUpButton = new Button();
            nameTextBox = new TextBox();
            label1 = new Label();
            loadingLabel = new Label();
            authenticationPanel = new Panel();
            rememberMeCheckBox = new CheckBox();
            changeOptionButton = new Button();
            confirmButton = new Button();
            anotherOptionLabel = new Label();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            usernameLabel = new Label();
            passwordLabel = new Label();
            errorLabel = new Label();
            headerLabel = new Label();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            waitingLabel = new Label();
            mainPanel.SuspendLayout();
            rightPanel.SuspendLayout();
            filesPanel.SuspendLayout();
            membersPanel.SuspendLayout();
            leftPanel.SuspendLayout();
            centerPanel.SuspendLayout();
            chatPanel.SuspendLayout();
            addMembersPanel.SuspendLayout();
            chooseChatTypePanel.SuspendLayout();
            inputChatNamePanel.SuspendLayout();
            createSingleChatPanel.SuspendLayout();
            nameInputPanel.SuspendLayout();
            authenticationPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(rightPanel);
            mainPanel.Controls.Add(leftPanel);
            mainPanel.Controls.Add(centerPanel);
            mainPanel.Location = new Point(6, 6);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1570, 847);
            mainPanel.TabIndex = 0;
            mainPanel.Visible = false;
            // 
            // rightPanel
            // 
            rightPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rightPanel.BackColor = Color.FromArgb(16, 53, 53);
            rightPanel.Controls.Add(filesPanel);
            rightPanel.Controls.Add(membersPanel);
            rightPanel.Location = new Point(1314, 6);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(250, 829);
            rightPanel.TabIndex = 11;
            // 
            // filesPanel
            // 
            filesPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            filesPanel.BackColor = Color.FromArgb(16, 53, 53);
            filesPanel.Controls.Add(downloadFileButton);
            filesPanel.Controls.Add(filesListBox);
            filesPanel.Controls.Add(label10);
            filesPanel.Location = new Point(0, 416);
            filesPanel.Name = "filesPanel";
            filesPanel.Size = new Size(250, 411);
            filesPanel.TabIndex = 25;
            // 
            // downloadFileButton
            // 
            downloadFileButton.BackColor = Color.DeepSkyBlue;
            downloadFileButton.FlatStyle = FlatStyle.Flat;
            downloadFileButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            downloadFileButton.ForeColor = Color.Black;
            downloadFileButton.Location = new Point(0, 371);
            downloadFileButton.Margin = new Padding(0);
            downloadFileButton.Name = "downloadFileButton";
            downloadFileButton.Size = new Size(250, 42);
            downloadFileButton.TabIndex = 24;
            downloadFileButton.Text = "Download";
            downloadFileButton.UseVisualStyleBackColor = false;
            downloadFileButton.Visible = false;
            downloadFileButton.Click += DownloadFileButton_Click;
            // 
            // filesListBox
            // 
            filesListBox.BackColor = Color.FromArgb(16, 53, 53);
            filesListBox.BorderStyle = BorderStyle.FixedSingle;
            filesListBox.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            filesListBox.ForeColor = Color.White;
            filesListBox.FormattingEnabled = true;
            filesListBox.HorizontalScrollbar = true;
            filesListBox.ItemHeight = 31;
            filesListBox.Location = new Point(0, 48);
            filesListBox.Name = "filesListBox";
            filesListBox.Size = new Size(250, 312);
            filesListBox.TabIndex = 22;
            filesListBox.Visible = false;
            // 
            // label10
            // 
            label10.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            label10.ForeColor = Color.White;
            label10.Location = new Point(0, 0);
            label10.Name = "label10";
            label10.Size = new Size(250, 41);
            label10.TabIndex = 24;
            label10.Text = "Files:";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // membersPanel
            // 
            membersPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            membersPanel.BackColor = Color.FromArgb(16, 53, 53);
            membersPanel.Controls.Add(chatInfoLabel);
            membersPanel.Controls.Add(label12);
            membersPanel.Location = new Point(0, 0);
            membersPanel.Name = "membersPanel";
            membersPanel.Size = new Size(250, 411);
            membersPanel.TabIndex = 26;
            // 
            // chatInfoLabel
            // 
            chatInfoLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            chatInfoLabel.ForeColor = Color.White;
            chatInfoLabel.Location = new Point(0, 47);
            chatInfoLabel.MaximumSize = new Size(250, 376);
            chatInfoLabel.Name = "chatInfoLabel";
            chatInfoLabel.Size = new Size(250, 363);
            chatInfoLabel.TabIndex = 25;
            chatInfoLabel.Text = "ChatInfo:";
            chatInfoLabel.Visible = false;
            // 
            // label12
            // 
            label12.Dock = DockStyle.Top;
            label12.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            label12.ForeColor = Color.White;
            label12.Location = new Point(0, 0);
            label12.Name = "label12";
            label12.Size = new Size(250, 41);
            label12.TabIndex = 24;
            label12.Text = "Chat info:";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // leftPanel
            // 
            leftPanel.BackColor = Color.FromArgb(16, 53, 53);
            leftPanel.Controls.Add(newChatButton);
            leftPanel.Location = new Point(6, 6);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(250, 829);
            leftPanel.TabIndex = 10;
            // 
            // newChatButton
            // 
            newChatButton.BackColor = Color.DeepSkyBlue;
            newChatButton.FlatStyle = FlatStyle.Flat;
            newChatButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            newChatButton.ForeColor = Color.Black;
            newChatButton.Location = new Point(0, 0);
            newChatButton.Margin = new Padding(0);
            newChatButton.Name = "newChatButton";
            newChatButton.Size = new Size(250, 41);
            newChatButton.TabIndex = 15;
            newChatButton.Text = "New chat";
            newChatButton.TextAlign = ContentAlignment.TopCenter;
            newChatButton.UseVisualStyleBackColor = false;
            newChatButton.Click += NewChatButton_Click;
            // 
            // centerPanel
            // 
            centerPanel.Anchor = AnchorStyles.Top;
            centerPanel.BackColor = Color.FromArgb(16, 53, 53);
            centerPanel.Controls.Add(messageTextBox);
            centerPanel.Controls.Add(fileButton);
            centerPanel.Controls.Add(logoutButton);
            centerPanel.Controls.Add(chatPanel);
            centerPanel.Controls.Add(chatNameLabel);
            centerPanel.Location = new Point(262, 6);
            centerPanel.Name = "centerPanel";
            centerPanel.Size = new Size(1046, 829);
            centerPanel.TabIndex = 9;
            // 
            // messageTextBox
            // 
            messageTextBox.BackColor = Color.DarkCyan;
            messageTextBox.BorderStyle = BorderStyle.FixedSingle;
            messageTextBox.Cursor = Cursors.IBeam;
            messageTextBox.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            messageTextBox.ForeColor = SystemColors.ButtonHighlight;
            messageTextBox.Location = new Point(46, 789);
            messageTextBox.Name = "messageTextBox";
            messageTextBox.Size = new Size(1000, 39);
            messageTextBox.TabIndex = 1;
            messageTextBox.Visible = false;
            messageTextBox.KeyPress += MessageTextBox_KeyPress;
            // 
            // fileButton
            // 
            fileButton.Anchor = AnchorStyles.None;
            fileButton.BackColor = Color.Transparent;
            fileButton.BackgroundImage = (Image)resources.GetObject("fileButton.BackgroundImage");
            fileButton.BackgroundImageLayout = ImageLayout.Stretch;
            fileButton.Cursor = Cursors.Hand;
            fileButton.FlatAppearance.BorderSize = 0;
            fileButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            fileButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            fileButton.FlatStyle = FlatStyle.Flat;
            fileButton.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            fileButton.ForeColor = Color.Black;
            fileButton.Location = new Point(0, 787);
            fileButton.Name = "fileButton";
            fileButton.Size = new Size(40, 41);
            fileButton.TabIndex = 4;
            fileButton.UseVisualStyleBackColor = false;
            fileButton.Visible = false;
            fileButton.Click += FileButton_Click;
            // 
            // logoutButton
            // 
            logoutButton.BackColor = Color.Cyan;
            logoutButton.FlatStyle = FlatStyle.Flat;
            logoutButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            logoutButton.ForeColor = Color.Black;
            logoutButton.Location = new Point(935, 0);
            logoutButton.Name = "logoutButton";
            logoutButton.Size = new Size(111, 40);
            logoutButton.TabIndex = 17;
            logoutButton.Text = "Log out";
            logoutButton.TextAlign = ContentAlignment.TopCenter;
            logoutButton.UseVisualStyleBackColor = false;
            logoutButton.Click += LogoutButton_Click;
            // 
            // chatPanel
            // 
            chatPanel.BackColor = Color.DarkSlateGray;
            chatPanel.Controls.Add(initialChatPanelLabel);
            chatPanel.Location = new Point(0, 47);
            chatPanel.Margin = new Padding(10);
            chatPanel.MaximumSize = new Size(1046, 0);
            chatPanel.Name = "chatPanel";
            chatPanel.Size = new Size(1046, 727);
            chatPanel.TabIndex = 21;
            // 
            // initialChatPanelLabel
            // 
            initialChatPanelLabel.AutoSize = true;
            initialChatPanelLabel.Font = new Font("Futura Md BT", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            initialChatPanelLabel.ForeColor = Color.Gainsboro;
            initialChatPanelLabel.Location = new Point(28, 89);
            initialChatPanelLabel.Name = "initialChatPanelLabel";
            initialChatPanelLabel.Size = new Size(507, 41);
            initialChatPanelLabel.TabIndex = 16;
            initialChatPanelLabel.Text = "Select a chat to start messaging";
            initialChatPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chatNameLabel
            // 
            chatNameLabel.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            chatNameLabel.ForeColor = Color.White;
            chatNameLabel.Location = new Point(0, 0);
            chatNameLabel.Name = "chatNameLabel";
            chatNameLabel.Size = new Size(929, 41);
            chatNameLabel.TabIndex = 20;
            chatNameLabel.Text = "Chat";
            chatNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // addMembersPanel
            // 
            addMembersPanel.Controls.Add(usersComboBox);
            addMembersPanel.Controls.Add(label11);
            addMembersPanel.Controls.Add(label9);
            addMembersPanel.Controls.Add(addedMembersLabel);
            addMembersPanel.Controls.Add(createGroupChatButton);
            addMembersPanel.Controls.Add(cancelButton4);
            addMembersPanel.Controls.Add(addMemberButton);
            addMembersPanel.Controls.Add(findUserErrorLabel2);
            addMembersPanel.Location = new Point(1141, 899);
            addMembersPanel.Name = "addMembersPanel";
            addMembersPanel.Size = new Size(250, 401);
            addMembersPanel.TabIndex = 20;
            addMembersPanel.Visible = false;
            // 
            // usersComboBox
            // 
            usersComboBox.AutoCompleteMode = AutoCompleteMode.Append;
            usersComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            usersComboBox.BackColor = Color.DarkCyan;
            usersComboBox.FlatStyle = FlatStyle.Flat;
            usersComboBox.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            usersComboBox.ForeColor = Color.White;
            usersComboBox.FormattingEnabled = true;
            usersComboBox.Location = new Point(15, 140);
            usersComboBox.MaxDropDownItems = 10;
            usersComboBox.Name = "usersComboBox";
            usersComboBox.Size = new Size(222, 39);
            usersComboBox.TabIndex = 27;
            usersComboBox.TextUpdate += UsernameComboBox_TextChanged;
            usersComboBox.KeyPress += UsersComboBox_KeyPress;
            // 
            // label11
            // 
            label11.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(0, 63);
            label11.Name = "label11";
            label11.Size = new Size(250, 74);
            label11.TabIndex = 27;
            label11.Text = "Enter or select a username:";
            label11.TextAlign = ContentAlignment.TopCenter;
            // 
            // label9
            // 
            label9.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            label9.ForeColor = Color.White;
            label9.Location = new Point(0, 0);
            label9.Name = "label9";
            label9.Size = new Size(250, 41);
            label9.TabIndex = 26;
            label9.Text = "New group chat";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // addedMembersLabel
            // 
            addedMembersLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            addedMembersLabel.Location = new Point(17, 326);
            addedMembersLabel.Name = "addedMembersLabel";
            addedMembersLabel.Size = new Size(220, 47);
            addedMembersLabel.TabIndex = 23;
            addedMembersLabel.Text = "Added members:";
            // 
            // createGroupChatButton
            // 
            createGroupChatButton.BackColor = Color.DeepSkyBlue;
            createGroupChatButton.FlatStyle = FlatStyle.Flat;
            createGroupChatButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            createGroupChatButton.ForeColor = Color.Black;
            createGroupChatButton.Location = new Point(17, 275);
            createGroupChatButton.Name = "createGroupChatButton";
            createGroupChatButton.Size = new Size(107, 42);
            createGroupChatButton.TabIndex = 22;
            createGroupChatButton.Text = "Create";
            createGroupChatButton.UseVisualStyleBackColor = false;
            createGroupChatButton.Click += CreateGroupChatButton_Click;
            // 
            // cancelButton4
            // 
            cancelButton4.BackColor = Color.Cyan;
            cancelButton4.FlatStyle = FlatStyle.Flat;
            cancelButton4.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            cancelButton4.ForeColor = Color.Black;
            cancelButton4.Location = new Point(130, 275);
            cancelButton4.Name = "cancelButton4";
            cancelButton4.Size = new Size(107, 42);
            cancelButton4.TabIndex = 19;
            cancelButton4.Text = "Cancel";
            cancelButton4.UseVisualStyleBackColor = false;
            cancelButton4.Click += CancelButton_Click;
            // 
            // addMemberButton
            // 
            addMemberButton.BackColor = Color.DeepSkyBlue;
            addMemberButton.FlatStyle = FlatStyle.Flat;
            addMemberButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            addMemberButton.ForeColor = Color.Black;
            addMemberButton.Location = new Point(15, 211);
            addMemberButton.Name = "addMemberButton";
            addMemberButton.Size = new Size(222, 42);
            addMemberButton.TabIndex = 20;
            addMemberButton.Text = "Add member";
            addMemberButton.UseVisualStyleBackColor = false;
            addMemberButton.Click += AddMemberButton_Click;
            // 
            // findUserErrorLabel2
            // 
            findUserErrorLabel2.Font = new Font("Futura Bk BT", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            findUserErrorLabel2.ForeColor = Color.Red;
            findUserErrorLabel2.Location = new Point(4, 182);
            findUserErrorLabel2.Name = "findUserErrorLabel2";
            findUserErrorLabel2.Size = new Size(250, 26);
            findUserErrorLabel2.TabIndex = 18;
            findUserErrorLabel2.Text = "user not found";
            findUserErrorLabel2.TextAlign = ContentAlignment.TopCenter;
            findUserErrorLabel2.Visible = false;
            // 
            // chooseChatTypePanel
            // 
            chooseChatTypePanel.Controls.Add(label4);
            chooseChatTypePanel.Controls.Add(cancelButton1);
            chooseChatTypePanel.Controls.Add(groupChatButton);
            chooseChatTypePanel.Controls.Add(privateChatButton);
            chooseChatTypePanel.Location = new Point(589, 892);
            chooseChatTypePanel.Name = "chooseChatTypePanel";
            chooseChatTypePanel.Size = new Size(250, 280);
            chooseChatTypePanel.TabIndex = 21;
            chooseChatTypePanel.Visible = false;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            label4.ForeColor = Color.White;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(250, 41);
            label4.TabIndex = 23;
            label4.Text = "Create a chat";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cancelButton1
            // 
            cancelButton1.BackColor = Color.Cyan;
            cancelButton1.FlatStyle = FlatStyle.Flat;
            cancelButton1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            cancelButton1.ForeColor = Color.Black;
            cancelButton1.Location = new Point(0, 187);
            cancelButton1.Margin = new Padding(0);
            cancelButton1.Name = "cancelButton1";
            cancelButton1.Size = new Size(250, 41);
            cancelButton1.TabIndex = 22;
            cancelButton1.Text = "Cancel";
            cancelButton1.UseVisualStyleBackColor = false;
            cancelButton1.Click += CancelButton_Click;
            // 
            // groupChatButton
            // 
            groupChatButton.BackColor = Color.DeepSkyBlue;
            groupChatButton.FlatStyle = FlatStyle.Flat;
            groupChatButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            groupChatButton.ForeColor = Color.Black;
            groupChatButton.Location = new Point(0, 111);
            groupChatButton.Margin = new Padding(0);
            groupChatButton.Name = "groupChatButton";
            groupChatButton.Size = new Size(250, 41);
            groupChatButton.TabIndex = 21;
            groupChatButton.Text = "Group chat";
            groupChatButton.UseVisualStyleBackColor = false;
            groupChatButton.Click += GroupChatButton_Click;
            // 
            // privateChatButton
            // 
            privateChatButton.BackColor = Color.DeepSkyBlue;
            privateChatButton.FlatStyle = FlatStyle.Flat;
            privateChatButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            privateChatButton.ForeColor = Color.Black;
            privateChatButton.Location = new Point(0, 59);
            privateChatButton.Margin = new Padding(0);
            privateChatButton.Name = "privateChatButton";
            privateChatButton.Size = new Size(250, 41);
            privateChatButton.TabIndex = 20;
            privateChatButton.Text = "Private chat";
            privateChatButton.UseVisualStyleBackColor = false;
            privateChatButton.Click += PrivateChatButton_Click;
            // 
            // inputChatNamePanel
            // 
            inputChatNamePanel.Controls.Add(chatNameErrorLabel);
            inputChatNamePanel.Controls.Add(label8);
            inputChatNamePanel.Controls.Add(cancelButton3);
            inputChatNamePanel.Controls.Add(chatNameTextBox);
            inputChatNamePanel.Controls.Add(label5);
            inputChatNamePanel.Controls.Add(continueButton);
            inputChatNamePanel.Location = new Point(1416, 899);
            inputChatNamePanel.Name = "inputChatNamePanel";
            inputChatNamePanel.Size = new Size(250, 310);
            inputChatNamePanel.TabIndex = 20;
            inputChatNamePanel.Visible = false;
            // 
            // chatNameErrorLabel
            // 
            chatNameErrorLabel.Font = new Font("Futura Bk BT", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            chatNameErrorLabel.ForeColor = Color.Red;
            chatNameErrorLabel.Location = new Point(0, 243);
            chatNameErrorLabel.Name = "chatNameErrorLabel";
            chatNameErrorLabel.Size = new Size(250, 69);
            chatNameErrorLabel.TabIndex = 25;
            chatNameErrorLabel.Text = "Please specify a chat name.";
            chatNameErrorLabel.TextAlign = ContentAlignment.TopCenter;
            chatNameErrorLabel.Visible = false;
            // 
            // label8
            // 
            label8.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            label8.ForeColor = Color.White;
            label8.Location = new Point(0, 0);
            label8.Name = "label8";
            label8.Size = new Size(250, 41);
            label8.TabIndex = 25;
            label8.Text = "New group chat";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cancelButton3
            // 
            cancelButton3.BackColor = Color.Cyan;
            cancelButton3.FlatStyle = FlatStyle.Flat;
            cancelButton3.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            cancelButton3.ForeColor = Color.Black;
            cancelButton3.Location = new Point(142, 198);
            cancelButton3.Name = "cancelButton3";
            cancelButton3.Size = new Size(94, 42);
            cancelButton3.TabIndex = 19;
            cancelButton3.Text = "Cancel";
            cancelButton3.UseVisualStyleBackColor = false;
            cancelButton3.Click += CancelButton_Click;
            // 
            // chatNameTextBox
            // 
            chatNameTextBox.BackColor = Color.DarkCyan;
            chatNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            chatNameTextBox.Cursor = Cursors.IBeam;
            chatNameTextBox.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            chatNameTextBox.ForeColor = Color.White;
            chatNameTextBox.Location = new Point(14, 140);
            chatNameTextBox.Name = "chatNameTextBox";
            chatNameTextBox.Size = new Size(222, 38);
            chatNameTextBox.TabIndex = 17;
            chatNameTextBox.TextChanged += ChatNameTextBox_TextChanged;
            // 
            // label5
            // 
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(0, 63);
            label5.Name = "label5";
            label5.Size = new Size(250, 65);
            label5.TabIndex = 16;
            label5.Text = "Create a name for the chat";
            label5.TextAlign = ContentAlignment.TopCenter;
            // 
            // continueButton
            // 
            continueButton.BackColor = Color.DeepSkyBlue;
            continueButton.FlatStyle = FlatStyle.Flat;
            continueButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            continueButton.ForeColor = Color.Black;
            continueButton.Location = new Point(14, 198);
            continueButton.Name = "continueButton";
            continueButton.Size = new Size(122, 42);
            continueButton.TabIndex = 15;
            continueButton.Text = "Continue";
            continueButton.UseVisualStyleBackColor = false;
            continueButton.Click += ContinueButton_Click;
            // 
            // createSingleChatPanel
            // 
            createSingleChatPanel.Controls.Add(usernameComboBox);
            createSingleChatPanel.Controls.Add(label7);
            createSingleChatPanel.Controls.Add(cancelButton2);
            createSingleChatPanel.Controls.Add(findUserErrorLabel1);
            createSingleChatPanel.Controls.Add(label3);
            createSingleChatPanel.Controls.Add(createSingleChatButton);
            createSingleChatPanel.Location = new Point(865, 899);
            createSingleChatPanel.Name = "createSingleChatPanel";
            createSingleChatPanel.Size = new Size(250, 294);
            createSingleChatPanel.TabIndex = 15;
            createSingleChatPanel.Visible = false;
            // 
            // usernameComboBox
            // 
            usernameComboBox.AutoCompleteMode = AutoCompleteMode.Append;
            usernameComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            usernameComboBox.BackColor = Color.DarkCyan;
            usernameComboBox.FlatStyle = FlatStyle.Flat;
            usernameComboBox.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            usernameComboBox.ForeColor = Color.White;
            usernameComboBox.FormattingEnabled = true;
            usernameComboBox.Location = new Point(13, 140);
            usernameComboBox.MaxDropDownItems = 10;
            usernameComboBox.Name = "usernameComboBox";
            usernameComboBox.Size = new Size(222, 39);
            usernameComboBox.TabIndex = 28;
            usernameComboBox.TextUpdate += UsernameComboBox_TextChanged;
            usernameComboBox.KeyPress += UsernameComboBox_KeyPress;
            // 
            // label7
            // 
            label7.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            label7.ForeColor = Color.White;
            label7.Location = new Point(0, 0);
            label7.Name = "label7";
            label7.Size = new Size(250, 41);
            label7.TabIndex = 24;
            label7.Text = "New private chat";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cancelButton2
            // 
            cancelButton2.BackColor = Color.Cyan;
            cancelButton2.FlatStyle = FlatStyle.Flat;
            cancelButton2.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            cancelButton2.ForeColor = Color.Black;
            cancelButton2.Location = new Point(126, 198);
            cancelButton2.Name = "cancelButton2";
            cancelButton2.Size = new Size(109, 42);
            cancelButton2.TabIndex = 19;
            cancelButton2.Text = "Cancel";
            cancelButton2.UseVisualStyleBackColor = false;
            cancelButton2.Click += CancelButton_Click;
            // 
            // findUserErrorLabel1
            // 
            findUserErrorLabel1.Font = new Font("Futura Bk BT", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            findUserErrorLabel1.ForeColor = Color.Red;
            findUserErrorLabel1.Location = new Point(0, 243);
            findUserErrorLabel1.Name = "findUserErrorLabel1";
            findUserErrorLabel1.Size = new Size(250, 38);
            findUserErrorLabel1.TabIndex = 18;
            findUserErrorLabel1.Text = "user not found";
            findUserErrorLabel1.TextAlign = ContentAlignment.TopCenter;
            findUserErrorLabel1.Visible = false;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(0, 63);
            label3.Name = "label3";
            label3.Size = new Size(250, 69);
            label3.TabIndex = 16;
            label3.Text = "Enter or select a username:";
            label3.TextAlign = ContentAlignment.TopCenter;
            // 
            // createSingleChatButton
            // 
            createSingleChatButton.BackColor = Color.DeepSkyBlue;
            createSingleChatButton.FlatStyle = FlatStyle.Flat;
            createSingleChatButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            createSingleChatButton.ForeColor = Color.Black;
            createSingleChatButton.Location = new Point(13, 198);
            createSingleChatButton.Name = "createSingleChatButton";
            createSingleChatButton.Size = new Size(109, 42);
            createSingleChatButton.TabIndex = 15;
            createSingleChatButton.Text = "Create";
            createSingleChatButton.UseVisualStyleBackColor = false;
            createSingleChatButton.Click += CreateSingleChatButton_Click;
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
            nameTextBox.BackColor = Color.DarkCyan;
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.Cursor = Cursors.IBeam;
            nameTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            nameTextBox.ForeColor = SystemColors.ButtonHighlight;
            nameTextBox.Location = new Point(42, 150);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(332, 40);
            nameTextBox.TabIndex = 16;
            nameTextBox.KeyPress += NameTextBox_KeyPress;
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
            authenticationPanel.Controls.Add(rememberMeCheckBox);
            authenticationPanel.Controls.Add(changeOptionButton);
            authenticationPanel.Controls.Add(confirmButton);
            authenticationPanel.Controls.Add(anotherOptionLabel);
            authenticationPanel.Controls.Add(usernameTextBox);
            authenticationPanel.Controls.Add(passwordTextBox);
            authenticationPanel.Controls.Add(usernameLabel);
            authenticationPanel.Controls.Add(passwordLabel);
            authenticationPanel.Controls.Add(errorLabel);
            authenticationPanel.Controls.Add(headerLabel);
            authenticationPanel.Location = new Point(1590, 478);
            authenticationPanel.Name = "authenticationPanel";
            authenticationPanel.Size = new Size(344, 462);
            authenticationPanel.TabIndex = 14;
            authenticationPanel.Visible = false;
            // 
            // rememberMeCheckBox
            // 
            rememberMeCheckBox.AutoSize = true;
            rememberMeCheckBox.Font = new Font("Futura Bk BT", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            rememberMeCheckBox.Location = new Point(3, 276);
            rememberMeCheckBox.Name = "rememberMeCheckBox";
            rememberMeCheckBox.Size = new Size(182, 31);
            rememberMeCheckBox.TabIndex = 21;
            rememberMeCheckBox.Text = "Remember me";
            rememberMeCheckBox.UseVisualStyleBackColor = true;
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
            usernameTextBox.BackColor = Color.DarkCyan;
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Cursor = Cursors.IBeam;
            usernameTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            usernameTextBox.ForeColor = SystemColors.ButtonHighlight;
            usernameTextBox.Location = new Point(0, 122);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(344, 40);
            usernameTextBox.TabIndex = 14;
            usernameTextBox.KeyPress += UsernameTextBox_KeyPress;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Anchor = AnchorStyles.Left;
            passwordTextBox.BackColor = Color.DarkCyan;
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.Cursor = Cursors.IBeam;
            passwordTextBox.Font = new Font("Futura Bk BT", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            passwordTextBox.ForeColor = SystemColors.ButtonHighlight;
            passwordTextBox.Location = new Point(0, 222);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(344, 40);
            passwordTextBox.TabIndex = 15;
            passwordTextBox.KeyPress += PasswordTextBox_KeyPress;
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
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // waitingLabel
            // 
            waitingLabel.AutoSize = true;
            waitingLabel.BackColor = Color.DarkSlateGray;
            waitingLabel.Font = new Font("Futura Md BT", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            waitingLabel.ForeColor = Color.Gainsboro;
            waitingLabel.Location = new Point(12, 924);
            waitingLabel.Name = "waitingLabel";
            waitingLabel.Size = new Size(525, 41);
            waitingLabel.TabIndex = 17;
            waitingLabel.Text = "Waiting for the user to connect...";
            waitingLabel.TextAlign = ContentAlignment.MiddleCenter;
            waitingLabel.Visible = false;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(16, 53, 53);
            ClientSize = new Size(1924, 1055);
            Controls.Add(waitingLabel);
            Controls.Add(addMembersPanel);
            Controls.Add(createSingleChatPanel);
            Controls.Add(inputChatNamePanel);
            Controls.Add(chooseChatTypePanel);
            Controls.Add(authenticationPanel);
            Controls.Add(nameInputPanel);
            Controls.Add(loadingLabel);
            Controls.Add(mainPanel);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Messenger";
            FormClosed += MainForm_FormClosed;
            Shown += MainForm_Shown;
            mainPanel.ResumeLayout(false);
            rightPanel.ResumeLayout(false);
            filesPanel.ResumeLayout(false);
            membersPanel.ResumeLayout(false);
            leftPanel.ResumeLayout(false);
            centerPanel.ResumeLayout(false);
            centerPanel.PerformLayout();
            chatPanel.ResumeLayout(false);
            chatPanel.PerformLayout();
            addMembersPanel.ResumeLayout(false);
            chooseChatTypePanel.ResumeLayout(false);
            inputChatNamePanel.ResumeLayout(false);
            inputChatNamePanel.PerformLayout();
            createSingleChatPanel.ResumeLayout(false);
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
        private Button button4;
        private TextBox messageTextBox;
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
        private CheckBox rememberMeCheckBox;
        private Panel createSingleChatPanel;
        private Label findUserErrorLabel1;
        private Label label3;
        private Button createSingleChatButton;
        private Button newChatButton;
        private Button logoutButton;
        private Button cancelButton2;
        private Label chatNameLabel;
        private Button fileButton;
        private Panel chatPanel;
        private Label initialChatPanelLabel;
        private Panel chooseChatTypePanel;
        private Button cancelButton1;
        private Button groupChatButton;
        private Button privateChatButton;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Panel inputChatNamePanel;
        private Button cancelButton3;
        private TextBox chatNameTextBox;
        private Label label5;
        private Button continueButton;
        private Panel addMembersPanel;
        private Button createGroupChatButton;
        private Button cancelButton4;
        private Button addMemberButton;
        private Label findUserErrorLabel2;
        private Label waitingLabel;
        private Label addedMembersLabel;
        private Label label9;
        private Label label7;
        private Label label4;
        private Label label8;
        private Label chatNameErrorLabel;
        private Label anotherOptionLabel;
        private Panel filesPanel;
        private Label label10;
        private Panel membersPanel;
        private Label label12;
        private ComboBox usersComboBox;
        private Label label11;
        private Label chatInfoLabel;
        private ComboBox usernameComboBox;
        private ListBox filesListBox;
        private Button downloadFileButton;
    }
}