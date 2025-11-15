using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TizenTVApp.Models;
using TizenTVApp.Services;
using TizenTVApp.Views;

namespace TizenTVApp.ViewModels;

/// <summary>
/// ViewModel for the main home screen.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly IContentService _contentService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private ObservableCollection<VideoContent> _featuredContent = new();

    [ObservableProperty]
    private ObservableCollection<string> _categories = new();

    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    public MainViewModel(IContentService contentService, INavigationService navigationService)
    {
        _contentService = contentService;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Loads the initial data.
    /// </summary>
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            // Load featured content
            var featured = await _contentService.GetFeaturedContentAsync();
            FeaturedContent = new ObservableCollection<VideoContent>(featured);

            // Load categories
            var categories = await _contentService.GetCategoriesAsync();
            Categories = new ObservableCollection<string>(categories);
        }
        catch (Exception ex)
        {
            // Handle error
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Navigates to the browse page for a specific category.
    /// </summary>
    [RelayCommand]
    private async Task BrowseCategoryAsync(string category)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Category", category }
        };

        await _navigationService.NavigateToAsync(nameof(BrowsePage), parameters);
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
}
