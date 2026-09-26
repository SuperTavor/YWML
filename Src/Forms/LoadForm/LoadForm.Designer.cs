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
            components = new System.ComponentModel.Container();
            extensionComboBox = new ComboBox();
            modsTreeView = new TreeView();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            selectExtLabel = new Label();
            moveUpSelectedModBtn = new Button();
            moveDownSelectedModBtn = new Button();
            addModBtn = new Button();
            modInstallPathLabel = new Label();
            modInstallPathTextBox = new TextBox();
            browseBtn = new Button();
            removeSelectedModBtn = new Button();
            installBtn = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            addModCtxMenu = new ContextMenuStrip(components);
            addByFolderItem = new ToolStripMenuItem();
            addByArchiveItem = new ToolStripMenuItem();
            autoInstallDirBtn = new Button();
            remoteInstallBtn = new Button();
            titleLabel = new Label();
            addModCtxMenu.SuspendLayout();
            SuspendLayout();
            // 
            // extensionComboBox
            // 
            extensionComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            extensionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            extensionComboBox.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            extensionComboBox.FormattingEnabled = true;
            extensionComboBox.ItemHeight = 20;
            extensionComboBox.Location = new Point(188, 80);
            extensionComboBox.Name = "extensionComboBox";
            extensionComboBox.Size = new Size(344, 26);
            extensionComboBox.TabIndex = 6;
            extensionComboBox.DrawItem += extensionComboBox_DrawItem;
            extensionComboBox.SelectedIndexChanged += extensionComboBox_SelectedIndexChanged;
            // 
            // modsTreeView
            // 
            modsTreeView.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            modsTreeView.Location = new Point(65, 232);
            modsTreeView.Name = "modsTreeView";
            modsTreeView.Size = new Size(590, 200);
            modsTreeView.TabIndex = 7;
            modsTreeView.AfterSelect += modsTreeView_AfterSelect;
            // 
            // selectExtLabel
            // 
            selectExtLabel.AutoSize = true;
            selectExtLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            selectExtLabel.Location = new Point(318, 60);
            selectExtLabel.Name = "selectExtLabel";
            selectExtLabel.Size = new Size(84, 17);
            selectExtLabel.TabIndex = 9;
            selectExtLabel.Text = "Target game";
            // 
            // moveUpSelectedModBtn
            // 
            moveUpSelectedModBtn.Font = new Font("Segoe MDL2 Assets", 12F);
            moveUpSelectedModBtn.Location = new Point(665, 275);
            moveUpSelectedModBtn.Name = "moveUpSelectedModBtn";
            moveUpSelectedModBtn.Size = new Size(36, 29);
            moveUpSelectedModBtn.TabIndex = 11;
            moveUpSelectedModBtn.Text = "";
            moveUpSelectedModBtn.UseVisualStyleBackColor = true;
            moveUpSelectedModBtn.Click += moveUpSelectedModBtn_Click;
            // 
            // moveDownSelectedModBtn
            // 
            moveDownSelectedModBtn.Font = new Font("Segoe MDL2 Assets", 12F);
            moveDownSelectedModBtn.Location = new Point(665, 310);
            moveDownSelectedModBtn.Name = "moveDownSelectedModBtn";
            moveDownSelectedModBtn.Size = new Size(36, 28);
            moveDownSelectedModBtn.TabIndex = 12;
            moveDownSelectedModBtn.Text = "";
            moveDownSelectedModBtn.UseVisualStyleBackColor = true;
            moveDownSelectedModBtn.Click += moveDownSelectedModBtn_Click;
            // 
            // addModBtn
            // 
            addModBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addModBtn.Location = new Point(313, 440);
            addModBtn.Name = "addModBtn";
            addModBtn.Size = new Size(93, 30);
            addModBtn.TabIndex = 13;
            addModBtn.Text = "Add mod";
            addModBtn.UseVisualStyleBackColor = true;
            addModBtn.Click += addModBtn_Click;
            // 
            // modInstallPathLabel
            // 
            modInstallPathLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            modInstallPathLabel.Location = new Point(188, 120);
            modInstallPathLabel.Name = "modInstallPathLabel";
            modInstallPathLabel.Size = new Size(344, 17);
            modInstallPathLabel.TabIndex = 14;
            modInstallPathLabel.Text = "Mod installation directory";
            modInstallPathLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // modInstallPathTextBox
            // 
            modInstallPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            modInstallPathTextBox.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modInstallPathTextBox.Location = new Point(188, 140);
            modInstallPathTextBox.Name = "modInstallPathTextBox";
            modInstallPathTextBox.ReadOnly = true;
            modInstallPathTextBox.Size = new Size(344, 23);
            modInstallPathTextBox.TabIndex = 16;
            modInstallPathTextBox.TextChanged += modInstallPathTextBox_TextChanged;
            // 
            // browseBtn
            // 
            browseBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            browseBtn.Location = new Point(65, 200);
            browseBtn.Name = "browseBtn";
            browseBtn.Size = new Size(289, 25);
            browseBtn.TabIndex = 17;
            browseBtn.Text = "Browse manually...";
            browseBtn.UseVisualStyleBackColor = true;
            browseBtn.Click += browseBtn_Click;
            // 
            // removeSelectedModBtn
            // 
            removeSelectedModBtn.Font = new Font("Segoe MDL2 Assets", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            removeSelectedModBtn.Location = new Point(665, 344);
            removeSelectedModBtn.Name = "removeSelectedModBtn";
            removeSelectedModBtn.Size = new Size(36, 28);
            removeSelectedModBtn.TabIndex = 10;
            removeSelectedModBtn.Text = "";
            removeSelectedModBtn.UseVisualStyleBackColor = true;
            removeSelectedModBtn.Click += removeSelectedModBtn_Click;
            // 
            // installBtn
            // 
            installBtn.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            installBtn.Location = new Point(65, 478);
            installBtn.Name = "installBtn";
            installBtn.Size = new Size(590, 30);
            installBtn.TabIndex = 19;
            installBtn.Text = "Install listed mods";
            installBtn.UseVisualStyleBackColor = true;
            installBtn.Click += installBtn_Click;
            // 
            // addModCtxMenu
            // 
            addModCtxMenu.Items.AddRange(new ToolStripItem[] { addByFolderItem, addByArchiveItem });
            addModCtxMenu.Name = "addModCtxMenu";
            addModCtxMenu.Size = new Size(160, 48);
            addModCtxMenu.Opening += contextMenuStrip1_Opening;
            // 
            // addByFolderItem
            // 
            addByFolderItem.Font = new Font("Yu Gothic UI", 9.75F);
            addByFolderItem.Name = "addByFolderItem";
            addByFolderItem.Size = new Size(159, 22);
            addByFolderItem.Text = "...From Folder";
            addByFolderItem.Click += addByFolderItem_Click;
            // 
            // addByArchiveItem
            // 
            addByArchiveItem.Font = new Font("Yu Gothic UI", 9.75F);
            addByArchiveItem.Name = "addByArchiveItem";
            addByArchiveItem.Size = new Size(159, 22);
            addByArchiveItem.Text = "...From Archive";
            addByArchiveItem.Click += addByArchiveItem_Click;
            // 
            // autoInstallDirBtn
            // 
            autoInstallDirBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            autoInstallDirBtn.Location = new Point(362, 200);
            autoInstallDirBtn.Name = "autoInstallDirBtn";
            autoInstallDirBtn.Size = new Size(293, 25);
            autoInstallDirBtn.TabIndex = 23;
            autoInstallDirBtn.Text = "Auto-detect install dir (Recommended for emulator)";
            autoInstallDirBtn.UseVisualStyleBackColor = true;
            autoInstallDirBtn.Click += autoInstallDirBtn_Click;
            // 
            // remoteInstallBtn
            // 
            remoteInstallBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            remoteInstallBtn.Location = new Point(234, 169);
            remoteInstallBtn.Name = "remoteInstallBtn";
            remoteInstallBtn.Size = new Size(244, 25);
            remoteInstallBtn.TabIndex = 24;
            remoteInstallBtn.Text = "Remote install (Recommended for 3DS)";
            remoteInstallBtn.UseVisualStyleBackColor = true;
            remoteInstallBtn.Click += remoteInstallBtn_Click;
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Consolas", 20F, FontStyle.Bold);
            titleLabel.Location = new Point(210, 12);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(300, 40);
            titleLabel.TabIndex = 28;
            titleLabel.Text = "LOAD";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LoadForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 520);
            Controls.Add(titleLabel);
            Controls.Add(remoteInstallBtn);
            Controls.Add(autoInstallDirBtn);
            Controls.Add(installBtn);
            Controls.Add(browseBtn);
            Controls.Add(modInstallPathTextBox);
            Controls.Add(modInstallPathLabel);
            Controls.Add(addModBtn);
            Controls.Add(moveDownSelectedModBtn);
            Controls.Add(moveUpSelectedModBtn);
            Controls.Add(removeSelectedModBtn);
            Controls.Add(selectExtLabel);
            Controls.Add(modsTreeView);
            Controls.Add(extensionComboBox);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "LoadForm";
            Text = "Load mods";
            Load += LoadForm_Load;
            addModCtxMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox extensionComboBox;
        private TreeView modsTreeView;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label selectExtLabel;
        private Button moveUpSelectedModBtn;
        private Button moveDownSelectedModBtn;
        private Button addModBtn;
        private Label modInstallPathLabel;
        private TextBox modInstallPathTextBox;
        private Button browseBtn;
        private Button removeSelectedModBtn;
        private Button installBtn;
        private FolderBrowserDialog folderBrowserDialog1;
        private ContextMenuStrip addModCtxMenu;
        private ToolStripMenuItem addByFolderItem;
        private ToolStripMenuItem addByArchiveItem;
        private Button autoInstallDirBtn;
        private Button remoteInstallBtn;
        private Label titleLabel;
    }
}