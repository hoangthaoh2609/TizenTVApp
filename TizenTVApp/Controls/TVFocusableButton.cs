using Microsoft.Maui.Controls;

namespace TizenTVApp.Controls;

/// <summary>
/// Custom button with TV-optimized focus indicators.
/// </summary>
public class TVFocusableButton : Button
{
    /// <summary>
    /// Bindable property for focus border color.
    /// </summary>
    public static readonly BindableProperty FocusBorderColorProperty =
        BindableProperty.Create(
            nameof(FocusBorderColor),
            typeof(Color),
            typeof(TVFocusableButton),
            Colors.White);

    /// <summary>
    /// Bindable property for focus border width.
    /// </summary>
    public static readonly BindableProperty FocusBorderWidthProperty =
        BindableProperty.Create(
            nameof(FocusBorderWidth),
            typeof(double),
            typeof(TVFocusableButton),
            4.0);

    /// <summary>
    /// Bindable property for focus scale factor.
    /// </summary>
    public static readonly BindableProperty FocusScaleProperty =
        BindableProperty.Create(
            nameof(FocusScale),
            typeof(double),
            typeof(TVFocusableButton),
            1.1);

    /// <summary>
    /// Gets or sets the focus border color.
    /// </summary>
    public Color FocusBorderColor
    {
        get => (Color)GetValue(FocusBorderColorProperty);
        set => SetValue(FocusBorderColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the focus border width.
    /// </summary>
    public double FocusBorderWidth
    {
        get => (double)GetValue(FocusBorderWidthProperty);
        set => SetValue(FocusBorderWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the focus scale factor.
    /// </summary>
    public double FocusScale
    {
        get => (double)GetValue(FocusScaleProperty);
        set => SetValue(FocusScaleProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the TVFocusableButton class.
    /// </summary>
    public TVFocusableButton()
    {
        // Set default TV-optimized properties
        FontSize = 32;
        FontAttributes = FontAttributes.Bold;
        Padding = new Thickness(40, 20);
        CornerRadius = 8;
        HeightRequest = 80;
        MinimumWidthRequest = 200;

        // Subscribe to focus events
        Focused += OnFocused;
        Unfocused += OnUnfocused;
    }

    /// <summary>
    /// Handles the focused event.
    /// </summary>
    private async void OnFocused(object? sender, FocusEventArgs e)
    {
        // Apply focus effects
        BorderColor = FocusBorderColor;
        BorderWidth = FocusBorderWidth;
        
        // Animate scale
        await this.ScaleTo(FocusScale, 150, Easing.CubicOut);
    }

    /// <summary>
    /// Handles the unfocused event.
    /// </summary>
    private async void OnUnfocused(object? sender, FocusEventArgs e)
    {
        // Remove focus effects
        BorderWidth = 0;
        
        // Animate scale back
        await this.ScaleTo(1.0, 150, Easing.CubicIn);
    }
}
