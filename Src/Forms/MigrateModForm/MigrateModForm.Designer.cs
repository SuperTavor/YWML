namespace YWML
{
    partial class MigrateModForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrateModForm));
            ywmlLabel = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            button2 = new Button();
            label1 = new Label();
            button1 = new Button();
            modSelectedLbl = new Label();
            button3 = new Button();
            ogRomfsLabel = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            modNameTextBox = new TextBox();
            modAuthorTextBox = new TextBox();
            modVersionTextBox = new TextBox();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            statusLabel = new Label();
            timeElapsedLabel = new Label();
            SuspendLayout();
            // 
            // ywmlLabel
            // 
            ywmlLabel.AutoSize = true;
            ywmlLabel.Font = new Font("Consolas", 17F);
            ywmlLabel.ForeColor = SystemColors.ActiveCaptionText;
            ywmlLabel.Location = new Point(40, 39);
            ywmlLabel.Name = "ywmlLabel";
            ywmlLabel.Size = new Size(350, 27);
            ywmlLabel.TabIndex = 5;
            ywmlLabel.Text = "MIGRATE AN EXISTING FA MOD";
            ywmlLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // button2
            // 
            button2.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(12, 456);
            button2.Name = "button2";
            button2.Size = new Size(425, 30);
            button2.TabIndex = 7;
            button2.Text = "Migrate";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI Semibold", 12F, FontStyle.Bold);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(12, 218);
            label1.Name = "label1";
            label1.Size = new Size(425, 21);
            label1.TabIndex = 10;
            label1.Text = "Choose a folder with ONLY the mod you want to convert.";
            // 
            // button1
            // 
            button1.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(17, 294);
            button1.Name = "button1";
            button1.Size = new Size(121, 33);
            button1.TabIndex = 11;
            button1.Text = "Choose";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // modSelectedLbl
            // 
            modSelectedLbl.AutoSize = true;
            modSelectedLbl.Font = new Font("Consolas", 8F, FontStyle.Italic);
            modSelectedLbl.ForeColor = SystemColors.ActiveCaptionText;
            modSelectedLbl.Location = new Point(186, 314);
            modSelectedLbl.Name = "modSelectedLbl";
            modSelectedLbl.Size = new Size(91, 13);
            modSelectedLbl.TabIndex = 12;
            modSelectedLbl.Text = "____ selected.";
            // 
            // button3
            // 
            button3.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(17, 406);
            button3.Name = "button3";
            button3.Size = new Size(121, 33);
            button3.TabIndex = 13;
            button3.Text = "Choose";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // ogRomfsLabel
            // 
            ogRomfsLabel.AutoSize = true;
            ogRomfsLabel.Font = new Font("Consolas", 8F, FontStyle.Italic);
            ogRomfsLabel.ForeColor = SystemColors.ActiveCaptionText;
            ogRomfsLabel.Location = new Point(186, 426);
            ogRomfsLabel.Name = "ogRomfsLabel";
            ogRomfsLabel.Size = new Size(91, 13);
            ogRomfsLabel.TabIndex = 14;
            ogRomfsLabel.Text = "____ selected.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI Semibold", 12F, FontStyle.Bold);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(12, 343);
            label2.Name = "label2";
            label2.Size = new Size(355, 21);
            label2.TabIndex = 15;
            label2.Text = "Choose the game's UNMODIFIED RomFS folder";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Consolas", 8F, FontStyle.Italic);
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(558, 152);
            label3.Name = "label3";
            label3.Size = new Size(91, 13);
            label3.TabIndex = 18;
            label3.Text = "____ selected.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Yu Gothic UI Semibold", 12F, FontStyle.Bold);
            label4.ForeColor = SystemColors.ActiveCaptionText;
            label4.Location = new Point(12, 78);
            label4.Name = "label4";
            label4.Size = new Size(133, 21);
            label4.TabIndex = 16;
            label4.Text = "Add Mod Details";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Yu Gothic UI Semibold", 8F, FontStyle.Bold);
            label5.ForeColor = SystemColors.ActiveCaptionText;
            label5.Location = new Point(12, 119);
            label5.Name = "label5";
            label5.Size = new Size(62, 13);
            label5.TabIndex = 19;
            label5.Text = "Mod name";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Yu Gothic UI Semibold", 8F, FontStyle.Bold);
            label6.ForeColor = SystemColors.ActiveCaptionText;
            label6.Location = new Point(12, 150);
            label6.Name = "label6";
            label6.Size = new Size(67, 13);
            label6.TabIndex = 20;
            label6.Text = "Mod author";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Yu Gothic UI Semibold", 8F, FontStyle.Bold);
            label7.ForeColor = SystemColors.ActiveCaptionText;
            label7.Location = new Point(12, 184);
            label7.Name = "label7";
            label7.Size = new Size(71, 13);
            label7.TabIndex = 21;
            label7.Text = "Mod version";
            // 
            // modNameTextBox
            // 
            modNameTextBox.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modNameTextBox.Location = new Point(85, 115);
            modNameTextBox.Name = "modNameTextBox";
            modNameTextBox.Size = new Size(227, 23);
            modNameTextBox.TabIndex = 22;
            // 
            // modAuthorTextBox
            // 
            modAuthorTextBox.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modAuthorTextBox.Location = new Point(85, 146);
            modAuthorTextBox.Name = "modAuthorTextBox";
            modAuthorTextBox.Size = new Size(227, 23);
            modAuthorTextBox.TabIndex = 23;
            // 
            // modVersionTextBox
            // 
            modVersionTextBox.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modVersionTextBox.Location = new Point(85, 180);
            modVersionTextBox.Name = "modVersionTextBox";
            modVersionTextBox.Size = new Size(227, 23);
            modVersionTextBox.TabIndex = 24;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Consolas", 8F, FontStyle.Italic);
            label9.ForeColor = SystemColors.ActiveCaptionText;
            label9.Location = new Point(12, 99);
            label9.Name = "label9";
            label9.Size = new Size(355, 13);
            label9.TabIndex = 26;
            label9.Text = "Can be anything - just how it'll appear in the YWML loader";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Consolas", 8F, FontStyle.Italic);
            label10.ForeColor = SystemColors.ActiveCaptionText;
            label10.ImageAlign = ContentAlignment.MiddleLeft;
            label10.Location = new Point(12, 239);
            label10.Name = "label10";
            label10.Size = new Size(373, 52);
            label10.TabIndex = 27;
            label10.Text = "The folder should be structured like this:\r\n-mov (If used, if not in the mod you want to migrate, ignore)\r\n-snd (If used, if not in the mod you want to migrate, ignore)\r\n-.FA files\r\n";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Consolas", 8F, FontStyle.Italic);
            label11.ForeColor = SystemColors.ActiveCaptionText;
            label11.Location = new Point(12, 364);
            label11.Name = "label11";
            label11.Size = new Size(367, 39);
            label11.TabIndex = 28;
            label11.Text = "You can dump the game's RomFS through Citra or a modded 3DS.\r\nMake sure you are using the SAME REGION as the mod\r\n(If the mod is meant for EUR, use EUR, etc)";
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            statusLabel.ForeColor = SystemColors.ActiveCaptionText;
            statusLabel.Location = new Point(0, 499);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(130, 15);
            statusLabel.TabIndex = 30;
            statusLabel.Text = "Status: Waiting on user";
            // 
            // timeElapsedLabel
            // 
            timeElapsedLabel.AutoSize = true;
            timeElapsedLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            timeElapsedLabel.ForeColor = SystemColors.ActiveCaptionText;
            timeElapsedLabel.Location = new Point(0, 514);
            timeElapsedLabel.Name = "timeElapsedLabel";
            timeElapsedLabel.Size = new Size(112, 15);
            timeElapsedLabel.TabIndex = 31;
            timeElapsedLabel.Text = "Time elapsed: None";
            // 
            // MigrateModForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(447, 541);
            Controls.Add(timeElapsedLabel);
            Controls.Add(statusLabel);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(modVersionTextBox);
            Controls.Add(modAuthorTextBox);
            Controls.Add(modNameTextBox);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(ogRomfsLabel);
            Controls.Add(button3);
            Controls.Add(modSelectedLbl);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(ywmlLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MigrateModForm";
            Text = "Migrate mod";
            Load += MigrateModForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label ywmlLabel;
        private ContextMenuStrip contextMenuStrip1;
        private Button button2;
        private Label label1;
        private Button button1;
        private Label modSelectedLbl;
        private Button button3;
        private Label ogRomfsLabel;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox modNameTextBox;
        private TextBox modAuthorTextBox;
        private TextBox modVersionTextBox;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label statusLabel;
        private Label timeElapsedLabel;
    }
}
