# Architecture Documentation

## Overview

This Samsung Tizen TV App follows a clean MVVM (Model-View-ViewModel) architecture with dependency injection, ensuring maintainability, testability, and scalability.

## Architecture Layers

### 1. Presentation Layer (Views + ViewModels)

#### Views
XAML-based UI pages optimized for TV (10-foot interface):
- **MainPage**: Home screen with featured content and categories
- **BrowsePage**: Category-based content browser with search
- **PlayerPage**: Full-screen video player with controls and subtitles

#### ViewModels
Observable objects using CommunityToolkit.Mvvm:
- **MainViewModel**: Manages home screen state and navigation
- **BrowseViewModel**: Handles content browsing and search
- **PlayerViewModel**: Controls video playback and subtitle display

Key MVVM Features:
- `[ObservableProperty]` for automatic INotifyPropertyChanged
- `[RelayCommand]` for command binding
- `IQueryAttributable` for navigation parameters
- Event-driven updates from services

### 2. Business Logic Layer (Services)

#### Core Services

**IVideoPlayerService / VideoPlayerService**
- Abstracts platform-specific player implementation
- Manages playback state (Idle, Playing, Paused, etc.)
- Position tracking and seeking
- Volume control
- Subtitle track management

**ISubtitleService / SubtitleService**
- Multi-format parsing (SRT, VTT, SSA, ASS)
- Time-synchronized cue retrieval
- Support for styling and positioning
- HTTP subtitle loading

**IContentService / ContentService**
- Content catalog management
- Category-based filtering
- Search functionality
- Mock data with real HLS streams

**IRemoteControlService / RemoteControlService**
- TV remote key event handling
- Platform abstraction for key mapping
- Event-driven key notification

**INavigationService / NavigationService**
- Shell-based navigation wrapper
- Parameter passing between pages
- Back navigation support

### 3. Platform Layer (Tizen-Specific)

#### TizenMediaPlayer
- Wraps Tizen.Multimedia.Player
- Handles HLS stream playback
- Manages player lifecycle (Prepare, Play, Pause, Stop)
- Event forwarding (state changes, position updates, errors)

#### TizenKeyEventHandler
- Subscribes to Tizen.NUI.Window key events
- Maps Tizen key names to RemoteKey enum
- Supports all Samsung TV remote buttons

#### Main.cs
- Tizen application entry point
- Initializes MAUI app

#### tizen-manifest.xml
- Declares TV profile
- Defines app permissions (internet, network, media controller)
- Sets app metadata and launch mode

### 4. Data Layer (Models)

#### VideoContent
- Video metadata (title, description, URL, duration)
- Streaming protocol (HLS, DASH, Direct)
- Category and rating
- Associated subtitle tracks

#### SubtitleTrack & SubtitleCue
- Track metadata (language, format, URL)
- Parsed cues with timing
- Style information (for SSA/ASS)

## Design Patterns

### 1. Dependency Injection
All services registered in `MauiProgram.cs`:
```csharp
builder.Services.AddSingleton<IVideoPlayerService, VideoPlayerService>();
builder.Services.AddTransient<MainViewModel>();
```

Benefits:
- Loose coupling
- Easy testing with mocks
- Centralized configuration

### 2. MVVM (Model-View-ViewModel)
- Views contain only UI logic
- ViewModels handle presentation logic
- Models represent data
- Commands for user actions
- Two-way data binding

### 3. Service Layer Pattern
- Business logic isolated in services
- Platform-agnostic interfaces
- Platform-specific implementations in Platforms/ folder

### 4. Observer Pattern
- Services raise events (StateChanged, PositionChanged)
- ViewModels subscribe and update UI
- Loose coupling between components

### 5. Strategy Pattern
- Different subtitle parsers based on format
- Different streaming protocols (HLS, DASH)

## Data Flow

### Video Playback Flow
```
User taps video
  → MainViewModel.PlayVideoCommand
  → NavigationService.NavigateToAsync(PlayerPage)
  → PlayerPage created with PlayerViewModel
  → PlayerViewModel.ApplyQueryAttributes(VideoContent)
  → VideoPlayerService.PrepareAsync(content)
  → TizenMediaPlayer.PrepareAsync (on Tizen)
  → VideoPlayerService.PlayAsync()
  → TizenMediaPlayer.PlayAsync
  → Position updates → ViewModel → View
```

### Subtitle Display Flow
```
VideoPlayerService.SetSubtitleTrackAsync(track)
  → SubtitleService.LoadSubtitleAsync(track)
  → HTTP download of subtitle file
  → Parse based on format (SRT/VTT/SSA/ASS)
  → Store parsed cues
  → On each position update:
    → SubtitleService.GetCurrentCue(position)
    → Return matching cue → ViewModel → View
```

### Remote Control Flow
```
User presses remote button
  → Tizen key event → TizenKeyEventHandler
  → Map to RemoteKey enum
  → RemoteControlService.KeyPressed event
  → ViewModel handles key
  → Execute appropriate command
```

## Threading Model

### UI Thread
- All XAML views
- Property change notifications
- Command execution

### Background Threads
- HTTP downloads (subtitles, content)
- Subtitle parsing
- Video preparation (async/await)

### Tizen Player Thread
- Managed by Tizen.Multimedia.Player
- Events marshaled to UI thread

## Error Handling

### Patterns Used
1. **Try-Catch with User Feedback**
   - Services catch exceptions
   - Raise ErrorOccurred events
   - ViewModels display user-friendly messages

2. **Async Exception Propagation**
   - Await all async calls
   - Handle Task exceptions
   - Log errors for debugging

3. **State Management**
   - PlayerState.Error for player failures
   - IsLoading flags for UI feedback
   - Graceful degradation (missing subtitles)

## Performance Optimizations

### 1. Lazy Loading
- ViewModels created on demand
- Services initialized when first used

### 2. Async/Await
- Non-blocking UI operations
- Smooth playback and navigation

### 3. Resource Management
- IDisposable for player cleanup
- Event unsubscription
- Proper lifecycle management

### 4. TV-Specific Optimizations
- Large touch targets (80px height)
- Preload next screen during navigation
- Minimize reflows with fixed layouts

## Testing Strategy

### Unit Tests (Recommended)
- ViewModel logic (commands, properties)
- Service methods (parsing, filtering)
- Model validation

### Integration Tests (Recommended)
- Service interactions
- Navigation flows
- Player lifecycle

### UI Tests (Recommended)
- Page navigation
- User interactions
- Focus management

### Manual Testing (Required)
- Physical TV testing
- Remote control handling
- Video playback quality
- Subtitle synchronization

## Security Considerations

### 1. Content Security
- HTTPS for all video URLs
- Validate HLS manifests
- Sanitize subtitle text (prevent injection)

### 2. Permission Management
- Minimal permissions in manifest
- Network access for streaming
- Media controller for playback control

### 3. Input Validation
- Validate remote key inputs
- Sanitize search queries
- Validate URLs before playback

## Scalability

### Adding New Features

**New Video Source**
1. Implement IContentService method
2. Add VideoContent entries
3. Support new StreamingProtocol if needed

**New Subtitle Format**
1. Add SubtitleFormat enum value
2. Implement parser method in SubtitleService
3. Handle in switch statement

**New Remote Key**
1. Add RemoteKey enum value
2. Map in TizenKeyEventHandler
3. Handle in ViewModel

**New Page**
1. Create View and ViewModel
2. Register in MauiProgram.cs
3. Add route in AppShell.xaml.cs
4. Navigate using NavigationService

## Deployment

### Debug Build
- Includes debug symbols
- Verbose logging enabled
- No optimizations

### Release Build
- AOT compilation for performance
- Optimized assets
- Minimal logging
- Code obfuscation (recommended)

### Tizen Package (.tpk)
- Built using `dotnet build -c Release`
- Includes manifest, resources, assemblies
- Signed with certificate
- Installable via SDB or Tizen Store

## Maintenance

### Code Quality
- XML documentation on all public members
- Consistent naming conventions
- StyleCop/analyzer rules (recommended)
- Regular dependency updates

### Monitoring
- Log all exceptions
- Track playback errors
- Monitor network failures
- User analytics (optional)

## Future Enhancements

### Potential Improvements
1. **Offline Playback**: Download videos for later viewing
2. **User Accounts**: Personalization and watch history
3. **Recommendations**: AI-powered content suggestions
4. **Live Streaming**: Support for live TV channels
5. **DVR Features**: Record and playback
6. **Multi-Audio**: Multiple audio tracks
7. **Picture-in-Picture**: Watch while browsing
8. **Voice Control**: Integration with Bixby
9. **Gesture Control**: Support for hand gestures
10. **Chromecast**: Cast to other devices

### Platform Extensions
- Android TV optimization
- Apple TV support (tvOS)
- Web version (Blazor)
- Windows desktop app

## References

- [.NET MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
- [MVVM Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [Tizen .NET API](https://docs.tizen.org/application/dotnet/api/overview/)
- [HLS Specification](https://datatracker.ietf.org/doc/html/rfc8216)
- [WebVTT Specification](https://www.w3.org/TR/webvtt1/)
