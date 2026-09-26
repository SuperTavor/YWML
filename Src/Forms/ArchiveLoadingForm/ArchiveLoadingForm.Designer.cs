namespace YWML.Src.Forms
{
    partial class ArchiveLoadingForm
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
            statusLabel = new Label();
            progressBar = new ProgressBar();
            SuspendLayout();
            // 
            // statusLabel
            // 
            statusLabel.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold);
            statusLabel.Location = new Point(12, 12);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(296, 18);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "Loading...";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 38);
            progressBar.Maximum = 100;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(296, 20);
            progressBar.TabIndex = 1;
            // 
            // ArchiveLoadingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 72);
            ControlBox = false;
            Controls.Add(progressBar);
            Controls.Add(statusLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ArchiveLoadingForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Loading archive";
            ResumeLayout(false);
        }

        #endregion

        private Label statusLabel;
        private ProgressBar progressBar;
    }
}
