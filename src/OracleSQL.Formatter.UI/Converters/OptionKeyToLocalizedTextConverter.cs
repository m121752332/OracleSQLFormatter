using System;
using Avalonia.Data.Converters;
using System.Globalization;
using OracleSQL.Formatter.UI.Localization;

namespace OracleSQL.Formatter.UI.Converters;

public class OptionKeyToLocalizedTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return null;
        var prefix = parameter as string; // e.g., "Comma", "ListStyle", "Align"
        var key = value.ToString();
        if (string.IsNullOrEmpty(key)) return value;
        if (!string.IsNullOrEmpty(prefix))
        {
            var fullKey = $"{prefix}.{key}";
            return Localizer.T(fullKey);
        }
        return Localizer.T(key);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
