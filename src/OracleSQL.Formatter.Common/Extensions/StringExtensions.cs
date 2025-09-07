using System.Globalization;

namespace OracleSQL.Formatter.Common.Extensions;

public static class StringExtensions
{
    public static string ToCase(this string input, Enums.CaseOption option)
    {
        return option switch
        {
            Enums.CaseOption.Uppercase => input.ToUpperInvariant(),
            Enums.CaseOption.Lowercase => input.ToLowerInvariant(),
            Enums.CaseOption.InitCap => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input.ToLowerInvariant()),
            _ => input,
        };
    }
}
