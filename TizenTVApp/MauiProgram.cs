using Microsoft.Extensions.Logging;
using TizenTVApp.Services;
using TizenTVApp.ViewModels;
using TizenTVApp.Views;

namespace TizenTVApp;

/// <summary>
/// Configures the MAUI application with dependency injection.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates and configures the MAUI application.
    /// </summary>
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

        // Register Services
        builder.Services.AddSingleton<IVideoPlayerService, VideoPlayerService>();
        builder.Services.AddSingleton<ISubtitleService, SubtitleService>();
        builder.Services.AddSingleton<IContentService, ContentService>();
        builder.Services.AddSingleton<IRemoteControlService, RemoteControlService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // Register ViewModels
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<BrowseViewModel>();
        builder.Services.AddTransient<PlayerViewModel>();

        // Register Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<BrowsePage>();
        builder.Services.AddTransient<PlayerPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
