using TizenTVApp.ViewModels;

namespace TizenTVApp.Views;

/// <summary>
/// Browse page for exploring content by category.
/// </summary>
public partial class BrowsePage : ContentPage
{
    private readonly BrowseViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the BrowsePage class.
    /// </summary>
    public BrowsePage(BrowseViewModel viewModel)
    {
        InitializeComponent();
        
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}
