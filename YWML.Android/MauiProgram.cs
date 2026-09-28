using System.Text;
using Microsoft.Extensions.Logging;
using YWML.Android.Services;
using YWML.Src.ConfigManager;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Android;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        CGeneralUtils.Initialize(FileSystem.AppDataDirectory);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        try
        {
            CConfigManager.Initialize();
        }
        catch (Exception ex)
        {
            CAppState.Current.StartupError = ex.Message;
        }

        return builder.Build();
    }
}
