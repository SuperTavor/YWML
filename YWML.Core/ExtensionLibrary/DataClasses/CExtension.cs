using YWML.Src.Net;
using YWML.Src.Utils.GeneralUtils;

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

        //Title ID used on the console for the game to generate the mod folder
        public string TitleId { get; set; }

        //Original FA name
        public string OgFAName { get; set;  }

        //OPTIONAL: Decides if auto-install should be disabled (for Switch games that use FA) (Fuck you Light)
        public bool IsDisableAutoInstall { get; set; }

        public Task UninstallAsync(Dictionary<string,CInstalledExtensionMetadata> installedList)
        {
            installedList.Remove(Id);
            var installDir = Path.Combine(CGeneralUtils.ExtensionInstallDirectory, Id);
            Directory.Delete(installDir, true);
            return Task.CompletedTask;
        }

        public async Task InstallAsync(IProgress<string> status, IProgress<int> percent, Dictionary<string, CInstalledExtensionMetadata> installedList, HttpMessageHandler? downloadHandler = null)
        {
            status.Report("Downloading LZMA extension");
            var compressedPath = Path.Combine(CGeneralUtils.TmpDirectory, "compressed.7z");
            Directory.CreateDirectory(CGeneralUtils.TmpDirectory);
            //Network I/O must not run on the UI thread on Android.
            await Task.Run(() => CFileDownloader.DownloadAsync(Link, compressedPath, percent, downloadHandler));

            status.Report("Unpacking LZMA extension");
            var unpackedFaPath = Path.Combine(Path.Combine(CGeneralUtils.ExtensionInstallDirectory, Id), "patchable.fa");
            Directory.CreateDirectory(Path.GetDirectoryName(unpackedFaPath)!);
            await Task.Run(() => DecompressLzmaAlone(compressedPath, unpackedFaPath, percent));

            Directory.Delete(CGeneralUtils.TmpDirectory, true);

            if(!installedList.Keys.Contains(this.Id))
            {
                installedList[this.Id] = new()
                {
                   FAName= this.OgFAName,
                   Name = this.Name,
                   TitleId = this.TitleId,
                   IsDisableAutoInstall = this.IsDisableAutoInstall
                };
            }

            status.Report("Finished installing extension!");
        }

        //Streams an LZMA-Alone stream (5-byte properties + 8-byte size + data) straight to disk.
        internal static void DecompressLzmaAlone(string compressedPath, string outputPath, IProgress<int>? percent = null)
        {
            using var input = File.OpenRead(compressedPath);

            Span<byte> header = stackalloc byte[13];
            input.ReadExactly(header);

            var properties = header[..5].ToArray();
            var uncompressedSize = BitConverter.ToInt64(header[5..]);

            if (uncompressedSize < 0)
            {
                throw new InvalidDataException("Unsupported LZMA stream (unknown size).");
            }

            var decoder = new SharpCompress.Compressors.LZMA.Decoder();
            decoder.SetDecoderProperties(properties);

            using var output = File.Create(outputPath);
            using var counting = new CCountingReadStream(input, input.Length - 13, percent);
            decoder.Code(counting, output, input.Length - 13, uncompressedSize, null!);
            percent?.Report(100);
        }

        //SharpCompress's LZMA decoder ignores the ICodeProgress callback, so progress is tracked
        //by counting the compressed bytes it reads from the input stream instead.
        private sealed class CCountingReadStream : Stream
        {
            private readonly Stream _inner;
            private readonly long _total;
            private readonly IProgress<int>? _percent;
            private long _read;
            private int _lastReported = -1;

            public CCountingReadStream(Stream inner, long total, IProgress<int>? percent)
            {
                _inner = inner;
                _total = total;
                _percent = percent;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                var read = _inner.Read(buffer, offset, count);
                Report(read);
                return read;
            }

            public override int ReadByte()
            {
                var value = _inner.ReadByte();
                if (value >= 0)
                {
                    Report(1);
                }
                return value;
            }

            private void Report(int count)
            {
                if (count <= 0)
                {
                    return;
                }

                _read += count;
                if (_percent == null || _total <= 0)
                {
                    return;
                }

                var value = (int)Math.Min(100, _read * 100 / _total);
                if (value == _lastReported)
                {
                    return;
                }

                _lastReported = value;
                _percent.Report(value);
            }

            public override bool CanRead => true;
            public override bool CanSeek => _inner.CanSeek;
            public override bool CanWrite => false;
            public override long Length => _inner.Length;
            public override long Position { get => _inner.Position; set => _inner.Position = value; }
            public override void Flush() => _inner.Flush();
            public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }

    }
}
