namespace YWML.Src.Forms
{
    partial class UpdatePromptForm
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
            installBtn = new Button();
            laterBtn = new Button();
            neverBtn = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Consolas", 15F);
            titleLabel.Location = new Point(0, 14);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(460, 34);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "UPDATE AVAILABLE";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // messageLabel
            // 
            messageLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            messageLabel.Location = new Point(16, 56);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(428, 62);
            messageLabel.TabIndex = 1;
            // 
            // installBtn
            // 
            installBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            installBtn.Location = new Point(16, 128);
            installBtn.Name = "installBtn";
            installBtn.Size = new Size(428, 34);
            installBtn.TabIndex = 2;
            installBtn.Text = "Install now";
            installBtn.UseVisualStyleBackColor = true;
            installBtn.Click += installBtn_Click;
            // 
            // laterBtn
            // 
            laterBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            laterBtn.Location = new Point(16, 170);
            laterBtn.Name = "laterBtn";
            laterBtn.Size = new Size(428, 34);
            laterBtn.TabIndex = 3;
            laterBtn.Text = "Later";
            laterBtn.UseVisualStyleBackColor = true;
            laterBtn.Click += laterBtn_Click;
            // 
            // neverBtn
            // 
            neverBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            neverBtn.Location = new Point(16, 212);
            neverBtn.Name = "neverBtn";
            neverBtn.Size = new Size(428, 34);
            neverBtn.TabIndex = 4;
            neverBtn.Text = "Don't remind me of this update ever again";
            neverBtn.UseVisualStyleBackColor = true;
            neverBtn.Click += neverBtn_Click;
            // 
            // UpdatePromptForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 264);
            Controls.Add(neverBtn);
            Controls.Add(laterBtn);
            Controls.Add(installBtn);
            Controls.Add(messageLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "UpdatePromptForm";
            Text = "YWML";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label titleLabel;
        private Label messageLabel;
        private Button installBtn;
        private Button laterBtn;
        private Button neverBtn;
    }
}
