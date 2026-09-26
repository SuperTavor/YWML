using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YWML.Src.ExtensionLibrary.DataClasses;
using YWML.Src.Loader.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.Forms
{
    public partial class AutoInstallDirForm : Form
    {
        private CInstalledExtensionMetadata _selectedExt;
        public string? GeneratedInstallPath;
        public AutoInstallDirForm(CInstalledExtensionMetadata selectedExt)
        {
            _selectedExt = selectedExt;
            InitializeComponent();
            platComboBox.DataSource = Enum.GetValues(typeof(SPlatform));

        }


        private void genModDirBtn_Click(object sender, EventArgs e)
        {
            string modInstallDir = string.Empty;

            if (platComboBox.Text != "Modded3DS")
            {
                modInstallDir = $"C:/Users/{Environment.UserName}/AppData/Roaming/{platComboBox.Text}/load/mods/{_selectedExt.TitleId}";
            }
            else
            {
                MessageBox.Show("Your 3DS microSD drive must be inserted into your computer. Please insert it if you haven't already before proceeding.\n\nClick 'OK' to select your 3DS microSD card's drive when prompted to in the file explorer. (For example: When inserting my microSD card, it showed up as the D:/ drive. So I went to \"This PC\", selected my D:/ drive and pressed \"open\". ");
                var selFolder = CGeneralUtils.ChooseFolder("Please choose your 3DS microSD card's drive");
                if (selFolder != null)
                {
                    //the last char removal is to remove the backslash at the end of the drive path (D:\)
                    if (selFolder.EndsWith("\\"))
                        selFolder = selFolder.Remove(selFolder.Length - 1);
                    modInstallDir = $"{selFolder}/luma/titles/{_selectedExt.TitleId}";
                }
                else
                {
                    MessageBox.Show("Operation was cancelled.");
                    return;
                }
            }
            GeneratedInstallPath = modInstallDir + "/romfs";
            this.Close();
        }

    }
}
