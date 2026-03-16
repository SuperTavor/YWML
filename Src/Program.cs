using System.Text;
using YWML.Src.ConfigManager;
using YWML.Src.Utils.GeneralUtils;
namespace YWML.Src
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
            
            //New user
            if(!Directory.Exists(CGeneralUtils.YWMLDataDir))
            {
                CConfigManager.Cfg.IsUpdateFirstBoot = false;
                CConfigManager.UpdateConfig();
            }
            //Old user who updated
            if(CConfigManager.Cfg.IsUpdateFirstBoot)
            {
                MessageBox.Show("YWML 1.1 introduced a restructure to the extension library, which means it unfortunately has to erase all previously installed extensions from your computer.\nDon't worry, you can reinstall all the extensions from the new and improved extension library!");
                Directory.Delete(CGeneralUtils.YWMLDataDir,true);
                CConfigManager.Cfg.IsUpdateFirstBoot = false;
                CConfigManager.UpdateConfig();
            }

            Directory.CreateDirectory(CGeneralUtils.YWMLDataDir);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.Run(new MainForm());
        }
    }
}