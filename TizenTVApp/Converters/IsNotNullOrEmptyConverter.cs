using System.Globalization;

namespace TizenTVApp.Converters;

/// <summary>
/// Converter that returns true if the value is not null or empty.
/// </summary>
public class IsNotNullOrEmptyConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
            return !string.IsNullOrEmpty(stringValue);
        
        return value != null;
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
