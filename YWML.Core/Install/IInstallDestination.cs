namespace YWML.Src.Install
{
    public interface IInstallDestination
    {
        Stream OpenWrite(string relativePath);
    }
}
