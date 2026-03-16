using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Runtime.InteropServices;
using YWML.Src.ExtensionLibrary.DataClasses;
using YWML.Src.FAMerger;
using YWML.Src.Forms;
using YWML.Src.Forms.LoadForm;
using YWML.Src.Loader.DataClasses;
using YWML.Src.Utils.GeneralUtils;
using YWML.Src.Utils.Tinifan.ArchiveL5.Level5.Archive.ARC0;

namespace YWML
{
    public partial class MigrateModForm : Form
    {
        private string _modFolder = "";
        public string _romfsFolder = "";

        Stopwatch stopwatch = new Stopwatch();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        public MigrateModForm()
        {
            InitializeComponent();
            //AllocConsole();
        }

        private void MigrateModForm_Load(object sender, EventArgs e)
        {
            timer.Interval = 100; 
            timer.Tick += Timer_Tick;
        }
        private string SelectFolder(string desc)
        {
            var fbd = new FolderBrowserDialog()
            {
                Description = desc,
            };
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            else return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var f = SelectFolder("Select your mod folder (should contain an FA file or two");
            if (f != null) _modFolder = f;
            modSelectedLbl.Text = new DirectoryInfo(_modFolder).Name + " selected.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var f = SelectFolder("Select the game's original RomFS folder (should contain an FA file or two");
            if (f != null) _romfsFolder = f;
            ogRomfsLabel.Text = new DirectoryInfo(_romfsFolder).Name + " selected.";
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsedLabel.Text = $"Time Elapsed: {stopwatch.Elapsed:mm\\:ss\\.ff}";
        }
        private async Task<int> ExportDiffsIntoYWMLMod(string modpath)
        {
            stopwatch.Start();
            timer.Start();
            //create YWML project json
            var proj = new CYwmlProject()
            {
                Name = modNameTextBox.Text,
                Author = modAuthorTextBox.Text,
                Version = modVersionTextBox.Text,
            };
            //merge mods
            var merger = new CFAMerger(_modFolder, _romfsFolder);

            statusLabel.Text = "Status: Scanning files...";
            statusLabel.Refresh();

            int result = await Task.Run(() => merger.GetDiffs());

            statusLabel.Text = "Status: Processing results...";
            statusLabel.Refresh();
            switch (result)
            {
                case 0:
                    HandleYWMLProjFiles(modpath, merger, proj);
                    break;
                case 1:
                    stopwatch.Stop();
                    timer.Stop();
                    return 1;

            }

            return 0;
        }

        private void HandleYWMLProjFiles(string modpath, CFAMerger merger, CYwmlProject proj)
        {
            //write ywml proj
            var projJson = JsonConvert.SerializeObject(proj);
            File.WriteAllText(Path.Combine(modpath, "ywml.json"), projJson);
            //copy loose folders
            CopyMovSnd(modpath);
            //get all files from all mod FAs
            foreach (var f in merger.ChangedOrAddedFiles)
            {
                var modFilePath = modpath + "/include/" + f.FilePath;
                Directory.CreateDirectory(Path.GetDirectoryName(modFilePath));
                var modFile = merger.ModFAHandles[f.FAIdx].Directory.GetFileFromFullPath(f.FilePath);
                File.WriteAllBytes(modFilePath, modFile);
            }
            //clean up
            foreach (var handle in merger.ModFAHandles) handle.Close();
            stopwatch.Stop();
            timer.Stop();
        }
        void CopyDir(string src, string dst)
        {
            foreach (var dir in Directory.GetDirectories(src, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dir.Replace(src, dst));

            foreach (var file in Directory.GetFiles(src, "*.*", SearchOption.AllDirectories))
            {
                var fileDst = file.Replace(src, dst);
                Directory.CreateDirectory(Path.GetDirectoryName(fileDst));
                File.Copy(file, fileDst, true);
            }
               
                
        }
        private void CopyMovSnd(string dst)
        {
            foreach(var folder in Directory.GetDirectories(_modFolder))
            {
                string? dirName = new DirectoryInfo(folder).Name;
                if (dirName == "mov" || dirName == "snd")
                {
                    CopyDir(folder, dst + "/" + dirName);
                }
            }
        }
        //migrate
        private async void button2_Click(object sender, EventArgs e)
        {
            stopwatch.Reset();
            timeElapsedLabel.Text = "Time elapsed: None";
            //check that fields were filled
            if (String.IsNullOrEmpty(modNameTextBox.Text) 
                || String.IsNullOrEmpty(modAuthorTextBox.Text) 
                || String.IsNullOrEmpty(modVersionTextBox.Text)
                || String.IsNullOrEmpty(_modFolder)
                || String.IsNullOrEmpty(_romfsFolder))
            {
                MessageBox.Show("Please fill out all the fields.");
                return;
            }

            //select output mod folder
            MessageBox.Show("Please choose a folder where your mod will be saved.");
            string modpath = "";
            var fbd = new FolderBrowserDialog();
            fbd.Description = "Choose your output mod folder";
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                modpath = fbd.SelectedPath;
            }
            button2.Enabled = false;
            button3.Enabled = false;
            button1.Enabled = false;
            modNameTextBox.Enabled = false;
            modAuthorTextBox.Enabled = false;
            modVersionTextBox.Enabled = false;
            int result = await ExportDiffsIntoYWMLMod(modpath);
            if (result == 0)
            {
                statusLabel.Text = "Status: Waiting on user";
                MessageBox.Show("Done!");
                button2.Enabled = true;
                button3.Enabled = true;
                button1.Enabled = true;
                modNameTextBox.Enabled = true;
                modAuthorTextBox.Enabled = true;
                modVersionTextBox.Enabled = true;
            }
            else return;
        }
    }
}
