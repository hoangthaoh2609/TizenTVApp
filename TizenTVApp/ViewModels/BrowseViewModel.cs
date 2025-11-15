using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TizenTVApp.Models;
using TizenTVApp.Services;
using TizenTVApp.Views;

namespace TizenTVApp.ViewModels;

/// <summary>
/// ViewModel for the browse page.
/// </summary>
public partial class BrowseViewModel : ObservableObject, IQueryAttributable
{
    private readonly IContentService _contentService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private ObservableCollection<VideoContent> _content = new();

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    /// <summary>
    /// Initializes a new instance of the BrowseViewModel class.
    /// </summary>
    public BrowseViewModel(IContentService contentService, INavigationService navigationService)
    {
        _contentService = contentService;
        _navigationService = navigationService;
    }

    /// <inheritdoc/>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Category"))
        {
            Category = query["Category"].ToString() ?? string.Empty;
            _ = LoadContentAsync();
        }
    }

    /// <summary>
    /// Loads content for the current category.
    /// </summary>
    [RelayCommand]
    public async Task LoadContentAsync()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            var content = string.IsNullOrEmpty(Category)
                ? await _contentService.GetFeaturedContentAsync()
                : await _contentService.GetContentByCategoryAsync(Category);

            Content = new ObservableCollection<VideoContent>(content);
        }
        catch (Exception ex)
        {
            // Handle error
            System.Diagnostics.Debug.WriteLine($"Error loading content: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Searches for content.
    /// </summary>
    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            await LoadContentAsync();
            return;
        }

        try
        {
            IsLoading = true;

            var results = await _contentService.SearchContentAsync(SearchQuery);
            Content = new ObservableCollection<VideoContent>(results);
        }
        catch (Exception ex)
        {
            // Handle error
            System.Diagnostics.Debug.WriteLine($"Error searching content: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Plays the selected video content.
    /// </summary>
    [RelayCommand]
    private async Task PlayVideoAsync(VideoContent content)
    {
        var parameters = new Dictionary<string, object>
        {
            { "VideoContent", content }
        };

        await _navigationService.NavigateToAsync(nameof(PlayerPage), parameters);
    }

    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    [RelayCommand]
    private async Task GoBackAsync()
    {
        await _navigationService.GoBackAsync();
    }
}
