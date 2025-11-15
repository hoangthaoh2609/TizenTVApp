namespace TizenTVApp.Models;

/// <summary>
/// Represents video content with streaming information.
/// </summary>
public class VideoContent
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the video title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the video description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the thumbnail URL.
    /// </summary>
    public string ThumbnailUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the video URL (can be HLS, DASH, or direct URL).
    /// </summary>
    public string VideoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the streaming protocol.
    /// </summary>
    public StreamingProtocol Protocol { get; set; } = StreamingProtocol.Direct;

    /// <summary>
    /// Gets or sets the duration in seconds.
    /// </summary>
    public int DurationSeconds { get; set; }

    /// <summary>
    /// Gets or sets the video category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content rating.
    /// </summary>
    public string Rating { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release year.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the available subtitle tracks.
    /// </summary>
    public List<SubtitleTrack> Subtitles { get; set; } = new();

    /// <summary>
    /// Gets the formatted duration string.
    /// </summary>
    public string FormattedDuration => TimeSpan.FromSeconds(DurationSeconds).ToString(@"hh\:mm\:ss");
}

/// <summary>
/// Represents the streaming protocol used for video content.
/// </summary>
public enum StreamingProtocol
{
    /// <summary>
    /// Direct video file URL.
    /// </summary>
    Direct,

    /// <summary>
    /// HLS (HTTP Live Streaming) - .m3u8 manifest.
    /// </summary>
    HLS,

    /// <summary>
    /// DASH (Dynamic Adaptive Streaming over HTTP).
    /// </summary>
    DASH,

    /// <summary>
    /// Smooth Streaming.
    /// </summary>
    SmoothStreaming
}
