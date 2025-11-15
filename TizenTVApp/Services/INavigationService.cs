namespace TizenTVApp.Services;

/// <summary>
/// Interface for navigation service.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to a page.
    /// </summary>
    Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null);

    /// <summary>
    /// Navigates back.
    /// </summary>
    Task GoBackAsync();
}
