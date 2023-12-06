
namespace ywml_console
{
    public class pack
    {
        public void Pack(string[] args)
        {
            if(!Directory.Exists("build"))
            {
                Directory.CreateDirectory("build");
            }
            if(!CheckForValidDumpedRomfs())
            {
                Console.WriteLine("Please make sure to have your YKW1 Europe ROMFS dumped at the default citra romfs dump location. You can do that by right clicking the game in citra and clicking \"dump romfs\". then you are done. no need to move files or anything. ");
                Environment.Exit(1);
            }
            if (!CheckForValidModdedRomfs(args[1]))
            {
                Console.WriteLine("Please make sure `yw1_a.fa` exists at the specified modded romfs.");
            }
            string pathToVanillaFa = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Citra\\dump\\romfs\\0004000000167800\\yw1_a.fa";
            string pathToModFa = args[1] + "/yw1_a.fa";

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "diff.dll",
                        Arguments = $"-e -s {pathToVanillaFa} {pathToModFa} \"delta.bin\"",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    }
                };

                process.Start();
                process.WaitForExit(); 

            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine($"Error starting the process: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            if(File.Exists("build\\delta.bin"))
            {
                File.Delete("build\\delta.bin");
            }
            File.Move("delta.bin", "build\\delta.bin");
            File.Delete("delta.bin");
            CreateMetadata(args[2], args[3], args[4]);
            IntoZip(args[2]);
            if (Directory.Exists("build"))
            {
                foreach (string filePath in Directory.GetFiles("build"))
                {
                    File.Delete(filePath);
                }

                Directory.Delete("build");
            }
            foreach (string dir in Directory.GetDirectories(args[1]))
            {
                if (!dir.Contains("yw1_a.fa"))
                {
                    CopyDirectory(dir, "build");
                }
            }
            Console.WriteLine("Finished packing mod");
        }
        public void CreateMetadata(string modName,string modVer, string author)
        {
            if(!File.Exists("build\\metadata.ywmd"))
            {
                using (FileStream fs = File.Create("build\\metadata.ywmd"))
                {
                    //clean up
                }
            }
            File.WriteAllText("build\\metadata.ywmd", "");
            File.AppendAllText("build\\metadata.ywmd", modName + "\n");
            File.AppendAllText("build\\metadata.ywmd", modVer + "\n");
            File.AppendAllText("build\\metadata.ywmd", author + "\n");
            
        }
        public void IntoZip(string modname)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory("build", $"{modname}.ywm");
        }
        public bool CheckForValidDumpedRomfs()
        {
            string pathToFa = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Citra\\dump\\romfs\\0004000000167800\\yw1_a.fa";
            if (File.Exists(pathToFa))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckForValidModdedRomfs(string moddedRomfsPath)
        {
            string pathToModFa = moddedRomfsPath + "/yw1_a.fa";
            if (File.Exists(pathToModFa))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void CopyDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            string[] files = Directory.GetFiles(sourceDir);

            foreach (string file in files)
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true); 
            }

            string[] subdirectories = Directory.GetDirectories(sourceDir);

            foreach (string subdir in subdirectories)
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(subdir));
                CopyDirectory(subdir, destSubDir);
            }
        }
    }
}
