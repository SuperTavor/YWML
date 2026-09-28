using Newtonsoft.Json;
using YWML.Src.ConfigManager;
using YWML.Src.ExtensionLibrary.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.ExtensionLibrary
{
    public class CExtensionLibrary
    {
        public CExtensionLibInfo ExtensionInfo = new();
        //string is id
        public Dictionary<string, CInstalledExtensionMetadata> InstalledList = new();

        public void LoadInstalledList()
        {
            if (!File.Exists(CGeneralUtils.ExtensionInstalledList))
            {
                return;
            }

            var installedListJson = File.ReadAllText(CGeneralUtils.ExtensionInstalledList);
            if (installedListJson == string.Empty)
            {
                return;
            }

            InstalledList = DeserializeInstalledListOrReset(installedListJson);
            RemoveMissingExtensions();
        }

        public void SaveInstalledList()
        {
            Directory.CreateDirectory(CGeneralUtils.ExtensionInstallDirectory);
            File.WriteAllText(CGeneralUtils.ExtensionInstalledList, JsonConvert.SerializeObject(InstalledList));
        }

        private void RemoveMissingExtensions()
        {
            var missing = InstalledList.Keys
                .Where(id => !File.Exists(GetInstalledFaPath(id)))
                .ToList();

            if (missing.Count == 0)
            {
                return;
            }

            foreach (var id in missing)
            {
                InstalledList.Remove(id);
            }

            SaveInstalledList();
        }

        private static string GetInstalledFaPath(string id)
        {
            return Path.Combine(CGeneralUtils.ExtensionInstallDirectory, id, "patchable.fa");
        }

        private static Dictionary<string, CInstalledExtensionMetadata> DeserializeInstalledListOrReset(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, CInstalledExtensionMetadata>>(json) ?? new();
            }
            catch
            {
                return new();
            }
        }

        public void FetchData()
        {
            var cachePath = CGeneralUtils.ExtensionLibraryCachePath;

            using var client = new HttpClient
            {
                BaseAddress = new Uri(CConfigManager.Cfg.ExtensionLibraryURL),
            };

            var extensionLibraryJson = client.GetStringAsync(string.Empty).Result;
            File.WriteAllText(cachePath, extensionLibraryJson);

            ExtensionInfo = JsonConvert.DeserializeObject<CExtensionLibInfo>(extensionLibraryJson);
        }

        public void LoadCachedData()
        {
            var extensionLibraryJson = File.ReadAllText(CGeneralUtils.ExtensionLibraryCachePath);
            ExtensionInfo = JsonConvert.DeserializeObject<CExtensionLibInfo>(extensionLibraryJson);
        }
    }
}
