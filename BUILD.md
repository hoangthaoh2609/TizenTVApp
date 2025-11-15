# Build Instructions for Samsung Tizen TV App

This is a .NET 8 MAUI application targeting Samsung Tizen TVs, Android, and iOS.

## Prerequisites

### Required Tools
- **Visual Studio 2022** (17.8 or later) or **Visual Studio for Mac** (latest version)
- **.NET 8 SDK** (8.0.100 or later)
- **Tizen Studio** (for Tizen TV development)

### Required Workloads

Install the following .NET workloads:

```bash
# MAUI workload
dotnet workload install maui

# Tizen workload
dotnet workload install tizen

# Optional: Android workload (if building for Android)
dotnet workload install android

# Optional: iOS workload (if building for iOS on macOS)
dotnet workload install ios
```

### Tizen SDK Setup

1. Download and install [Tizen Studio](https://developer.tizen.org/development/tizen-studio/download)
2. Install the following packages via Tizen Package Manager:
   - Tizen SDK Tools
   - TV Extensions-6.5
   - TV Emulator

3. Set up Tizen development environment:
   ```bash
   # Add Tizen tools to PATH (adjust path based on your installation)
   export PATH=$PATH:~/tizen-studio/tools/ide/bin
   ```

## Building the Project

### For Tizen TV

```bash
# Navigate to the project directory
cd TizenTVApp

# Restore dependencies
dotnet restore

# Build for Tizen
dotnet build -f net8.0-tizen

# Create a Tizen package (.tpk)
dotnet build -f net8.0-tizen -c Release
```

### For Android

```bash
# Build for Android
dotnet build -f net8.0-android

# Create an APK
dotnet publish -f net8.0-android -c Release
```

### For iOS (macOS only)

```bash
# Build for iOS
dotnet build -f net8.0-ios

# Create an IPA
dotnet publish -f net8.0-ios -c Release
```

## Running on Tizen TV

### Using Tizen Emulator

```bash
# Start Tizen TV emulator
tizen emulator start --profile tv

# Install the app
tizen install -n TizenTVApp.tpk -t <target-name>

# Run the app
tizen run -p com.companyname.tizentv -t <target-name>
```

### Using Physical Samsung TV

1. Enable Developer Mode on your Samsung TV:
   - Open Smart Hub
   - Press 1-2-3-4-5 on your remote
   - Enable Developer Mode
   - Enter your PC's IP address

2. Connect to your TV:
   ```bash
   # Connect using SDB (Samsung Debug Bridge)
   sdb connect <TV-IP-ADDRESS>:26101
   
   # Verify connection
   sdb devices
   ```

3. Install and run:
   ```bash
   # Install
   tizen install -n TizenTVApp.tpk -t <TV-NAME>
   
   # Run
   tizen run -p com.companyname.tizentv -t <TV-NAME>
   ```

## Project Structure

```
TizenTVApp/
├── App.xaml / App.xaml.cs          - Application entry point
├── AppShell.xaml / AppShell.xaml.cs - Navigation shell
├── MauiProgram.cs                   - Dependency injection setup
├── Models/                          - Data models
│   ├── VideoContent.cs
│   └── SubtitleTrack.cs
├── ViewModels/                      - MVVM view models
│   ├── MainViewModel.cs
│   ├── BrowseViewModel.cs
│   └── PlayerViewModel.cs
├── Views/                           - XAML views
│   ├── MainPage.xaml / .cs
│   ├── BrowsePage.xaml / .cs
│   └── PlayerPage.xaml / .cs
├── Services/                        - Business logic services
│   ├── VideoPlayerService.cs       - Video playback
│   ├── SubtitleService.cs          - Subtitle parsing
│   ├── ContentService.cs           - Content management
│   ├── RemoteControlService.cs     - TV remote handling
│   └── NavigationService.cs        - Navigation helper
├── Controls/                        - Custom controls
│   └── TVFocusableButton.cs
├── Platforms/Tizen/                 - Tizen-specific code
│   ├── Main.cs                     - Tizen entry point
│   ├── tizen-manifest.xml          - Tizen manifest
│   ├── TizenMediaPlayer.cs         - Media player wrapper
│   └── TizenKeyEventHandler.cs     - Remote control handler
└── Resources/                       - App resources
    ├── Styles/                      - XAML styles
    ├── Fonts/                       - Custom fonts
    └── Images/                      - App images
```

## Features

- **HLS/M3U8 Streaming**: Adaptive streaming support
- **Multiple Subtitle Formats**: SRT, VTT, SSA, ASS
- **TV Remote Control**: Full remote key mapping
- **TV-Optimized UI**: Large fonts, dark theme, focus indicators
- **MVVM Architecture**: Clean separation of concerns
- **Dependency Injection**: Service-based architecture

## Troubleshooting

### Build Errors

**Error: NETSDK1139 - Target platform 'tizen' not recognized**
- Install Tizen workload: `dotnet workload install tizen`
- Ensure Tizen Studio is installed

**Error: Cannot find Tizen.NET package**
- Update package sources to include Tizen NuGet feed
- Run: `dotnet nuget add source https://tizen.myget.org/F/dotnet/api/v3/index.json -n tizen`

### Runtime Errors

**App crashes on launch**
- Check Tizen TV profile is correctly set in manifest
- Verify all required privileges are declared
- Check logs: `sdb dlog TizenTVApp`

**Video playback fails**
- Ensure internet permission is granted
- Verify HLS URL is accessible
- Check supported codec formats for your Tizen version

## Development Tips

1. **Use Tizen TV Emulator** for faster development iteration
2. **Enable Debug Logging** in MauiProgram.cs
3. **Test on Physical Device** before release
4. **Optimize for 10-foot UI** - use large, readable fonts
5. **Handle Remote Control** - all navigation should work via remote

## References

- [.NET MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
- [Tizen .NET Documentation](https://docs.tizen.org/application/dotnet/index)
- [Samsung TV Development Guide](https://developer.samsung.com/smarttv/develop/getting-started/setting-up-sdk.html)
- [Tizen Multimedia API](https://docs.tizen.org/application/dotnet/api/tizenfx/latest/Tizen.Multimedia.html)
