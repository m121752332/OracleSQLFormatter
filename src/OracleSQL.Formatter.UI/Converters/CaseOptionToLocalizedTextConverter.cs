using System;
using Avalonia.Data.Converters;
using OracleSQL.Formatter.Common.Enums;
using OracleSQL.Formatter.UI.Localization;

namespace OracleSQL.Formatter.UI.Converters;

public class CaseOptionToLocalizedTextConverter : IValueConverter
{
    public CaseOptionToLocalizedTextConverter()
    {
        Localizer.LanguageChanged += () => { /* force UI refresh via binding updates if needed */ };
    }

    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is CaseOption opt)
        {
            return Localizer.T($"CaseOption.{opt}");
        }
        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => throw new NotSupportedException();
}
