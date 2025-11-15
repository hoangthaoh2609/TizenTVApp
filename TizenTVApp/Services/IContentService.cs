using TizenTVApp.Models;

namespace TizenTVApp.Services;

/// <summary>
/// Interface for content service.
/// </summary>
public interface IContentService
{
    /// <summary>
    /// Gets featured content.
    /// </summary>
    Task<List<VideoContent>> GetFeaturedContentAsync();

    /// <summary>
    /// Gets content by category.
    /// </summary>
    Task<List<VideoContent>> GetContentByCategoryAsync(string category);

    /// <summary>
    /// Gets all available categories.
    /// </summary>
    Task<List<string>> GetCategoriesAsync();

    /// <summary>
    /// Searches for content.
    /// </summary>
    Task<List<VideoContent>> SearchContentAsync(string query);
}
