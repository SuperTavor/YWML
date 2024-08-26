namespace YWML
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
            button2 = new Button();
            extLibBtn = new Button();
            ywmlLabel = new Label();
            verLabel = new Label();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(97, 124);
            button2.Name = "button2";
            button2.Size = new Size(167, 30);
            button2.TabIndex = 2;
            button2.Text = "Load";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // extLibBtn
            // 
            extLibBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            extLibBtn.Location = new Point(97, 160);
            extLibBtn.Name = "extLibBtn";
            extLibBtn.Size = new Size(167, 30);
            extLibBtn.TabIndex = 4;
            extLibBtn.Text = "Extension library";
            extLibBtn.UseVisualStyleBackColor = true;
            extLibBtn.Click += extLibBtn_Click;
            // 
            // ywmlLabel
            // 
            ywmlLabel.AutoSize = true;
            ywmlLabel.Font = new Font("Consolas", 48F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ywmlLabel.ForeColor = SystemColors.ActiveCaptionText;
            ywmlLabel.Location = new Point(97, 33);
            ywmlLabel.Name = "ywmlLabel";
            ywmlLabel.Size = new Size(172, 75);
            ywmlLabel.TabIndex = 5;
            ywmlLabel.Text = "YWML";
            // 
            // verLabel
            // 
            verLabel.AutoSize = true;
            verLabel.Font = new Font("Consolas", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            verLabel.ForeColor = SystemColors.ActiveCaptionText;
            verLabel.Location = new Point(12, 184);
            verLabel.Name = "verLabel";
            verLabel.Size = new Size(32, 18);
            verLabel.TabIndex = 6;
            verLabel.Text = "ver";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(361, 211);
            Controls.Add(verLabel);
            Controls.Add(ywmlLabel);
            Controls.Add(extLibBtn);
            Controls.Add(button2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "YWML";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button2;
        private Button extLibBtn;
        private Label ywmlLabel;
        private Label verLabel;
    }
}
