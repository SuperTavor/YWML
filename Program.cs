using System.Text;
using YWML.Src.ConfigManager;

namespace YWML
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            CConfigManager.Initialize();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.Run(new MainForm());
        }
    }
}