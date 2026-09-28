namespace YWML.Src.Forms
{
    partial class AutoInstallDirForm
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
            genModDirBtn = new Button();
            label1 = new Label();
            platComboBox = new ComboBox();
            autogenerateWarningLabel = new Label();
            titleLabel = new Label();
            SuspendLayout();
            // 
            // genModDirBtn
            // 
            genModDirBtn.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            genModDirBtn.Location = new Point(127, 141);
            genModDirBtn.Name = "genModDirBtn";
            genModDirBtn.Size = new Size(227, 30);
            genModDirBtn.TabIndex = 26;
            genModDirBtn.Text = "Generate installation dir";
            genModDirBtn.UseVisualStyleBackColor = true;
            genModDirBtn.Click += genModDirBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            label1.Location = new Point(12, 110);
            label1.Name = "label1";
            label1.Size = new Size(120, 17);
            label1.TabIndex = 25;
            label1.Text = "Platform/Emulator";
            // 
            // platComboBox
            // 
            platComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            platComboBox.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            platComboBox.FormattingEnabled = true;
            platComboBox.Location = new Point(159, 107);
            platComboBox.Name = "platComboBox";
            platComboBox.Size = new Size(265, 25);
            platComboBox.TabIndex = 24;
            // 
            // autogenerateWarningLabel
            // 
            autogenerateWarningLabel.AutoSize = true;
            autogenerateWarningLabel.Font = new Font("Consolas", 8F, FontStyle.Italic);
            autogenerateWarningLabel.ForeColor = SystemColors.ActiveCaptionText;
            autogenerateWarningLabel.Location = new Point(12, 52);
            autogenerateWarningLabel.Name = "autogenerateWarningLabel";
            autogenerateWarningLabel.Size = new Size(433, 39);
            autogenerateWarningLabel.TabIndex = 23;
            autogenerateWarningLabel.Text = "To automatically detect your mod installation directory,\r\nChoose your emulator, or if you are on a modded 3DS, select \"Modded3DS\"\r\nand click \"Generate installation dir\"";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Consolas", 14F, FontStyle.Bold);
            titleLabel.Location = new Point(12, 19);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(240, 22);
            titleLabel.TabIndex = 27;
            titleLabel.Text = "Auto-detect install dir";
            // 
            // AutoInstallDirForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(462, 182);
            Controls.Add(titleLabel);
            Controls.Add(genModDirBtn);
            Controls.Add(label1);
            Controls.Add(platComboBox);
            Controls.Add(autogenerateWarningLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "AutoInstallDirForm";
            Text = "Auto-detect mod installation directory";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button genModDirBtn;
        private Label label1;
        private ComboBox platComboBox;
        private Label autogenerateWarningLabel;
        private Label titleLabel;
    }
}