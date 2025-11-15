using TizenTVApp.Models;

namespace TizenTVApp.Services;

/// <summary>
/// Service for managing video content with mock data and HLS samples.
/// </summary>
public class ContentService : IContentService
{
    private readonly List<VideoContent> _mockContent;

    /// <summary>
    /// Initializes a new instance of the ContentService class.
    /// </summary>
    public ContentService()
    {
        _mockContent = GenerateMockContent();
    }

    /// <inheritdoc/>
    public async Task<List<VideoContent>> GetFeaturedContentAsync()
    {
        await Task.Delay(100); // Simulate network delay
        return _mockContent.Where(c => c.Rating == "Featured").ToList();
    }

    /// <inheritdoc/>
    public async Task<List<VideoContent>> GetContentByCategoryAsync(string category)
    {
        await Task.Delay(100); // Simulate network delay
        return _mockContent.Where(c => c.Category == category).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetCategoriesAsync()
    {
        await Task.Delay(50); // Simulate network delay
        return _mockContent.Select(c => c.Category).Distinct().OrderBy(c => c).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<VideoContent>> SearchContentAsync(string query)
    {
        await Task.Delay(100); // Simulate network delay
        var lowerQuery = query.ToLower();
        return _mockContent.Where(c =>
            c.Title.ToLower().Contains(lowerQuery) ||
            c.Description.ToLower().Contains(lowerQuery) ||
            c.Category.ToLower().Contains(lowerQuery)
        ).ToList();
    }

    /// <summary>
    /// Generates mock video content with HLS samples.
    /// </summary>
    private List<VideoContent> GenerateMockContent()
    {
        return new List<VideoContent>
        {
            // Apple HLS Test Streams
            new VideoContent
            {
                Id = "1",
                Title = "Big Buck Bunny",
                Description = "Open source animation short featuring a large white rabbit. Perfect for testing adaptive streaming.",
                ThumbnailUrl = "https://peach.blender.org/wp-content/uploads/title_anouncement.jpg",
                VideoUrl = "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 634,
                Category = "Animation",
                Rating = "Featured",
                Year = 2008,
                Subtitles = new List<SubtitleTrack>
                {
                    new SubtitleTrack
                    {
                        Id = "en",
                        Language = "en",
                        Label = "English",
                        Url = "https://example.com/subtitles/bigbuck_en.srt",
                        Format = SubtitleFormat.SRT,
                        Protocol = SubtitleProtocol.Http
                    }
                }
            },
            new VideoContent
            {
                Id = "2",
                Title = "Sintel",
                Description = "Fantasy short film about a young woman searching for her dragon. High-quality CGI animation.",
                ThumbnailUrl = "https://durian.blender.org/wp-content/uploads/2010/09/sintel_trailer_1080p.jpg",
                VideoUrl = "https://bitdash-a.akamaihd.net/content/sintel/hls/playlist.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 888,
                Category = "Animation",
                Rating = "Featured",
                Year = 2010,
                Subtitles = new List<SubtitleTrack>
                {
                    new SubtitleTrack
                    {
                        Id = "en",
                        Language = "en",
                        Label = "English",
                        Url = "https://example.com/subtitles/sintel_en.vtt",
                        Format = SubtitleFormat.VTT,
                        Protocol = SubtitleProtocol.Http
                    }
                }
            },
            new VideoContent
            {
                Id = "3",
                Title = "Tears of Steel",
                Description = "Sci-fi short film combining live action and CGI. Tests complex visual effects streaming.",
                ThumbnailUrl = "https://mango.blender.org/wp-content/uploads/2012/09/01_thom_celia_bridge.jpg",
                VideoUrl = "https://demo.unified-streaming.com/k8s/features/stable/video/tears-of-steel/tears-of-steel.ism/.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 734,
                Category = "Sci-Fi",
                Rating = "Featured",
                Year = 2012,
                Subtitles = new List<SubtitleTrack>()
            },
            new VideoContent
            {
                Id = "4",
                Title = "Elephants Dream",
                Description = "Surrealist fantasy about two characters exploring a bizarre machine world.",
                ThumbnailUrl = "https://orange.blender.org/wp-content/themes/orange/images/media/p_01.jpg",
                VideoUrl = "https://archive.org/download/ElephantsDream/ed_hd.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 654,
                Category = "Animation",
                Rating = "G",
                Year = 2006,
                Subtitles = new List<SubtitleTrack>()
            },
            new VideoContent
            {
                Id = "5",
                Title = "Cosmos Laundromat",
                Description = "A depressed sheep finds himself in surreal situations. Stunning visual effects showcase.",
                ThumbnailUrl = "https://gooseberry.blender.org/wp-content/uploads/2015/08/shot_09_comp_c02_v11_still.jpg",
                VideoUrl = "https://ftp.nluug.nl/pub/graphics/blender/demo/movies/cosmos_laundromat/cosmos_laundromat_4k.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 734,
                Category = "Animation",
                Rating = "G",
                Year = 2015,
                Subtitles = new List<SubtitleTrack>()
            },
            new VideoContent
            {
                Id = "6",
                Title = "Nature Documentary",
                Description = "Beautiful wildlife footage from around the world in stunning 4K resolution.",
                ThumbnailUrl = "https://picsum.photos/400/225?random=6",
                VideoUrl = "https://demo.unified-streaming.com/k8s/features/stable/video/tears-of-steel/tears-of-steel.ism/.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 1800,
                Category = "Documentary",
                Rating = "G",
                Year = 2020,
                Subtitles = new List<SubtitleTrack>()
            },
            new VideoContent
            {
                Id = "7",
                Title = "Space Exploration",
                Description = "Journey through the cosmos exploring planets, stars, and galaxies.",
                ThumbnailUrl = "https://picsum.photos/400/225?random=7",
                VideoUrl = "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 2400,
                Category = "Documentary",
                Rating = "G",
                Year = 2021,
                Subtitles = new List<SubtitleTrack>
                {
                    new SubtitleTrack
                    {
                        Id = "en",
                        Language = "en",
                        Label = "English",
                        Url = "https://example.com/subtitles/space_en.srt",
                        Format = SubtitleFormat.SRT,
                        Protocol = SubtitleProtocol.Http
                    },
                    new SubtitleTrack
                    {
                        Id = "es",
                        Language = "es",
                        Label = "Español",
                        Url = "https://example.com/subtitles/space_es.srt",
                        Format = SubtitleFormat.SRT,
                        Protocol = SubtitleProtocol.Http
                    }
                }
            },
            new VideoContent
            {
                Id = "8",
                Title = "Ocean Mysteries",
                Description = "Dive deep into the ocean to discover amazing marine life and underwater landscapes.",
                ThumbnailUrl = "https://picsum.photos/400/225?random=8",
                VideoUrl = "https://bitdash-a.akamaihd.net/content/sintel/hls/playlist.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 3000,
                Category = "Documentary",
                Rating = "G",
                Year = 2022,
                Subtitles = new List<SubtitleTrack>()
            },
            new VideoContent
            {
                Id = "9",
                Title = "Mountain Adventures",
                Description = "Experience breathtaking mountain climbing expeditions around the world.",
                ThumbnailUrl = "https://picsum.photos/400/225?random=9",
                VideoUrl = "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 2700,
                Category = "Sports",
                Rating = "PG",
                Year = 2021,
                Subtitles = new List<SubtitleTrack>()
            },
            new VideoContent
            {
                Id = "10",
                Title = "Urban Exploration",
                Description = "Discover the hidden beauty and architecture of major cities worldwide.",
                ThumbnailUrl = "https://picsum.photos/400/225?random=10",
                VideoUrl = "https://demo.unified-streaming.com/k8s/features/stable/video/tears-of-steel/tears-of-steel.ism/.m3u8",
                Protocol = StreamingProtocol.HLS,
                DurationSeconds = 1500,
                Category = "Travel",
                Rating = "G",
                Year = 2023,
                Subtitles = new List<SubtitleTrack>()
            }
        };
    }
}
