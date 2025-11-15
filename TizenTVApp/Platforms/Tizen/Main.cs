using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TizenTVApp;

/// <summary>
/// Tizen application entry point.
/// </summary>
class Program : MauiApplication
{
    /// <summary>
    /// Creates the MAUI app.
    /// </summary>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    /// <summary>
    /// Main entry point.
    /// </summary>
    static void Main(string[] args)
    {
        var app = new Program();
        app.Run(args);
    }
}
