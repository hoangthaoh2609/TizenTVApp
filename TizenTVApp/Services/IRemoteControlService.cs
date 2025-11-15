namespace TizenTVApp.Services;

/// <summary>
/// Interface for TV remote control service.
/// </summary>
public interface IRemoteControlService
{
    /// <summary>
    /// Occurs when a remote control key is pressed.
    /// </summary>
    event EventHandler<RemoteKey>? KeyPressed;

    /// <summary>
    /// Initializes the remote control service.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Releases resources.
    /// </summary>
    void Dispose();
}

/// <summary>
/// Represents TV remote control keys.
/// </summary>
public enum RemoteKey
{
    // Navigation
    Up,
    Down,
    Left,
    Right,
    Enter,
    Back,

    // Playback
    Play,
    Pause,
    Stop,
    PlayPause,
    Rewind,
    FastForward,

    // Volume
    VolumeUp,
    VolumeDown,
    Mute,

    // Channel
    ChannelUp,
    ChannelDown,

    // Number pad
    Number0,
    Number1,
    Number2,
    Number3,
    Number4,
    Number5,
    Number6,
    Number7,
    Number8,
    Number9,

    // Color buttons
    Red,
    Green,
    Yellow,
    Blue,

    // Special
    Menu,
    Home,
    Exit,
    Info,
    Guide,

    // Additional
    Unknown
}
