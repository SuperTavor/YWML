using YWML.Src.Loader.Archive.DataClasses;
using YWML.Src.Loader.SevenZip;
using YWML.Src.Loader.Zip;

namespace YWML.Src.Loader.Archive
{
    public static class CArchiveReaderFactory
    {
        private static readonly byte[] ZIP_SIGNATURE = { 0x50, 0x4B, 0x03, 0x04 };
        private static readonly byte[] SEVEN_ZIP_SIGNATURE = { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C };

        public static SArchiveFormat DetectFormat(string path)
        {
            using var stream = File.OpenRead(path);
            var header = new byte[SEVEN_ZIP_SIGNATURE.Length];
            var bytesRead = stream.Read(header, 0, header.Length);

            if (HasSignature(header, ZIP_SIGNATURE, bytesRead))
            {
                return SArchiveFormat.Zip;
            }

            if (HasSignature(header, SEVEN_ZIP_SIGNATURE, bytesRead))
            {
                return SArchiveFormat.SevenZip;
            }

            throw new NotSupportedException("Unsupported mod archive format. Only .zip and .7z archives are supported.");
        }

        public static IArchiveReader Open(string path)
        {
            return DetectFormat(path) switch
            {
                SArchiveFormat.Zip => new CZipArchiveReader(path),
                SArchiveFormat.SevenZip => new C7zArchiveReader(path),
                _ => throw new NotSupportedException("Unsupported mod archive format. Only .zip and .7z archives are supported.")
            };
        }

        private static bool HasSignature(byte[] buffer, byte[] signature, int bytesRead)
        {
            if (bytesRead < signature.Length)
            {
                return false;
            }

            for (int i = 0; i < signature.Length; i++)
            {
                if (buffer[i] != signature[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
