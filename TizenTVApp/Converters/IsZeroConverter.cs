using System.Globalization;

namespace TizenTVApp.Converters;

/// <summary>
/// Converter that returns true if the value is zero.
/// </summary>
public class IsZeroConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
            return intValue == 0;
        
        if (value is double doubleValue)
            return doubleValue == 0.0;
        
        if (value is float floatValue)
            return floatValue == 0.0f;
        
        return true;
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
