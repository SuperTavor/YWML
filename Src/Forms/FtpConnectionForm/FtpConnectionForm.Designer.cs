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
            infoLabel = new Label();
            hostLabel = new Label();
            hostTextBox = new TextBox();
            portLabel = new Label();
            portNumeric = new NumericUpDown();
            saveInfoCheckBox = new CheckBox();
            testBtn = new Button();
            useBtn = new Button();
            titleLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)portNumeric).BeginInit();
            SuspendLayout();
            // 
            // infoLabel
            // 
            infoLabel.Font = new Font("Consolas", 8F, FontStyle.Italic);
            infoLabel.Location = new Point(12, 40);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new Size(420, 40);
            infoLabel.TabIndex = 1;
            // 
            // hostLabel
            // 
            hostLabel.AutoSize = true;
            hostLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            hostLabel.Location = new Point(12, 85);
            hostLabel.Name = "hostLabel";
            hostLabel.Size = new Size(62, 17);
            hostLabel.TabIndex = 2;
            hostLabel.Text = "Server IP";
            // 
            // hostTextBox
            // 
            hostTextBox.BorderStyle = BorderStyle.FixedSingle;
            hostTextBox.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            hostTextBox.Location = new Point(110, 82);
            hostTextBox.Name = "hostTextBox";
            hostTextBox.Size = new Size(320, 23);
            hostTextBox.TabIndex = 3;
            // 
            // portLabel
            // 
            portLabel.AutoSize = true;
            portLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            portLabel.Location = new Point(12, 120);
            portLabel.Name = "portLabel";
            portLabel.Size = new Size(34, 17);
            portLabel.TabIndex = 4;
            portLabel.Text = "Port";
            // 
            // portNumeric
            // 
            portNumeric.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            portNumeric.Location = new Point(110, 117);
            portNumeric.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            portNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            portNumeric.Name = "portNumeric";
            portNumeric.Size = new Size(80, 23);
            portNumeric.TabIndex = 5;
            portNumeric.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            // 
            // saveInfoCheckBox
            // 
            saveInfoCheckBox.AutoSize = true;
            saveInfoCheckBox.Checked = true;
            saveInfoCheckBox.CheckState = CheckState.Checked;
            saveInfoCheckBox.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            saveInfoCheckBox.Location = new Point(110, 150);
            saveInfoCheckBox.Name = "saveInfoCheckBox";
            saveInfoCheckBox.Size = new Size(138, 19);
            saveInfoCheckBox.TabIndex = 6;
            saveInfoCheckBox.Text = "Save connection info";
            saveInfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // testBtn
            // 
            testBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            testBtn.Location = new Point(110, 182);
            testBtn.Name = "testBtn";
            testBtn.Size = new Size(150, 30);
            testBtn.TabIndex = 7;
            testBtn.Text = "Test connection";
            testBtn.UseVisualStyleBackColor = true;
            testBtn.Click += testBtn_Click;
            // 
            // useBtn
            // 
            useBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            useBtn.Location = new Point(270, 182);
            useBtn.Name = "useBtn";
            useBtn.Size = new Size(160, 30);
            useBtn.TabIndex = 8;
            useBtn.Text = "Use remote install";
            useBtn.UseVisualStyleBackColor = true;
            useBtn.Click += useBtn_Click;
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Consolas", 17F);
            titleLabel.Location = new Point(-17, 17);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(498, 45);
            titleLabel.TabIndex = 9;
            titleLabel.Text = "REMOTE INSTALL (FTPD)";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FtpConnectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 230);
            Controls.Add(titleLabel);
            Controls.Add(useBtn);
            Controls.Add(testBtn);
            Controls.Add(saveInfoCheckBox);
            Controls.Add(portNumeric);
            Controls.Add(portLabel);
            Controls.Add(hostTextBox);
            Controls.Add(hostLabel);
            Controls.Add(infoLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FtpConnectionForm";
            Text = "Remote install";
            ((System.ComponentModel.ISupportInitialize)portNumeric).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label infoLabel;
        private Label hostLabel;
        private TextBox hostTextBox;
        private Label portLabel;
        private NumericUpDown portNumeric;
        private CheckBox saveInfoCheckBox;
        private Button testBtn;
        private Button useBtn;
        private Label titleLabel;
    }
}
