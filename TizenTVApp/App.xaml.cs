using TizenTVApp.Services;

namespace TizenTVApp;

/// <summary>
/// Main application class that initializes the app and services.
/// </summary>
public partial class App : Application
{
    private readonly IRemoteControlService _remoteControlService;

    /// <summary>
    /// Initializes a new instance of the App class.
    /// </summary>
    public App(IRemoteControlService remoteControlService)
    {
        InitializeComponent();

        _remoteControlService = remoteControlService;
        
        MainPage = new AppShell();

        // Initialize remote control service
        _remoteControlService.Initialize();
    }

    /// <summary>
    /// Called when the application starts.
    /// </summary>
    protected override void OnStart()
    {
        base.OnStart();
    }

    /// <summary>
    /// Called when the application sleeps.
    /// </summary>
    protected override void OnSleep()
    {
        base.OnSleep();
    }

    /// <summary>
    /// Called when the application resumes.
    /// </summary>
    protected override void OnResume()
    {
        base.OnResume();
    }
}
