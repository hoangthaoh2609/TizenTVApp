using System.Text;
using System.Text.RegularExpressions;
using TizenTVApp.Models;

namespace TizenTVApp.Services;

/// <summary>
/// Service for loading and parsing subtitles in various formats.
/// </summary>
public class SubtitleService : ISubtitleService
{
    private SubtitleTrack? _currentTrack;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the SubtitleService class.
    /// </summary>
    public SubtitleService()
    {
        _httpClient = new HttpClient();
    }

    /// <inheritdoc/>
    public async Task LoadSubtitleAsync(SubtitleTrack track)
    {
        try
        {
            string content;

            if (track.Protocol == SubtitleProtocol.Http)
            {
                content = await _httpClient.GetStringAsync(track.Url);
            }
            else
            {
                throw new NotSupportedException($"Protocol {track.Protocol} is not supported yet.");
            }

            track.Cues = track.Format switch
            {
                SubtitleFormat.SRT => ParseSRT(content),
                SubtitleFormat.VTT => ParseVTT(content),
                SubtitleFormat.SSA or SubtitleFormat.ASS => ParseSSA(content),
                _ => throw new NotSupportedException($"Format {track.Format} is not supported.")
            };

            _currentTrack = track;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load subtitle: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public SubtitleCue? GetCurrentCue(TimeSpan position)
    {
        if (_currentTrack == null)
            return null;

        return _currentTrack.Cues.FirstOrDefault(c =>
            c.StartTime <= position && c.EndTime >= position);
    }

    /// <inheritdoc/>
    public void ClearSubtitle()
    {
        _currentTrack = null;
    }

    /// <summary>
    /// Parses SRT format subtitles.
    /// </summary>
    private List<SubtitleCue> ParseSRT(string content)
    {
        var cues = new List<SubtitleCue>();
        var blocks = Regex.Split(content, @"\r?\n\r?\n").Where(b => !string.IsNullOrWhiteSpace(b));

        foreach (var block in blocks)
        {
            var lines = block.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 3)
                continue;

            // Parse index
            if (!int.TryParse(lines[0].Trim(), out int index))
                continue;

            // Parse timecode (e.g., "00:00:01,000 --> 00:00:04,000")
            var timeMatch = Regex.Match(lines[1], @"(\d{2}):(\d{2}):(\d{2}),(\d{3})\s*-->\s*(\d{2}):(\d{2}):(\d{2}),(\d{3})");
            if (!timeMatch.Success)
                continue;

            var startTime = new TimeSpan(0,
                int.Parse(timeMatch.Groups[1].Value),
                int.Parse(timeMatch.Groups[2].Value),
                int.Parse(timeMatch.Groups[3].Value),
                int.Parse(timeMatch.Groups[4].Value));

            var endTime = new TimeSpan(0,
                int.Parse(timeMatch.Groups[5].Value),
                int.Parse(timeMatch.Groups[6].Value),
                int.Parse(timeMatch.Groups[7].Value),
                int.Parse(timeMatch.Groups[8].Value));

            // Parse text (everything after the timecode)
            var text = string.Join("\n", lines.Skip(2));

            cues.Add(new SubtitleCue
            {
                Index = index,
                StartTime = startTime,
                EndTime = endTime,
                Text = text
            });
        }

        return cues;
    }

    /// <summary>
    /// Parses WebVTT format subtitles.
    /// </summary>
    private List<SubtitleCue> ParseVTT(string content)
    {
        var cues = new List<SubtitleCue>();

        // Remove WEBVTT header
        content = Regex.Replace(content, @"^WEBVTT.*?\r?\n\r?\n", "", RegexOptions.Singleline);

        var blocks = Regex.Split(content, @"\r?\n\r?\n").Where(b => !string.IsNullOrWhiteSpace(b));
        int index = 1;

        foreach (var block in blocks)
        {
            var lines = block.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
                continue;

            // Skip cue identifier if present
            int startLine = 0;
            if (!lines[0].Contains("-->"))
                startLine = 1;

            if (startLine >= lines.Length)
                continue;

            // Parse timecode (e.g., "00:00:01.000 --> 00:00:04.000")
            var timeMatch = Regex.Match(lines[startLine], @"(\d{2}):(\d{2}):(\d{2})\.(\d{3})\s*-->\s*(\d{2}):(\d{2}):(\d{2})\.(\d{3})");
            if (!timeMatch.Success)
                continue;

            var startTime = new TimeSpan(0,
                int.Parse(timeMatch.Groups[1].Value),
                int.Parse(timeMatch.Groups[2].Value),
                int.Parse(timeMatch.Groups[3].Value),
                int.Parse(timeMatch.Groups[4].Value));

            var endTime = new TimeSpan(0,
                int.Parse(timeMatch.Groups[5].Value),
                int.Parse(timeMatch.Groups[6].Value),
                int.Parse(timeMatch.Groups[7].Value),
                int.Parse(timeMatch.Groups[8].Value));

            // Parse text
            var text = string.Join("\n", lines.Skip(startLine + 1));

            cues.Add(new SubtitleCue
            {
                Index = index++,
                StartTime = startTime,
                EndTime = endTime,
                Text = text
            });
        }

        return cues;
    }

    /// <summary>
    /// Parses SSA/ASS format subtitles.
    /// </summary>
    private List<SubtitleCue> ParseSSA(string content)
    {
        var cues = new List<SubtitleCue>();
        var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        bool inEventsSection = false;
        int index = 1;

        foreach (var line in lines)
        {
            if (line.StartsWith("[Events]"))
            {
                inEventsSection = true;
                continue;
            }

            if (line.StartsWith("[") && inEventsSection)
            {
                inEventsSection = false;
                continue;
            }

            if (!inEventsSection || !line.StartsWith("Dialogue:"))
                continue;

            // Parse dialogue line
            // Format: Dialogue: Layer,Start,End,Style,Name,MarginL,MarginR,MarginV,Effect,Text
            var parts = line.Substring(9).Split(',', 10); // Split on comma, max 10 parts
            if (parts.Length < 10)
                continue;

            try
            {
                var startTime = ParseSSATime(parts[1].Trim());
                var endTime = ParseSSATime(parts[2].Trim());
                var text = parts[9].Trim();

                // Remove SSA formatting tags
                text = Regex.Replace(text, @"\{[^}]*\}", "");
                text = text.Replace("\\N", "\n").Replace("\\n", "\n");

                cues.Add(new SubtitleCue
                {
                    Index = index++,
                    StartTime = startTime,
                    EndTime = endTime,
                    Text = text
                });
            }
            catch
            {
                // Skip invalid lines
                continue;
            }
        }

        return cues;
    }

    /// <summary>
    /// Parses SSA time format (H:MM:SS.CC).
    /// </summary>
    private TimeSpan ParseSSATime(string time)
    {
        var match = Regex.Match(time, @"(\d+):(\d{2}):(\d{2})\.(\d{2})");
        if (!match.Success)
            throw new FormatException($"Invalid SSA time format: {time}");

        return new TimeSpan(0,
            int.Parse(match.Groups[1].Value),
            int.Parse(match.Groups[2].Value),
            int.Parse(match.Groups[3].Value),
            int.Parse(match.Groups[4].Value) * 10);
    }
}
