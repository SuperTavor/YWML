namespace YWML.Src.Forms.LoadForm
{
    partial class LoadForm
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
            extensionsLabel = new Label();
            extensionComboBox = new ComboBox();
            modsTreeView = new TreeView();
            installBtn = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            selectExtLabel = new Label();
            moveUpSelectedModBtn = new Button();
            moveDownSelectedModBtn = new Button();
            addModBtn = new Button();
            modInstallPathLabel = new Label();
            modInstallPathTextBox = new TextBox();
            browseBtn = new Button();
            removeSelectedModBtn = new Button();
            SuspendLayout();
            // 
            // extensionsLabel
            // 
            extensionsLabel.AutoSize = true;
            extensionsLabel.Font = new Font("Consolas", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            extensionsLabel.Location = new Point(208, 36);
            extensionsLabel.Name = "extensionsLabel";
            extensionsLabel.Size = new Size(179, 37);
            extensionsLabel.TabIndex = 5;
            extensionsLabel.Text = "LOAD MODS";
            // 
            // extensionComboBox
            // 
            extensionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            extensionComboBox.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            extensionComboBox.FormattingEnabled = true;
            extensionComboBox.Location = new Point(312, 116);
            extensionComboBox.Name = "extensionComboBox";
            extensionComboBox.Size = new Size(265, 25);
            extensionComboBox.TabIndex = 6;
            extensionComboBox.SelectedIndexChanged += extensionComboBox_SelectedIndexChanged;
            // 
            // modsTreeView
            // 
            modsTreeView.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            modsTreeView.Location = new Point(14, 156);
            modsTreeView.Name = "modsTreeView";
            modsTreeView.Size = new Size(590, 221);
            modsTreeView.TabIndex = 7;
            // 
            // installBtn
            // 
            installBtn.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            installBtn.Location = new Point(192, 421);
            installBtn.Name = "installBtn";
            installBtn.Size = new Size(227, 30);
            installBtn.TabIndex = 8;
            installBtn.Text = "Install selected mods";
            installBtn.UseVisualStyleBackColor = true;
            installBtn.Click += installBtn_Click;
            // 
            // selectExtLabel
            // 
            selectExtLabel.AutoSize = true;
            selectExtLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            selectExtLabel.Location = new Point(404, 96);
            selectExtLabel.Name = "selectExtLabel";
            selectExtLabel.Size = new Size(84, 17);
            selectExtLabel.TabIndex = 9;
            selectExtLabel.Text = "Target game";
            // 
            // moveUpSelectedModBtn
            // 
            moveUpSelectedModBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            moveUpSelectedModBtn.Location = new Point(163, 383);
            moveUpSelectedModBtn.Name = "moveUpSelectedModBtn";
            moveUpSelectedModBtn.Size = new Size(143, 30);
            moveUpSelectedModBtn.TabIndex = 11;
            moveUpSelectedModBtn.Text = "Move up selected";
            moveUpSelectedModBtn.UseVisualStyleBackColor = true;
            moveUpSelectedModBtn.Click += moveUpSelectedModBtn_Click;
            // 
            // moveDownSelectedModBtn
            // 
            moveDownSelectedModBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            moveDownSelectedModBtn.Location = new Point(14, 383);
            moveDownSelectedModBtn.Name = "moveDownSelectedModBtn";
            moveDownSelectedModBtn.Size = new Size(143, 30);
            moveDownSelectedModBtn.TabIndex = 12;
            moveDownSelectedModBtn.Text = "Move down selected";
            moveDownSelectedModBtn.UseVisualStyleBackColor = true;
            moveDownSelectedModBtn.Click += moveDownSelectedModBtn_Click;
            // 
            // addModBtn
            // 
            addModBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addModBtn.Location = new Point(461, 383);
            addModBtn.Name = "addModBtn";
            addModBtn.Size = new Size(143, 30);
            addModBtn.TabIndex = 13;
            addModBtn.Text = "Add mod";
            addModBtn.UseVisualStyleBackColor = true;
            addModBtn.Click += addModBtn_Click;
            // 
            // modInstallPathLabel
            // 
            modInstallPathLabel.AutoSize = true;
            modInstallPathLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            modInstallPathLabel.Location = new Point(51, 96);
            modInstallPathLabel.Name = "modInstallPathLabel";
            modInstallPathLabel.Size = new Size(164, 17);
            modInstallPathLabel.TabIndex = 14;
            modInstallPathLabel.Text = "Mod installation directory";
            // 
            // modInstallPathTextBox
            // 
            modInstallPathTextBox.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modInstallPathTextBox.Location = new Point(14, 116);
            modInstallPathTextBox.Name = "modInstallPathTextBox";
            modInstallPathTextBox.ReadOnly = true;
            modInstallPathTextBox.Size = new Size(227, 23);
            modInstallPathTextBox.TabIndex = 16;
            // 
            // browseBtn
            // 
            browseBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            browseBtn.Location = new Point(234, 117);
            browseBtn.Name = "browseBtn";
            browseBtn.Size = new Size(32, 24);
            browseBtn.TabIndex = 17;
            browseBtn.Text = "...";
            browseBtn.UseVisualStyleBackColor = true;
            browseBtn.Click += browseBtn_Click;
            // 
            // removeSelectedModBtn
            // 
            removeSelectedModBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            removeSelectedModBtn.Location = new Point(312, 383);
            removeSelectedModBtn.Name = "removeSelectedModBtn";
            removeSelectedModBtn.Size = new Size(143, 30);
            removeSelectedModBtn.TabIndex = 10;
            removeSelectedModBtn.Text = "Remove selected";
            removeSelectedModBtn.UseVisualStyleBackColor = true;
            removeSelectedModBtn.Click += removeSelectedModBtn_Click;
            // 
            // LoadForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(615, 463);
            Controls.Add(browseBtn);
            Controls.Add(modInstallPathTextBox);
            Controls.Add(modInstallPathLabel);
            Controls.Add(addModBtn);
            Controls.Add(moveDownSelectedModBtn);
            Controls.Add(moveUpSelectedModBtn);
            Controls.Add(removeSelectedModBtn);
            Controls.Add(selectExtLabel);
            Controls.Add(installBtn);
            Controls.Add(modsTreeView);
            Controls.Add(extensionComboBox);
            Controls.Add(extensionsLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "LoadForm";
            Text = "Load mods";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label extensionsLabel;
        private ComboBox extensionComboBox;
        private TreeView modsTreeView;
        private Button installBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label selectExtLabel;
        private Button moveUpSelectedModBtn;
        private Button moveDownSelectedModBtn;
        private Button addModBtn;
        private Label modInstallPathLabel;
        private TextBox modInstallPathTextBox;
        private Button browseBtn;
        private Button removeSelectedModBtn;
    }
}