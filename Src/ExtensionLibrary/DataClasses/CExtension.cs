using Newtonsoft.Json;
using YWML.Src.Utils.GeneralUtils;
using static YWML.Src.ExtensionLibrary.CExtensionLibrary;
namespace YWML.Src.ExtensionLibrary.DataClasses
{
    public class CExtension
    {
        public string Name { get; set;  }
        //in mb
        public int FileSize { get; set;  }

        //Download link for the LZMA extension 
        public string Link { get; set; }
        public string Id { get; set; }
        //Original FA name
        public string OgFAName { get; set;  }

        public async Task UninstallAsync(List<Button> btnsToLock,Label statusLabel,Dictionary<string,SExtensionLibraryItem> installedList)
        {
            foreach (var button in btnsToLock)
            {
                button.Enabled = false;
            }
            installedList.Remove(Id);
            var installDir = Path.Combine(CGeneralUtils.ExtensionInstallDirectory, Id);
            Directory.Delete(installDir, true);
            statusLabel.Text = "Finished uninstalling extension";
            foreach (var button in btnsToLock)
            {
                button.Enabled = true;
            }
        }
        public async Task InstallAsync(List<Button> btnsToLock, Label statusLabel,Label percentageLabel,Dictionary<string, SExtensionLibraryItem> installedList)
        {
            foreach(var button in btnsToLock)
            {
                button.Enabled = false;
            }
            statusLabel.Text = "Downloading LZMA extension";
            var compressedPath = Path.Combine(CGeneralUtils.TmpDirectory, "compressed.7z");
            Directory.CreateDirectory(CGeneralUtils.TmpDirectory);
            //Download compressed FA
            await DownloadCompressedFAAsync(compressedPath,percentageLabel);
            //Unpack it
            percentageLabel.Text = "";
            statusLabel.Text = "Unpacking LZMA extension";
            var decompressedBytes = await DecompressAsync(await File.ReadAllBytesAsync(compressedPath));
            //Write decompressed FA to disk
            statusLabel.Text = "Installing unpacked extension";
            var unpackedFaPath = Path.Combine(Path.Combine(CGeneralUtils.ExtensionInstallDirectory, Id), "patchable.fa");
            Directory.CreateDirectory(Path.GetDirectoryName(unpackedFaPath)!);
            await File.WriteAllBytesAsync(unpackedFaPath, decompressedBytes);
            Directory.Delete(CGeneralUtils.TmpDirectory, true);

          
            if(!installedList.Keys.Contains(this.Id))
            {
                installedList[this.Id] = new()
                {
                   FAName= this.OgFAName,
                   Name = this.Name
                };
            }
            
            //Add to installed list
            statusLabel.Text = "Finished installing extension!";
            foreach (var button in btnsToLock)
            {
                button.Enabled = true;
            }
        }

        private async Task DownloadCompressedFAAsync(string compressedPath,Label percentageLabel)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(Link, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? 0;
            var downloadedBytes = 0L;

            using (var fs = new FileStream(compressedPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                using (var contentStream = await response.Content.ReadAsStreamAsync())
                {
                    var buffer = new byte[8192];
                    int bytesRead;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fs.WriteAsync(buffer, 0, bytesRead);
                        downloadedBytes += bytesRead;

                        var percentage = (int)((downloadedBytes * 100) / totalBytes);

                        percentageLabel.Invoke(new Action(() => percentageLabel.Text = $"{percentage}%"));
                    }
                }
            }
        }

        public async Task<byte[]> DecompressAsync(byte[] toDecompress)
        {
            var decompressed = await Task.Run(() => LZMA.Engine.Decompress(toDecompress));
            return decompressed;
        }

    }
}
