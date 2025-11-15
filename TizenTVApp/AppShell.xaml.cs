using TizenTVApp.Views;

namespace TizenTVApp;

/// <summary>
/// Application shell that manages navigation and routing.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Initializes a new instance of the AppShell class.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();

        // Register routes
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(BrowsePage), typeof(BrowsePage));
        Routing.RegisterRoute(nameof(PlayerPage), typeof(PlayerPage));
    }
}
