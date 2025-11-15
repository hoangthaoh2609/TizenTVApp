namespace TizenTVApp.Services;

/// <summary>
/// Service for handling navigation within the app.
/// </summary>
public class NavigationService : INavigationService
{
    /// <inheritdoc/>
    public async Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        try
        {
            if (parameters != null)
            {
                await Shell.Current.GoToAsync(route, parameters);
            }
            else
            {
                await Shell.Current.GoToAsync(route);
            }
        }
        catch (Exception ex)
        {
            // Log the error
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task GoBackAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            // Log the error
            System.Diagnostics.Debug.WriteLine($"Navigation back error: {ex.Message}");
            throw;
        }
    }
}
