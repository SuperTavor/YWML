namespace YWML.Src.RemoteInstall.DataClasses
{
    public struct SFtpConnectionInfo
    {
        public const int DEFAULT_PORT = 5000;

        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public SFtpConnectionInfo()
        {
            Host = "";
            Port = DEFAULT_PORT;
            UserName = "";
            Password = "";
        }
    }
}
