using Microsoft.Extensions.Logging;
using System.Net;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace iService3;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        //builder.Services.AddHttpClient("api", httpClient => httpClient.BaseAddress = new Uri("https://localhost:7169/api");
        
        return builder.Build();
    }
}