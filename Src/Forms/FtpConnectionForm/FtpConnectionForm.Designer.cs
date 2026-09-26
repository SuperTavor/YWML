namespace YWML.Src.Forms
{
    partial class FtpConnectionForm
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
            titleLabel = new Label();
            infoLabel = new Label();
            hostLabel = new Label();
            hostTextBox = new TextBox();
            portLabel = new Label();
            portNumeric = new NumericUpDown();
            userLabel = new Label();
            userTextBox = new TextBox();
            passLabel = new Label();
            passTextBox = new TextBox();
            saveInfoCheckBox = new CheckBox();
            testBtn = new Button();
            useBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)portNumeric).BeginInit();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Consolas", 14F, FontStyle.Bold);
            titleLabel.Location = new Point(12, 12);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(230, 22);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Remote install (FTP)";
            // 
            // infoLabel
            // 
            infoLabel.Font = new Font("Yu Gothic UI", 9F);
            infoLabel.Location = new Point(12, 40);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new Size(420, 40);
            infoLabel.TabIndex = 1;
            // 
            // hostLabel
            // 
            hostLabel.AutoSize = true;
            hostLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            hostLabel.Location = new Point(12, 85);
            hostLabel.Name = "hostLabel";
            hostLabel.Size = new Size(64, 15);
            hostLabel.TabIndex = 2;
            hostLabel.Text = "Server IP";
            // 
            // hostTextBox
            // 
            hostTextBox.Location = new Point(110, 82);
            hostTextBox.Name = "hostTextBox";
            hostTextBox.Size = new Size(320, 23);
            hostTextBox.TabIndex = 3;
            // 
            // portLabel
            // 
            portLabel.AutoSize = true;
            portLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            portLabel.Location = new Point(12, 120);
            portLabel.Name = "portLabel";
            portLabel.Size = new Size(29, 15);
            portLabel.TabIndex = 4;
            portLabel.Text = "Port";
            // 
            // portNumeric
            // 
            portNumeric.Location = new Point(110, 117);
            portNumeric.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            portNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            portNumeric.Name = "portNumeric";
            portNumeric.Size = new Size(80, 23);
            portNumeric.TabIndex = 5;
            portNumeric.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            // 
            // userLabel
            // 
            userLabel.AutoSize = true;
            userLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            userLabel.Location = new Point(12, 155);
            userLabel.Name = "userLabel";
            userLabel.Size = new Size(65, 15);
            userLabel.TabIndex = 6;
            userLabel.Text = "Username";
            // 
            // userTextBox
            // 
            userTextBox.Location = new Point(110, 152);
            userTextBox.Name = "userTextBox";
            userTextBox.Size = new Size(320, 23);
            userTextBox.TabIndex = 7;
            // 
            // passLabel
            // 
            passLabel.AutoSize = true;
            passLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            passLabel.Location = new Point(12, 190);
            passLabel.Name = "passLabel";
            passLabel.Size = new Size(61, 15);
            passLabel.TabIndex = 8;
            passLabel.Text = "Password";
            // 
            // passTextBox
            // 
            passTextBox.Location = new Point(110, 187);
            passTextBox.Name = "passTextBox";
            passTextBox.PasswordChar = '*';
            passTextBox.Size = new Size(320, 23);
            passTextBox.TabIndex = 9;
            // 
            // saveInfoCheckBox
            // 
            saveInfoCheckBox.AutoSize = true;
            saveInfoCheckBox.Font = new Font("Yu Gothic UI", 9F);
            saveInfoCheckBox.Location = new Point(110, 218);
            saveInfoCheckBox.Name = "saveInfoCheckBox";
            saveInfoCheckBox.Size = new Size(300, 19);
            saveInfoCheckBox.TabIndex = 10;
            saveInfoCheckBox.Text = "Save connection info (password is never saved)";
            saveInfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // testBtn
            // 
            testBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            testBtn.Location = new Point(110, 250);
            testBtn.Name = "testBtn";
            testBtn.Size = new Size(150, 30);
            testBtn.TabIndex = 11;
            testBtn.Text = "Test connection";
            testBtn.UseVisualStyleBackColor = true;
            testBtn.Click += testBtn_Click;
            // 
            // useBtn
            // 
            useBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            useBtn.Location = new Point(270, 250);
            useBtn.Name = "useBtn";
            useBtn.Size = new Size(160, 30);
            useBtn.TabIndex = 12;
            useBtn.Text = "Use remote install";
            useBtn.UseVisualStyleBackColor = true;
            useBtn.Click += useBtn_Click;
            // 
            // FtpConnectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 300);
            Controls.Add(useBtn);
            Controls.Add(testBtn);
            Controls.Add(saveInfoCheckBox);
            Controls.Add(passTextBox);
            Controls.Add(passLabel);
            Controls.Add(userTextBox);
            Controls.Add(userLabel);
            Controls.Add(portNumeric);
            Controls.Add(portLabel);
            Controls.Add(hostTextBox);
            Controls.Add(hostLabel);
            Controls.Add(infoLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FtpConnectionForm";
            Text = "Remote install";
            ((System.ComponentModel.ISupportInitialize)portNumeric).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label infoLabel;
        private Label hostLabel;
        private TextBox hostTextBox;
        private Label portLabel;
        private NumericUpDown portNumeric;
        private Label userLabel;
        private TextBox userTextBox;
        private Label passLabel;
        private TextBox passTextBox;
        private CheckBox saveInfoCheckBox;
        private Button testBtn;
        private Button useBtn;
    }
}
