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
            titleLabel = new Label();
            selectExtLabel = new Label();
            extensionComboBox = new ComboBox();
            installationModeLabel = new Label();
            modePanel = new Panel();
            localModeRadio = new RadioButton();
            remoteModeRadio = new RadioButton();
            modInstallPathTextBox = new TextBox();
            browseBtn = new Button();
            autoInstallDirBtn = new Button();
            modsTreeView = new TreeView();
            moveUpSelectedModBtn = new Button();
            moveDownSelectedModBtn = new Button();
            removeSelectedModBtn = new Button();
            addModBtn = new Button();
            installBtn = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            addModCtxMenu = new ContextMenuStrip(components);
            addByFolderItem = new ToolStripMenuItem();
            addByArchiveItem = new ToolStripMenuItem();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            modePanel.SuspendLayout();
            addModCtxMenu.SuspendLayout();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Consolas", 24F);
            titleLabel.Location = new Point(210, 12);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(300, 45);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "LOAD";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // selectExtLabel
            // 
            selectExtLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            selectExtLabel.Location = new Point(210, 64);
            selectExtLabel.Name = "selectExtLabel";
            selectExtLabel.Size = new Size(300, 18);
            selectExtLabel.TabIndex = 1;
            selectExtLabel.Text = "Target game";
            selectExtLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // extensionComboBox
            // 
            extensionComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            extensionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            extensionComboBox.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            extensionComboBox.FormattingEnabled = true;
            extensionComboBox.ItemHeight = 20;
            extensionComboBox.Location = new Point(125, 86);
            extensionComboBox.Name = "extensionComboBox";
            extensionComboBox.Size = new Size(470, 26);
            extensionComboBox.TabIndex = 2;
            extensionComboBox.DrawItem += extensionComboBox_DrawItem;
            extensionComboBox.SelectedIndexChanged += extensionComboBox_SelectedIndexChanged;
            // 
            // installationModeLabel
            // 
            installationModeLabel.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            installationModeLabel.Location = new Point(210, 120);
            installationModeLabel.Name = "installationModeLabel";
            installationModeLabel.Size = new Size(300, 18);
            installationModeLabel.TabIndex = 3;
            installationModeLabel.Text = "Installation Mode";
            installationModeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // modePanel
            // 
            modePanel.BorderStyle = BorderStyle.FixedSingle;
            modePanel.Controls.Add(localModeRadio);
            modePanel.Controls.Add(remoteModeRadio);
            modePanel.Location = new Point(112, 141);
            modePanel.Name = "modePanel";
            modePanel.Size = new Size(513, 64);
            modePanel.TabIndex = 4;
            modePanel.Paint += modePanel_Paint;
            // 
            // localModeRadio
            // 
            localModeRadio.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            localModeRadio.Location = new Point(14, 35);
            localModeRadio.Name = "localModeRadio";
            localModeRadio.Size = new Size(498, 22);
            localModeRadio.TabIndex = 1;
            localModeRadio.Text = "Local install (Required for emulators; Can optionally be used with real 3ds)";
            localModeRadio.UseVisualStyleBackColor = true;
            localModeRadio.CheckedChanged += installModeRadio_CheckedChanged;
            // 
            // remoteModeRadio
            // 
            remoteModeRadio.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            remoteModeRadio.Location = new Point(14, 9);
            remoteModeRadio.Name = "remoteModeRadio";
            remoteModeRadio.Size = new Size(498, 22);
            remoteModeRadio.TabIndex = 0;
            remoteModeRadio.TabStop = true;
            remoteModeRadio.Text = "Remote install over FTPD (Recommended for real 3DS; DOESNT support emu)";
            remoteModeRadio.UseVisualStyleBackColor = true;
            remoteModeRadio.CheckedChanged += installModeRadio_CheckedChanged;
            // 
            // modInstallPathTextBox
            // 
            modInstallPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            modInstallPathTextBox.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modInstallPathTextBox.Location = new Point(38, 264);
            modInstallPathTextBox.Name = "modInstallPathTextBox";
            modInstallPathTextBox.PlaceholderText = "Local mod install path will appear here...";
            modInstallPathTextBox.ReadOnly = true;
            modInstallPathTextBox.Size = new Size(640, 23);
            modInstallPathTextBox.TabIndex = 5;
            modInstallPathTextBox.TextAlign = HorizontalAlignment.Center;
            modInstallPathTextBox.TextChanged += modInstallPathTextBox_TextChanged;
            // 
            // browseBtn
            // 
            browseBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            browseBtn.Location = new Point(39, 221);
            browseBtn.Name = "browseBtn";
            browseBtn.Size = new Size(305, 28);
            browseBtn.TabIndex = 6;
            browseBtn.Text = "Browse manually...";
            browseBtn.UseVisualStyleBackColor = true;
            browseBtn.Click += browseBtn_Click;
            // 
            // autoInstallDirBtn
            // 
            autoInstallDirBtn.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            autoInstallDirBtn.Location = new Point(364, 221);
            autoInstallDirBtn.Name = "autoInstallDirBtn";
            autoInstallDirBtn.Size = new Size(315, 28);
            autoInstallDirBtn.TabIndex = 7;
            autoInstallDirBtn.Text = "Auto-detect install dir (Recommended for local install)";
            autoInstallDirBtn.UseVisualStyleBackColor = true;
            autoInstallDirBtn.Click += autoInstallDirBtn_Click;
            // 
            // modsTreeView
            // 
            modsTreeView.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold);
            modsTreeView.Location = new Point(38, 304);
            modsTreeView.Name = "modsTreeView";
            modsTreeView.Size = new Size(600, 210);
            modsTreeView.TabIndex = 8;
            modsTreeView.AfterSelect += modsTreeView_AfterSelect;
            // 
            // moveUpSelectedModBtn
            // 
            moveUpSelectedModBtn.Font = new Font("Segoe MDL2 Assets", 12F);
            moveUpSelectedModBtn.Location = new Point(648, 346);
            moveUpSelectedModBtn.Name = "moveUpSelectedModBtn";
            moveUpSelectedModBtn.Size = new Size(36, 30);
            moveUpSelectedModBtn.TabIndex = 9;
            moveUpSelectedModBtn.Text = "";
            moveUpSelectedModBtn.UseVisualStyleBackColor = true;
            moveUpSelectedModBtn.Click += moveUpSelectedModBtn_Click;
            // 
            // moveDownSelectedModBtn
            // 
            moveDownSelectedModBtn.Font = new Font("Segoe MDL2 Assets", 12F);
            moveDownSelectedModBtn.Location = new Point(648, 384);
            moveDownSelectedModBtn.Name = "moveDownSelectedModBtn";
            moveDownSelectedModBtn.Size = new Size(36, 30);
            moveDownSelectedModBtn.TabIndex = 10;
            moveDownSelectedModBtn.Text = "";
            moveDownSelectedModBtn.UseVisualStyleBackColor = true;
            moveDownSelectedModBtn.Click += moveDownSelectedModBtn_Click;
            // 
            // removeSelectedModBtn
            // 
            removeSelectedModBtn.Font = new Font("Segoe MDL2 Assets", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            removeSelectedModBtn.Location = new Point(648, 422);
            removeSelectedModBtn.Name = "removeSelectedModBtn";
            removeSelectedModBtn.Size = new Size(36, 30);
            removeSelectedModBtn.TabIndex = 11;
            removeSelectedModBtn.Text = "";
            removeSelectedModBtn.UseVisualStyleBackColor = true;
            removeSelectedModBtn.Click += removeSelectedModBtn_Click;
            // 
            // addModBtn
            // 
            addModBtn.Font = new Font("Yu Gothic UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addModBtn.Location = new Point(298, 524);
            addModBtn.Name = "addModBtn";
            addModBtn.Size = new Size(120, 32);
            addModBtn.TabIndex = 12;
            addModBtn.Text = "Add mod";
            addModBtn.UseVisualStyleBackColor = true;
            addModBtn.Click += addModBtn_Click;
            // 
            // installBtn
            // 
            installBtn.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            installBtn.Location = new Point(38, 566);
            installBtn.Name = "installBtn";
            installBtn.Size = new Size(640, 42);
            installBtn.TabIndex = 13;
            installBtn.Text = "INSTALL MODS";
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
            // LoadForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 620);
            Controls.Add(installBtn);
            Controls.Add(addModBtn);
            Controls.Add(removeSelectedModBtn);
            Controls.Add(moveDownSelectedModBtn);
            Controls.Add(moveUpSelectedModBtn);
            Controls.Add(modsTreeView);
            Controls.Add(autoInstallDirBtn);
            Controls.Add(browseBtn);
            Controls.Add(modInstallPathTextBox);
            Controls.Add(modePanel);
            Controls.Add(installationModeLabel);
            Controls.Add(extensionComboBox);
            Controls.Add(selectExtLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "LoadForm";
            Text = "Load mods";
            Load += LoadForm_Load;
            modePanel.ResumeLayout(false);
            addModCtxMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label selectExtLabel;
        private ComboBox extensionComboBox;
        private Label installationModeLabel;
        private Panel modePanel;
        private RadioButton remoteModeRadio;
        private RadioButton localModeRadio;
        private TextBox modInstallPathTextBox;
        private Button browseBtn;
        private Button autoInstallDirBtn;
        private TreeView modsTreeView;
        private Button moveUpSelectedModBtn;
        private Button moveDownSelectedModBtn;
        private Button removeSelectedModBtn;
        private Button addModBtn;
        private Button installBtn;
        private FolderBrowserDialog folderBrowserDialog1;
        private ContextMenuStrip addModCtxMenu;
        private ToolStripMenuItem addByFolderItem;
        private ToolStripMenuItem addByArchiveItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
