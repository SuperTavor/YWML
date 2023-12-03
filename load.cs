using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ywml_console
{
    public class load
    {
        string citraModFolder = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Citra\\load\\mods\\0004000000167800\\romfs";
        string pathToVanillaFa = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Citra\\dump\\romfs\\0004000000167800\\yw1_a.fa";

        public void Load(string[] args)
        {
            if(!Directory.Exists(citraModFolder))
            {
                Directory.CreateDirectory(citraModFolder);
            }
            if (!CheckForValidDumpedRomfs())
            {
                Console.WriteLine("Please make sure to have your YKW1 Europe ROMFS dumped at the default citra romfs dump location. You can do that by right clicking the game in citra and clicking \"dump romfs\". then you are done. no need to move files or anything. ");
                Environment.Exit(1);
            }
            if (!ValidateYWM(args[1]))
            {
                Console.WriteLine("provided file is not a valid ywm file, consider asking who made the mod to repack it.");
                Environment.Exit(1);
            }

            string[] metadata = ReadMetadata(args[1]);
            Console.WriteLine($"Mod name: {metadata[0]}");
            Console.WriteLine($"Mod version: {metadata[1]}");
            Console.WriteLine($"Mod author: {metadata[2]}");
            Console.WriteLine("Do you want to install this mod? (write y or n)");
            string isInstall = Console.ReadLine();
            if(isInstall.Trim() == "y")
            {
                Unpatch(args[1]);
            }
            else
            {
                Environment.Exit(0);
            }
            Console.WriteLine($"Finished loading {metadata[0]}! Have fun.");

        }
        public void Unpatch(string filename)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(filename, "build", true);

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "diff.dll",
                        Arguments = $"-f -d -s {citraModFolder + "\\yw1_a.fa"} build\\delta.bin {citraModFolder + "\\yw1_a.fa"}",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    }
                };

                process.Start();
                process.WaitForExit();
                Console.WriteLine($"diff.dll -f -d -s {pathToVanillaFa} build\\delta.bin {citraModFolder + "\\yw1_a.fa"}");
            }
            
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine($"Error starting the process: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            if (Directory.Exists("build"))
            {
                foreach (string filePath in Directory.GetFiles("build"))
                {
                    File.Delete(filePath);
                }

                Directory.Delete("build");
            }
        }
        public string[] ReadMetadata(string filepath)
        {
            string metadataStr = new StreamReader(new ZipArchive(File.OpenRead(filepath)).GetEntry("metadata.ywmd").Open()).ReadToEnd();
            return metadataStr.Split("\n");
        }
        public bool ValidateYWM(string filepath)
        {
            string[] fileNames = ZipFile.OpenRead(filepath).Entries.Select(entry => entry.FullName).ToArray();
            StringBuilder sb = new StringBuilder(); 
            foreach(string file in fileNames)
            {
                sb.Append(file + "|");
            }
            if(sb.ToString() != "delta.bin|metadata.ywmd|")
            {
                return false;
            }
            else
            {
                return true;
            }
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
    }
}
