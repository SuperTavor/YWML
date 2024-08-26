using YWML.Src.Forms;
using YWML.Src.Forms.LoadForm;
using YWML.Src.Utils.GeneralUtils;

namespace YWML
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.verLabel.Text = CGeneralUtils.APP_VERSION;
        }

        private void extLibBtn_Click(object sender, EventArgs e)
        {
            new ExtensionLibraryForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new LoadForm().ShowDialog();
        }
    }
}
