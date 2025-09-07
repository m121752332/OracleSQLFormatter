using OracleSQL.Formatter.Common.Enums;

namespace OracleSQL.Formatter.Common.Helpers;

public static class LocalizationHelper
{
    public static Language CurrentLanguage { get; private set; } = Language.ZhTW;

    public static void SetLanguage(Language language)
    {
        CurrentLanguage = language;
        // In a full app, hook up resource managers.
    }
}
