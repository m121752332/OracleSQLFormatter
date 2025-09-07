using OracleSQL.Formatter.Common.Enums;
using OracleSQL.Formatter.UI.ViewModels;

namespace OracleSQL.Formatter.UI.Models;

public class AppSettings
{
    public Language Language { get; set; } = Language.ZhTW; // store enum names in English
    public SettingsViewModel.ThemeMode Theme { get; set; } = SettingsViewModel.ThemeMode.Dark;
}
