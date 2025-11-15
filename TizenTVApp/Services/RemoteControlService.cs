namespace TizenTVApp.Services;

/// <summary>
/// Service for handling TV remote control input.
/// </summary>
public class RemoteControlService : IRemoteControlService
{
    private bool _isInitialized;

    /// <inheritdoc/>
    public event EventHandler<RemoteKey>? KeyPressed;

    /// <inheritdoc/>
    public void Initialize()
    {
        if (_isInitialized)
            return;

#if TIZEN
        InitializeTizenKeyHandler();
#endif

        _isInitialized = true;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
#if TIZEN
        DisposeTizenKeyHandler();
#endif
        _isInitialized = false;
    }

    /// <summary>
    /// Raises the KeyPressed event.
    /// </summary>
    protected virtual void OnKeyPressed(RemoteKey key)
    {
        KeyPressed?.Invoke(this, key);
    }

#if TIZEN
    private void InitializeTizenKeyHandler()
    {
        // Platform-specific implementation will be in TizenKeyEventHandler.cs
    }

    private void DisposeTizenKeyHandler()
    {
        // Platform-specific cleanup
    }
#endif
}
