using Newtonsoft.Json;
using YWML.Src.ConfigManager;
using YWML.Src.ExtensionLibrary.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.ExtensionLibrary
{
    public class CExtensionLibrary
    {
        public struct SExtensionLibraryItem
        {
            public string FAName { get; set; }
            public string Name { get; set; }
        }
        public List<CExtension>? Extensions;
        //string is id
        public Dictionary<string, SExtensionLibraryItem> InstalledList = new();

        public void LoadInstalledList()
        {
            string installedListJson = string.Empty;
            if (File.Exists(CGeneralUtils.ExtensionInstalledList))
            {
                installedListJson = File.ReadAllText(CGeneralUtils.ExtensionInstalledList);
            }

            if (installedListJson != string.Empty)
            {
                try
                {
                    InstalledList = JsonConvert.DeserializeObject<Dictionary<string, SExtensionLibraryItem>>(installedListJson)!;
                }
                catch
                {
                    MessageBox.Show("Invalid installed list. Resetting");
                    InstalledList = new();
                }
                if (InstalledList == null)
                {
                    MessageBox.Show("Invalid installed list. Resetting");
                    InstalledList = new();
                }
            }
        }
        public void FetchData()
        {
            bool useCache = false;
            string extensionLibraryJson = string.Empty;
            var cachePath = CGeneralUtils.ExtensionLibraryCachePath;
            //Get the extension library
            HttpClient client = new()
            {
                BaseAddress = new Uri(CConfigManager.Cfg.ExtensionLibraryURL),
            };
            try
            {
                extensionLibraryJson = client.GetStringAsync(string.Empty).Result;
                File.WriteAllText(cachePath, extensionLibraryJson);
            }
            catch
            {
                var result = MessageBox.Show("An error occurred while fetching the latest extension library. Would you like to try using a cached extension library (It may be missing new extension or even straight up including broken links)?","Error",MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    if(File.Exists(cachePath))
                    {
                        MessageBox.Show("Cache available. Loading now");
                        useCache = true;
                    }
                    else
                    {
                        MessageBox.Show("Sorry, there isn't a previously cached version of the extension library on this device.");
                        throw new HttpRequestException();
                    }
                }
                else
                {
                    throw new HttpRequestException();
                }
            }
            
            if(useCache)
            {
                extensionLibraryJson = File.ReadAllText(cachePath);
            }

            Extensions = JsonConvert.DeserializeObject<List<CExtension>>(extensionLibraryJson);
        }
    }
}
