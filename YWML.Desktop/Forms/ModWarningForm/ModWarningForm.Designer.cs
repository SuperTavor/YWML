namespace YWML.Src.Forms
{
    partial class ModWarningForm
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
            messageLabel = new Label();
            continueValidBtn = new Button();
            continueAllBtn = new Button();
            cancelBtn = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Consolas", 15F);
            titleLabel.Location = new Point(0, 14);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(460, 34);
            titleLabel.TabIndex = 0;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // messageLabel
            // 
            messageLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            messageLabel.Location = new Point(16, 58);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(428, 76);
            messageLabel.TabIndex = 1;
            // 
            // continueValidBtn
            // 
            continueValidBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            continueValidBtn.Location = new Point(16, 142);
            continueValidBtn.Name = "continueValidBtn";
            continueValidBtn.Size = new Size(428, 34);
            continueValidBtn.TabIndex = 2;
            continueValidBtn.Text = "Continue without invalid patches";
            continueValidBtn.UseVisualStyleBackColor = true;
            continueValidBtn.Click += continueValidBtn_Click;
            // 
            // continueAllBtn
            // 
            continueAllBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            continueAllBtn.Location = new Point(16, 184);
            continueAllBtn.Name = "continueAllBtn";
            continueAllBtn.Size = new Size(428, 34);
            continueAllBtn.TabIndex = 3;
            continueAllBtn.Text = "Continue with all files";
            continueAllBtn.UseVisualStyleBackColor = true;
            continueAllBtn.Click += continueAllBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            cancelBtn.Location = new Point(16, 226);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(428, 34);
            cancelBtn.TabIndex = 4;
            cancelBtn.Text = "Cancel";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // ModWarningForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 280);
            Controls.Add(cancelBtn);
            Controls.Add(continueAllBtn);
            Controls.Add(continueValidBtn);
            Controls.Add(messageLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ModWarningForm";
            Text = "YWML";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label titleLabel;
        private Label messageLabel;
        private Button continueValidBtn;
        private Button continueAllBtn;
        private Button cancelBtn;
    }
}
