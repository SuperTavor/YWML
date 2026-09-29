using YWML.Src.Updates.DataClasses;

namespace YWML.Src.Forms
{
    public partial class UpdatePromptForm : Form
    {
        private SUpdateChoice _choice = SUpdateChoice.Later;

        public UpdatePromptForm(SUpdateInfo update)
        {
            InitializeComponent();
            messageLabel.Text = $"A new update is available! Do you want to install it right now?\r\n\r\nVersion {update.Version}";
        }

        public static SUpdateChoice Prompt(IWin32Window owner, SUpdateInfo update)
        {
            using var form = new UpdatePromptForm(update);
            form.ShowDialog(owner);
            return form._choice;
        }

        private void installBtn_Click(object sender, EventArgs e) => Finish(SUpdateChoice.InstallNow);

        private void laterBtn_Click(object sender, EventArgs e) => Finish(SUpdateChoice.Later);

        private void neverBtn_Click(object sender, EventArgs e) => Finish(SUpdateChoice.Never);

        private void Finish(SUpdateChoice choice)
        {
            _choice = choice;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
