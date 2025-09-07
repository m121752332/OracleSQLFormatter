using Avalonia.Data.Converters;
using System;
using System.Globalization;
using OracleSQL.Formatter.UI.Localization;

namespace OracleSQL.Formatter.UI.Converters;

public class LocalizationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string key)
        {
            return Localizer.T(key);
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
