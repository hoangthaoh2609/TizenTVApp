using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TizenTVApp.Models;
using TizenTVApp.Services;

namespace TizenTVApp.ViewModels;

/// <summary>
/// ViewModel for the video player page.
/// </summary>
public partial class PlayerViewModel : ObservableObject, IQueryAttributable
{
    private readonly IVideoPlayerService _playerService;
    private readonly ISubtitleService _subtitleService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private VideoContent? _currentVideo;

    [ObservableProperty]
    private PlayerState _playerState = PlayerState.Idle;

    [ObservableProperty]
    private TimeSpan _position = TimeSpan.Zero;

    [ObservableProperty]
    private TimeSpan _duration = TimeSpan.Zero;

    [ObservableProperty]
    private double _volume = 1.0;

    [ObservableProperty]
    private string _currentSubtitle = string.Empty;

    [ObservableProperty]
    private SubtitleTrack? _selectedSubtitleTrack;

    [ObservableProperty]
    private ObservableCollection<SubtitleTrack> _availableSubtitles = new();

    [ObservableProperty]
    private bool _isControlsVisible = true;

    [ObservableProperty]
    private bool _isLoading;

    /// <summary>
    /// Gets whether the player is playing.
    /// </summary>
    public bool IsPlaying => PlayerState == PlayerState.Playing;

    /// <summary>
    /// Gets whether the player is paused.
    /// </summary>
    public bool IsPaused => PlayerState == PlayerState.Paused;

    /// <summary>
    /// Gets the formatted position string.
    /// </summary>
    public string FormattedPosition => Position.ToString(@"hh\:mm\:ss");

    /// <summary>
    /// Gets the formatted duration string.
    /// </summary>
    public string FormattedDuration => Duration.ToString(@"hh\:mm\:ss");

    /// <summary>
    /// Gets the progress percentage (0-100).
    /// </summary>
    public double ProgressPercentage => Duration.TotalSeconds > 0
        ? (Position.TotalSeconds / Duration.TotalSeconds) * 100
        : 0;

    /// <summary>
    /// Initializes a new instance of the PlayerViewModel class.
    /// </summary>
    public PlayerViewModel(
        IVideoPlayerService playerService,
        ISubtitleService subtitleService,
        INavigationService navigationService)
    {
        _playerService = playerService;
        _subtitleService = subtitleService;
        _navigationService = navigationService;

        // Subscribe to player events
        _playerService.StateChanged += OnPlayerStateChanged;
        _playerService.PositionChanged += OnPlayerPositionChanged;
        _playerService.ErrorOccurred += OnPlayerError;
    }

    /// <inheritdoc/>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("VideoContent") && query["VideoContent"] is VideoContent content)
        {
            CurrentVideo = content;
            _ = InitializePlayerAsync();
        }
    }

    /// <summary>
    /// Initializes the player with the current video.
    /// </summary>
    [RelayCommand]
    private async Task InitializePlayerAsync()
    {
        if (CurrentVideo == null)
            return;

        try
        {
            IsLoading = true;

            // Load available subtitles
            AvailableSubtitles = new ObservableCollection<SubtitleTrack>(CurrentVideo.Subtitles);

            // Prepare the player
            await _playerService.PrepareAsync(CurrentVideo);

            Duration = _playerService.Duration;

            // Auto-play
            await PlayAsync();
        }
        catch (Exception ex)
        {
            // Handle error
            System.Diagnostics.Debug.WriteLine($"Error initializing player: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Plays or resumes the video.
    /// </summary>
    [RelayCommand]
    private async Task PlayAsync()
    {
        try
        {
            await _playerService.PlayAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error playing video: {ex.Message}");
        }
    }

    /// <summary>
    /// Pauses the video.
    /// </summary>
    [RelayCommand]
    private async Task PauseAsync()
    {
        try
        {
            await _playerService.PauseAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error pausing video: {ex.Message}");
        }
    }

    /// <summary>
    /// Toggles play/pause.
    /// </summary>
    [RelayCommand]
    private async Task TogglePlayPauseAsync()
    {
        if (IsPlaying)
        {
            await PauseAsync();
        }
        else
        {
            await PlayAsync();
        }
    }

    /// <summary>
    /// Stops the video.
    /// </summary>
    [RelayCommand]
    private async Task StopAsync()
    {
        try
        {
            await _playerService.StopAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error stopping video: {ex.Message}");
        }
    }

    /// <summary>
    /// Seeks forward by a specified number of seconds.
    /// </summary>
    [RelayCommand]
    private async Task SeekForwardAsync(int seconds = 10)
    {
        try
        {
            var newPosition = Position.Add(TimeSpan.FromSeconds(seconds));
            if (newPosition > Duration)
                newPosition = Duration;

            await _playerService.SeekAsync(newPosition);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error seeking forward: {ex.Message}");
        }
    }

    /// <summary>
    /// Seeks backward by a specified number of seconds.
    /// </summary>
    [RelayCommand]
    private async Task SeekBackwardAsync(int seconds = 10)
    {
        try
        {
            var newPosition = Position.Subtract(TimeSpan.FromSeconds(seconds));
            if (newPosition < TimeSpan.Zero)
                newPosition = TimeSpan.Zero;

            await _playerService.SeekAsync(newPosition);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error seeking backward: {ex.Message}");
        }
    }

    /// <summary>
    /// Seeks to a specific position.
    /// </summary>
    [RelayCommand]
    private async Task SeekToAsync(double progressPercentage)
    {
        try
        {
            var newPosition = TimeSpan.FromSeconds(Duration.TotalSeconds * progressPercentage / 100.0);
            await _playerService.SeekAsync(newPosition);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error seeking: {ex.Message}");
        }
    }

    /// <summary>
    /// Changes the volume.
    /// </summary>
    [RelayCommand]
    private void ChangeVolume(double delta)
    {
        Volume = Math.Clamp(Volume + delta, 0.0, 1.0);
        _playerService.Volume = Volume;
    }

    /// <summary>
    /// Selects a subtitle track.
    /// </summary>
    [RelayCommand]
    private async Task SelectSubtitleAsync(SubtitleTrack? track)
    {
        try
        {
            SelectedSubtitleTrack = track;
            await _playerService.SetSubtitleTrackAsync(track);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error selecting subtitle: {ex.Message}");
        }
    }

    /// <summary>
    /// Toggles controls visibility.
    /// </summary>
    [RelayCommand]
    private void ToggleControls()
    {
        IsControlsVisible = !IsControlsVisible;
    }

    /// <summary>
    /// Closes the player and navigates back.
    /// </summary>
    [RelayCommand]
    private async Task CloseAsync()
    {
        try
        {
            await _playerService.StopAsync();
            await _navigationService.GoBackAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error closing player: {ex.Message}");
        }
    }

    private void OnPlayerStateChanged(object? sender, PlayerState state)
    {
        PlayerState = state;
        OnPropertyChanged(nameof(IsPlaying));
        OnPropertyChanged(nameof(IsPaused));
    }

    private void OnPlayerPositionChanged(object? sender, TimeSpan position)
    {
        Position = position;
        OnPropertyChanged(nameof(FormattedPosition));
        OnPropertyChanged(nameof(ProgressPercentage));

        // Update current subtitle
        var cue = _subtitleService.GetCurrentCue(position);
        CurrentSubtitle = cue?.Text ?? string.Empty;
    }

    private void OnPlayerError(object? sender, string error)
    {
        // Handle error
        System.Diagnostics.Debug.WriteLine($"Player error: {error}");
    }
}
