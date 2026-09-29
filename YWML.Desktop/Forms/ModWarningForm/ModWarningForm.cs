using YWML.Src.Loader.DataClasses;
using YWML.Src.Warnings.DataClasses;

namespace YWML.Src.Forms
{
    public partial class ModWarningForm : Form
    {
        private SExeFsMode? _choice;

        public ModWarningForm(SModWarning warning)
        {
            InitializeComponent();
            titleLabel.Text = warning.Title.ToUpperInvariant();
            messageLabel.Text = warning.Message;
        }

        public static SExeFsMode? ShowChoice(IWin32Window owner, SModWarning warning)
        {
            using var form = new ModWarningForm(warning);
            form.ShowDialog(owner);
            return form._choice;
        }

        private void continueValidBtn_Click(object sender, EventArgs e) => Finish(SExeFsMode.ValidOnly);

        private void continueAllBtn_Click(object sender, EventArgs e) => Finish(SExeFsMode.All);

        private void cancelBtn_Click(object sender, EventArgs e) => Finish(null);

        private void Finish(SExeFsMode? choice)
        {
            _choice = choice;
            DialogResult = choice == null ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }
    }
}
