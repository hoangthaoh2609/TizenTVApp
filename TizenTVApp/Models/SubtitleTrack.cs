namespace TizenTVApp.Models;

/// <summary>
/// Represents a subtitle track.
/// </summary>
public class SubtitleTrack
{
    /// <summary>
    /// Gets or sets the track identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the language code (e.g., "en", "es", "fr").
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display label.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subtitle URL.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subtitle format.
    /// </summary>
    public SubtitleFormat Format { get; set; } = SubtitleFormat.SRT;

    /// <summary>
    /// Gets or sets the subtitle protocol.
    /// </summary>
    public SubtitleProtocol Protocol { get; set; } = SubtitleProtocol.Http;

    /// <summary>
    /// Gets or sets the parsed subtitle cues.
    /// </summary>
    public List<SubtitleCue> Cues { get; set; } = new();
}

/// <summary>
/// Represents a subtitle cue (a single subtitle entry).
/// </summary>
public class SubtitleCue
{
    /// <summary>
    /// Gets or sets the cue index.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the start time.
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time.
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Gets or sets the subtitle text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional styling information for advanced formats (SSA/ASS).
    /// </summary>
    public SubtitleStyle? Style { get; set; }
}

/// <summary>
/// Represents subtitle styling information.
/// </summary>
public class SubtitleStyle
{
    /// <summary>
    /// Gets or sets the font name.
    /// </summary>
    public string? FontName { get; set; }

    /// <summary>
    /// Gets or sets the font size.
    /// </summary>
    public int? FontSize { get; set; }

    /// <summary>
    /// Gets or sets the primary color (hex format).
    /// </summary>
    public string? PrimaryColor { get; set; }

    /// <summary>
    /// Gets or sets the outline color (hex format).
    /// </summary>
    public string? OutlineColor { get; set; }

    /// <summary>
    /// Gets or sets the background color (hex format).
    /// </summary>
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets whether the text is bold.
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Gets or sets whether the text is italic.
    /// </summary>
    public bool Italic { get; set; }

    /// <summary>
    /// Gets or sets the alignment (1-9, numpad style).
    /// </summary>
    public int Alignment { get; set; } = 2;
}

/// <summary>
/// Represents subtitle format types.
/// </summary>
public enum SubtitleFormat
{
    /// <summary>
    /// SubRip (.srt) format.
    /// </summary>
    SRT,

    /// <summary>
    /// WebVTT (.vtt) format.
    /// </summary>
    VTT,

    /// <summary>
    /// SubStation Alpha (.ssa) format.
    /// </summary>
    SSA,

    /// <summary>
    /// Advanced SubStation Alpha (.ass) format.
    /// </summary>
    ASS,

    /// <summary>
    /// TTML (Timed Text Markup Language).
    /// </summary>
    TTML
}

/// <summary>
/// Represents subtitle delivery protocol.
/// </summary>
public enum SubtitleProtocol
{
    /// <summary>
    /// HTTP(S) download.
    /// </summary>
    Http,

    /// <summary>
    /// Embedded in video stream.
    /// </summary>
    Embedded,

    /// <summary>
    /// HLS embedded subtitles.
    /// </summary>
    HLS
}
