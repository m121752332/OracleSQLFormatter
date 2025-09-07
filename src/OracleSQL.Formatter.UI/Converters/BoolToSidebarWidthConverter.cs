using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace OracleSQL.Formatter.UI.Converters;

public class BoolToSidebarWidthConverter : IValueConverter
{
    public double OpenWidth { get; set; } = 220;
    public double ClosedWidth { get; set; } = 56;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b ? OpenWidth : ClosedWidth;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
