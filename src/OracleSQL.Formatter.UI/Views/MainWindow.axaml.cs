using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OracleSQL.Formatter.UI.Localization;

namespace OracleSQL.Formatter.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new OracleSQL.Formatter.UI.ViewModels.MainWindowViewModel();

        // Try set window icon from embedded Avalonia resource (ICO)
        try
        {
            this.Icon = new WindowIcon("avares://OracleSQL.Formatter.UI/Assets/images/logo.ico");
        }
        catch
        {
            // ignore if icon couldn't be set
        }

        // Apply localized strings
        UpdateLocalization();
        Localizer.LanguageChanged += () => Dispatcher.UIThread.Post(UpdateLocalization);

        // Also listen to Settings changes to apply theme brushes
        if (DataContext is OracleSQL.Formatter.UI.ViewModels.MainWindowViewModel vm)
        {
            vm.Settings.PropertyChanged += (_, __) => Dispatcher.UIThread.Post(ApplyThemeBrushes);
        }

        // apply initially
        Dispatcher.UIThread.Post(ApplyThemeBrushes);
    }

    private void InitializeComponent()
    {
        try
        {
            AvaloniaXamlLoader.Load(this);
        }
        catch (XamlLoadException xlex)
        {
            LogXamlError("MainWindow", xlex);
            throw;
        }
        catch (Exception ex)
        {
            LogXamlError("MainWindow", ex);
            throw;
        }
    }

    private void UpdateLocalization()
    {
        try
        {
            // Update window title
            this.Title = Localizer.T("App.Title");

            // Find controls by name to avoid relying on generated fields which may be null
            var header = this.FindControl<TextBlock>("HeaderTitle");
            if (header is not null)
                header.Text = Localizer.T("App.Title");

            var sidebarHeader = this.FindControl<TextBlock>("SidebarHeader");
            if (sidebarHeader is not null)
                sidebarHeader.Text = Localizer.T("Sidebar.Functions");

            var beautifier = this.FindControl<TextBlock>("BeautifierLabel");
            if (beautifier is not null)
                beautifier.Text = Localizer.T("Sidebar.Beautifier");

            var settings = this.FindControl<TextBlock>("SettingsLabel");
            if (settings is not null)
                settings.Text = Localizer.T("Sidebar.Settings");

            // Update tooltips on buttons so hover works when collapsed
            var beautifierBtn = this.FindControl<Button>("BeautifierButton");
            if (beautifierBtn is not null)
                ToolTip.SetTip(beautifierBtn, Localizer.T("Sidebar.Beautifier"));

            var settingsBtn = this.FindControl<Button>("SettingsButton");
            if (settingsBtn is not null)
                ToolTip.SetTip(settingsBtn, Localizer.T("Sidebar.Settings"));
        }
        catch { }
    }

    private void ApplyThemeBrushes()
    {
        try
        {
            if (DataContext is not OracleSQL.Formatter.UI.ViewModels.MainWindowViewModel vm) return;
            var headerBorder = this.FindControl<Border>("HeaderBorder");
            var sidebarBorder = this.FindControl<Border>("SidebarBorder");
            var root = this.FindControl<Grid>("RootGrid");

            if (headerBorder is not null && vm.Settings is not null)
            {
                headerBorder.Background = vm.Settings.CardBackground;
            }
            if (sidebarBorder is not null && vm.Settings is not null)
            {
                sidebarBorder.Background = vm.Settings.ControlBackground;
            }

            if (root is not null && vm.Settings is not null)
            {
                root.Background = vm.Settings.PanelBackground as Avalonia.Media.IBrush;
            }
        }
        catch { }
    }

    private static void LogXamlError(string view, Exception ex)
    {
        try
        {
            var path = Path.Combine(Path.GetTempPath(), "OracleSQLFormatter.xaml.error.log");
            File.AppendAllText(path, $"[{DateTime.Now:O}] {view} XAML load failed: {ex}\n\n");
            Console.Error.WriteLine($"{view} XAML load failed: {ex.Message}\nSee log: {path}");
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
