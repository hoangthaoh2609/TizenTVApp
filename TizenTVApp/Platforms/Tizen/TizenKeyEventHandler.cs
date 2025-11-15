#if TIZEN
using System;
using Tizen.NUI;
using TizenTVApp.Services;

namespace TizenTVApp.Platforms.Tizen;

/// <summary>
/// Tizen-specific key event handler for TV remote control.
/// </summary>
public class TizenKeyEventHandler : IDisposable
{
    private Window? _window;
    private bool _isDisposed;

    /// <summary>
    /// Occurs when a remote key is pressed.
    /// </summary>
    public event EventHandler<RemoteKey>? KeyPressed;

    /// <summary>
    /// Initializes the key event handler.
    /// </summary>
    public void Initialize()
    {
        try
        {
            _window = Window.Instance;
            if (_window != null)
            {
                _window.KeyEvent += OnKeyEvent;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to initialize key handler: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles key events from the Tizen window.
    /// </summary>
    private void OnKeyEvent(object? sender, Window.KeyEventArgs e)
    {
        if (e.Key.State != Key.StateType.Down)
            return;

        var remoteKey = MapTizenKeyToRemoteKey(e.Key.KeyPressedName);
        if (remoteKey != RemoteKey.Unknown)
        {
            KeyPressed?.Invoke(this, remoteKey);
        }
    }

    /// <summary>
    /// Maps Tizen key names to RemoteKey enum values.
    /// </summary>
    private RemoteKey MapTizenKeyToRemoteKey(string keyName)
    {
        return keyName switch
        {
            // Navigation
            "Up" => RemoteKey.Up,
            "Down" => RemoteKey.Down,
            "Left" => RemoteKey.Left,
            "Right" => RemoteKey.Right,
            "Return" or "Select" => RemoteKey.Enter,
            "Back" or "XF86Back" => RemoteKey.Back,

            // Playback
            "XF86PlayBack" or "XF86Play" => RemoteKey.Play,
            "XF86AudioPause" or "Pause" => RemoteKey.Pause,
            "XF86AudioStop" or "Stop" => RemoteKey.Stop,
            "XF86PlayPause" => RemoteKey.PlayPause,
            "XF86AudioRewind" or "Rewind" => RemoteKey.Rewind,
            "XF86AudioForward" or "FastForward" => RemoteKey.FastForward,

            // Volume
            "XF86AudioRaiseVolume" or "VolumeUp" => RemoteKey.VolumeUp,
            "XF86AudioLowerVolume" or "VolumeDown" => RemoteKey.VolumeDown,
            "XF86AudioMute" or "Mute" => RemoteKey.Mute,

            // Channel
            "ChannelUp" or "XF86ChannelUp" => RemoteKey.ChannelUp,
            "ChannelDown" or "XF86ChannelDown" => RemoteKey.ChannelDown,

            // Number pad
            "0" => RemoteKey.Number0,
            "1" => RemoteKey.Number1,
            "2" => RemoteKey.Number2,
            "3" => RemoteKey.Number3,
            "4" => RemoteKey.Number4,
            "5" => RemoteKey.Number5,
            "6" => RemoteKey.Number6,
            "7" => RemoteKey.Number7,
            "8" => RemoteKey.Number8,
            "9" => RemoteKey.Number9,

            // Color buttons
            "Red" or "XF86Red" => RemoteKey.Red,
            "Green" or "XF86Green" => RemoteKey.Green,
            "Yellow" or "XF86Yellow" => RemoteKey.Yellow,
            "Blue" or "XF86Blue" => RemoteKey.Blue,

            // Special
            "Menu" or "XF86Menu" => RemoteKey.Menu,
            "Home" or "XF86Home" => RemoteKey.Home,
            "Exit" or "XF86Exit" => RemoteKey.Exit,
            "Info" or "XF86Info" => RemoteKey.Info,
            "Guide" or "XF86Guide" => RemoteKey.Guide,

            _ => RemoteKey.Unknown
        };
    }

    /// <summary>
    /// Releases resources.
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed)
            return;

        try
        {
            if (_window != null)
            {
                _window.KeyEvent -= OnKeyEvent;
                _window = null;
            }

            _isDisposed = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error disposing key handler: {ex.Message}");
        }
    }
}
#endif
