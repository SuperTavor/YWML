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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            button2 = new Button();
            extLibBtn = new Button();
            ywmlLabel = new Label();
            verLabel = new Label();
            configOpenBtn = new Button();
            button1 = new Button();
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
            verLabel.Location = new Point(12, 267);
            verLabel.Name = "verLabel";
            verLabel.Size = new Size(32, 18);
            verLabel.TabIndex = 6;
            verLabel.Text = "ver";
            // 
            // configOpenBtn
            // 
            configOpenBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            configOpenBtn.Location = new Point(97, 196);
            configOpenBtn.Name = "configOpenBtn";
            configOpenBtn.Size = new Size(167, 30);
            configOpenBtn.TabIndex = 7;
            configOpenBtn.Text = "Locate config file";
            configOpenBtn.UseVisualStyleBackColor = true;
            configOpenBtn.Click += configOpenBtn_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(97, 232);
            button1.Name = "button1";
            button1.Size = new Size(167, 30);
            button1.TabIndex = 8;
            button1.Text = "Migrate old mod";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(361, 294);
            Controls.Add(button1);
            Controls.Add(configOpenBtn);
            Controls.Add(verLabel);
            Controls.Add(ywmlLabel);
            Controls.Add(extLibBtn);
            Controls.Add(button2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private Button configOpenBtn;
        private Button button1;
    }
}
