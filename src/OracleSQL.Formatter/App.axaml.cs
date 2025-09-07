using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OracleSQL.Formatter.UI.Views;

namespace OracleSQL.Formatter;

public partial class App : Application
{
    public override void Initialize()
    {
        try
        {
            AvaloniaXamlLoader.Load(this);
        }
        catch (XamlLoadException xlex)
        {
            LogXamlError("App.Initialize", xlex);
            throw;
        }
        catch (Exception ex)
        {
            LogXamlError("App.Initialize", ex);
            throw;
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }
        }
        catch (Exception ex)
        {
            LogXamlError("App.OnFrameworkInitializationCompleted", ex);
            throw;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void LogXamlError(string where, Exception ex)
    {
        try
        {
            var path = Path.Combine(Path.GetTempPath(), "OracleSQLFormatter.xaml.error.log");
            File.AppendAllText(path, $"[{DateTime.Now:O}] {where} failed: {ex}\n\n");
            Console.Error.WriteLine($"{where} failed: {ex.Message}\nSee log: {path}");
            if (ex.InnerException != null)
            {
                File.AppendAllText(path, $"Inner: {ex.InnerException}\n\n");
            }
        }
        catch
        {
            // ignore
        }
    }
}
