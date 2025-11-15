using TizenTVApp.ViewModels;

namespace TizenTVApp.Views;

/// <summary>
/// Video player page with playback controls and subtitle support.
/// </summary>
public partial class PlayerPage : ContentPage
{
    private readonly PlayerViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the PlayerPage class.
    /// </summary>
    public PlayerPage(PlayerViewModel viewModel)
    {
        InitializeComponent();
        
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// <summary>
    /// Called when the slider value changes.
    /// </summary>
    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        // This allows the slider to update smoothly during playback
        // The actual seek happens on DragCompleted
    }

    /// <summary>
    /// Called when the page disappears.
    /// </summary>
    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Stop the player when leaving the page
        await _viewModel.StopCommand.ExecuteAsync(null);
    }
}
