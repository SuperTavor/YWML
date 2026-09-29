using YWML.Src.Updates.DataClasses;

namespace YWML.Src.Updates
{
    public interface IUpdatePlatform
    {
        SUpdatePlatform Platform { get; }

        bool IsConnected { get; }

        Task<SUpdateChoice> AskAsync(SUpdateInfo update);

        string GetDownloadPath(string assetName);

        IProgress<int>? BeginDownload(SUpdateInfo update);

        void EndDownload();

        void ReportError(string message);

        Task InstallAsync(SUpdateInfo update, string downloadedFile);
    }
}
