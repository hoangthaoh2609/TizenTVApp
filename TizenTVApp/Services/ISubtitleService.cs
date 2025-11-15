using TizenTVApp.Models;

namespace TizenTVApp.Services;

/// <summary>
/// Interface for subtitle service.
/// </summary>
public interface ISubtitleService
{
    /// <summary>
    /// Loads a subtitle track.
    /// </summary>
    Task LoadSubtitleAsync(SubtitleTrack track);

    /// <summary>
    /// Gets the current subtitle cue for the given time.
    /// </summary>
    SubtitleCue? GetCurrentCue(TimeSpan position);

    /// <summary>
    /// Clears the currently loaded subtitle.
    /// </summary>
    void ClearSubtitle();
}
