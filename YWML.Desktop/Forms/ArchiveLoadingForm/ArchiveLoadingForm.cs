namespace YWML.Src.Forms
{
    public partial class ArchiveLoadingForm : Form
    {
        public ArchiveLoadingForm(string archiveName)
        {
            InitializeComponent();
            statusLabel.Text = $"Loading {archiveName}...";
        }

        public void SetProgress(int percent)
        {
            if (IsDisposed || Disposing)
            {
                return;
            }

            progressBar.Value = Math.Clamp(percent, progressBar.Minimum, progressBar.Maximum);
        }
    }
}
