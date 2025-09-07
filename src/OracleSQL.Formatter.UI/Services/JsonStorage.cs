using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace OracleSQL.Formatter.UI.Services;

public static class JsonStorage
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public static T? Load<T>(string path)
    {
        try
        {
            if (!File.Exists(path)) return default;
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json, Options);
        }
        catch
        {
            return default;
        }
    }

    public static void Save<T>(string path, T value)
    {
        try
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var json = JsonSerializer.Serialize(value, Options);
            File.WriteAllText(path, json);

            // Notify on console where the file was saved (may not be visible for GUI apps)
            try
            {
                Console.WriteLine($"Saved JSON to: {path}");
            }
            catch { }

            // Also write to trace output (visible in debugger output)
            try
            {
                Trace.WriteLine($"Saved JSON to: {path}");
            }
            catch { }

            // Also write a persistent log next to settings (AppPaths may be used)
            try
            {
                var logDir = Path.GetDirectoryName(path) ?? Path.GetTempPath();
                var logPath = Path.Combine(logDir, "json.save.log");
                File.AppendAllText(logPath, $"[{DateTime.Now:O}] Saved JSON to: {path}{Environment.NewLine}");
            }
            catch { }
        }
        catch (Exception ex)
        {
            // Log exception to temporary log so failures are visible even without console
            try
            {
                var logPath = Path.Combine(Path.GetTempPath(), "OracleSQLFormatter.json.error.log");
                File.AppendAllText(logPath, $"[{DateTime.Now:O}] Failed to save JSON to: {path}{Environment.NewLine}{ex}{Environment.NewLine}");
            }
            catch { }
        }
    }
}
