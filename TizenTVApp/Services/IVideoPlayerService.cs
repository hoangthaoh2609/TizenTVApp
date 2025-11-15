using TizenTVApp.Models;

namespace TizenTVApp.Services;

/// <summary>
/// Interface for video player service.
/// </summary>
public interface IVideoPlayerService
{
    /// <summary>
    /// Occurs when the player state changes.
    /// </summary>
    event EventHandler<PlayerState>? StateChanged;

    /// <summary>
    /// Occurs when the playback position changes.
    /// </summary>
    event EventHandler<TimeSpan>? PositionChanged;

    /// <summary>
    /// Occurs when an error occurs.
    /// </summary>
    event EventHandler<string>? ErrorOccurred;

    /// <summary>
    /// Gets the current player state.
    /// </summary>
    PlayerState State { get; }

    /// <summary>
    /// Gets the current playback position.
    /// </summary>
    TimeSpan Position { get; }

    /// <summary>
    /// Gets the video duration.
    /// </summary>
    TimeSpan Duration { get; }

    /// <summary>
    /// Gets or sets the playback volume (0.0 to 1.0).
    /// </summary>
    double Volume { get; set; }

    /// <summary>
    /// Prepares the player with video content.
    /// </summary>
    Task PrepareAsync(VideoContent content);

    /// <summary>
    /// Starts or resumes playback.
    /// </summary>
    Task PlayAsync();

    /// <summary>
    /// Pauses playback.
    /// </summary>
    Task PauseAsync();

    /// <summary>
    /// Stops playback.
    /// </summary>
    Task StopAsync();

    /// <summary>
    /// Seeks to the specified position.
    /// </summary>
    Task SeekAsync(TimeSpan position);

    /// <summary>
    /// Sets the subtitle track.
    /// </summary>
    Task SetSubtitleTrackAsync(SubtitleTrack? track);

    /// <summary>
    /// Releases player resources.
    /// </summary>
    void Dispose();
}

/// <summary>
/// Represents the player state.
/// </summary>
public enum PlayerState
{
    /// <summary>
    /// Player is idle.
    /// </summary>
    Idle,

    /// <summary>
    /// Player is preparing.
    /// </summary>
    Preparing,

    /// <summary>
    /// Player is ready.
    /// </summary>
    Ready,

    /// <summary>
    /// Player is playing.
    /// </summary>
    Playing,

    /// <summary>
    /// Player is paused.
    /// </summary>
    Paused,

    /// <summary>
    /// Player has stopped.
    /// </summary>
    Stopped,

    /// <summary>
    /// Player encountered an error.
    /// </summary>
    Error
}
