# Samsung Tizen TV App

A production-ready .NET 8 MAUI application for Samsung Tizen Smart TVs with support for HLS streaming, multiple subtitle formats, and TV remote control integration.

## Features

- **📺 TV-Optimized UI**: Large fonts (42-48px), dark theme, focus management for 10-foot interface
- **🎬 HLS/M3U8 Streaming**: Full adaptive streaming support with quality switching
- **📝 Subtitle Support**: Complete parser for SRT, VTT, WebVTT, SSA, ASS formats
- **🎮 TV Remote Control**: Full Samsung TV remote key mapping (navigation, playback, volume)
- **🏗️ MVVM Architecture**: Clean separation with CommunityToolkit.Mvvm
- **💉 Dependency Injection**: All services registered and injectable
- **🔒 Type Safety**: C# 12 with nullable reference types enabled

## Technology Stack

- **.NET 8**: Latest C# 12 features
- **MAUI 8.0.90**: Cross-platform UI framework
- **CommunityToolkit.Mvvm 8.3.2**: MVVM helpers
- **Tizen.NET 12.0**: Tizen platform APIs
- **Target Platforms**: Tizen 6.5+, Android, iOS

## Quick Start

See [BUILD.md](BUILD.md) for detailed build and deployment instructions.

### Prerequisites

- Visual Studio 2022 (17.8+) or VS for Mac
- .NET 8 SDK
- Tizen Studio (for TV development)

### Build

```bash
# Install workloads
dotnet workload install maui tizen

# Restore packages
dotnet restore

# Build for Tizen TV
dotnet build -f net8.0-tizen
```

## Project Structure

```
TizenTVApp/
├── Models/            - Data models (VideoContent, SubtitleTrack)
├── ViewModels/        - MVVM view models with CommunityToolkit
├── Views/             - XAML pages (Main, Browse, Player)
├── Services/          - Business logic (Video, Subtitle, Content, Navigation)
├── Controls/          - Custom TV controls (TVFocusableButton)
├── Platforms/Tizen/   - Tizen-specific implementations
└── Resources/         - Styles, fonts, images
```

## Key Components

### Video Player
- Tizen MediaPlayer integration
- HLS manifest parsing
- Buffering and quality switching
- Real-time position tracking

### Subtitle Engine
- Multi-format parser (SRT, VTT, SSA, ASS)
- Time-synchronized display
- Style support (fonts, colors, positioning)
- HTTP/embedded protocols

### Remote Control
- Complete key mapping for Samsung TV remote
- Navigation (Up, Down, Left, Right, Enter, Back)
- Playback (Play, Pause, Stop, FF, Rewind)
- Volume and channel controls
- Color and number buttons

### Content Service
- Mock content with real HLS test streams
- Category-based browsing
- Search functionality
- Metadata (title, description, duration, year)

## Sample Content

The app includes several open-source test streams:
- Big Buck Bunny (634s, HLS)
- Sintel (888s, HLS)
- Tears of Steel (734s, HLS)
- Elephants Dream (654s, HLS)
- Cosmos Laundromat (734s, HLS)

All streams use public HLS endpoints for testing adaptive streaming.

## Development

### Adding New Content

Edit `Services/ContentService.cs`:

```csharp
new VideoContent
{
    Id = "11",
    Title = "My Video",
    VideoUrl = "https://example.com/video.m3u8",
    Protocol = StreamingProtocol.HLS,
    Category = "Entertainment",
    Subtitles = new List<SubtitleTrack> { ... }
}
```

### Implementing Custom Styles

Edit `Resources/Styles/Styles.xaml` for TV-specific styling:
- Large touch targets (80px height)
- High contrast colors
- Focus indicators (4px borders)
- Animation timing (150ms)

### TV Remote Handling

The `RemoteControlService` maps Tizen keys to `RemoteKey` enum values. Handle them in ViewModels:

```csharp
remoteControlService.KeyPressed += (s, key) =>
{
    switch (key)
    {
        case RemoteKey.Play: await PlayAsync(); break;
        case RemoteKey.Pause: await PauseAsync(); break;
        // ... more handlers
    }
};
```

## Testing

### Tizen Emulator
```bash
tizen emulator start --profile tv
sdb devices
tizen install -n TizenTVApp.tpk
```

### Physical Samsung TV
1. Enable Developer Mode (Press 1-2-3-4-5 in Smart Hub)
2. Connect: `sdb connect <TV-IP>:26101`
3. Install: `tizen install -n TizenTVApp.tpk`

## License

This project is provided as-is for demonstration and educational purposes.

## Contributing

Contributions welcome! Please follow:
- Clean code principles
- MVVM pattern
- XML documentation
- Null safety
- Async/await best practices

## Support

For issues, questions, or contributions, please use GitHub Issues.