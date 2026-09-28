namespace YWML.Src.Forms
{
    partial class ExtensionLibraryForm
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
            extensionsTreeView = new TreeView();
            installBtn = new Button();
            extensionsLabel = new Label();
            statusLabel = new Label();
            percentageLabel = new Label();
            exitBtn = new Button();
            uninstallBtn = new Button();
            SuspendLayout();
            // 
            // extensionsTreeView
            // 
            extensionsTreeView.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            extensionsTreeView.Location = new Point(12, 89);
            extensionsTreeView.Name = "extensionsTreeView";
            extensionsTreeView.Size = new Size(461, 217);
            extensionsTreeView.TabIndex = 0;
            extensionsTreeView.AfterSelect += extensionsTreeView_AfterSelect;
            // 
            // installBtn
            // 
            installBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            installBtn.Location = new Point(12, 327);
            installBtn.Name = "installBtn";
            installBtn.Size = new Size(227, 30);
            installBtn.TabIndex = 3;
            installBtn.Text = "Install selected extension";
            installBtn.UseVisualStyleBackColor = true;
            installBtn.Click += installBtn_Click;
            // 
            // extensionsLabel
            // 
            extensionsLabel.AutoSize = true;
            extensionsLabel.Font = new Font("Consolas", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            extensionsLabel.Location = new Point(139, 34);
            extensionsLabel.Name = "extensionsLabel";
            extensionsLabel.Size = new Size(197, 37);
            extensionsLabel.TabIndex = 4;
            extensionsLabel.Text = "EXTENSIONS";
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Font = new Font("Yu Gothic UI Semilight", 9.2F, FontStyle.Bold);
            statusLabel.Location = new Point(139, 360);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(200, 17);
            statusLabel.TabIndex = 5;
            statusLabel.Text = "Status will be displayed here.";
            // 
            // percentageLabel
            // 
            percentageLabel.AutoSize = true;
            percentageLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            percentageLabel.Location = new Point(226, 381);
            percentageLabel.Name = "percentageLabel";
            percentageLabel.Size = new Size(24, 15);
            percentageLabel.TabIndex = 6;
            percentageLabel.Text = "0%";
            // 
            // exitBtn
            // 
            exitBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exitBtn.Location = new Point(12, 372);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(47, 30);
            exitBtn.TabIndex = 7;
            exitBtn.Text = "Exit";
            exitBtn.UseVisualStyleBackColor = true;
            exitBtn.Click += exitBtn_Click;
            // 
            // uninstallBtn
            // 
            uninstallBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uninstallBtn.Location = new Point(245, 327);
            uninstallBtn.Name = "uninstallBtn";
            uninstallBtn.Size = new Size(227, 30);
            uninstallBtn.TabIndex = 8;
            uninstallBtn.Text = "Uninstall selected extension";
            uninstallBtn.UseVisualStyleBackColor = true;
            uninstallBtn.Click += uninstallBtn_Click;
            // 
            // ExtensionLibraryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(485, 414);
            Controls.Add(uninstallBtn);
            Controls.Add(exitBtn);
            Controls.Add(percentageLabel);
            Controls.Add(statusLabel);
            Controls.Add(extensionsLabel);
            Controls.Add(installBtn);
            Controls.Add(extensionsTreeView);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "ExtensionLibraryForm";
            Text = "YWML Extension Library";
            Load += ExtensionLibraryForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView extensionsTreeView;
        private Button installBtn;
        private Label extensionsLabel;
        private Label statusLabel;
        private Label percentageLabel;
        private Button exitBtn;
        private Button uninstallBtn;
    }
}