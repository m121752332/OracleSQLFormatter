using System;
using System.IO;

namespace OracleSQL.Formatter.UI.Services;

public static class AppPaths
{
    public static string AppFolder
    {
        get
        {
            var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folder = Path.Combine(root, "OracleSQLFormatter");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
    }

    public static string SettingsPath => Path.Combine(AppFolder, "setting.json");
    public static string FormatterOptionsPath => Path.Combine(AppFolder, "formatter.json");
}
