using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;

namespace OracleSQL.Formatter;

internal static class Program
{
    [STAThread]
    // Avalonia entry point
    public static void Main(string[] args)
    {
        // Start UI if explicit flag passed, or if running interactively / under debugger
        bool startUi = (args is not null && args.Length > 0 && args[0] == "--ui")
            || Environment.UserInteractive
            || Debugger.IsAttached;

        if (startUi)
        {
            try
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args ?? Array.Empty<string>());
            }
            catch (Exception ex)
            {
                try
                {
                    // Try to write to console first
                    Console.Error.WriteLine("Exception starting Avalonia application:");
                    Console.Error.WriteLine(ex.ToString());
                    if (ex.InnerException != null)
                    {
                        Console.Error.WriteLine("Inner exception:");
                        Console.Error.WriteLine(ex.InnerException.ToString());
                    }
                }
                catch
                {
                    // If no console is available, write to temp log file for diagnosis
                    try
                    {
                        var logPath = Path.Combine(Path.GetTempPath(), "OracleSQLFormatter.error.log");
                        File.WriteAllText(logPath, ex.ToString());
                    }
                    catch
                    {
                        // last resort: swallow to avoid throwing on shutdown
                    }
                }

                Environment.Exit(1);
            }
        }
        else
        {
            Console.WriteLine("UI not started. To start the Avalonia UI, run with the '--ui' argument.");
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
