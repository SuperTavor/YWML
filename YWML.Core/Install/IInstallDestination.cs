namespace YWML.Src.Install
{
    public interface IInstallDestination
    {
        Stream OpenWrite(string relativePath);

        //The scope one level above the mod root, where ExeFS files (code.ips / code.bin) go.
        IInstallDestination Parent { get; }
    }
}
