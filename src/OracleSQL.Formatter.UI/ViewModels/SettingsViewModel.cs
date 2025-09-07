using Avalonia;
using Avalonia.Styling;
using Avalonia.Media;
using OracleSQL.Formatter.Common.Enums;
using OracleSQL.Formatter.UI.Localization;
using ReactiveUI;
using OracleSQL.Formatter.UI.Models;
using OracleSQL.Formatter.UI.Services;

namespace OracleSQL.Formatter.UI.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    // 語言
    private Language _currentLanguage = Language.ZhTW;
    public Language CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage.Equals(value)) return;
            this.RaiseAndSetIfChanged(ref _currentLanguage, value);
            Localizer.CurrentLanguage = value;
            Save();
        }
    }
    public Language[] Languages { get; } = System.Enum.GetValues<Language>();

    // 主題
    public enum ThemeMode { System, Light, Dark }

    private ThemeMode _currentTheme;
    public ThemeMode CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme.Equals(value)) return;
            this.RaiseAndSetIfChanged(ref _currentTheme, value);
            ApplyThemeAndBrushes(value);
            Save();
        }
    }

    public ThemeMode[] ThemeModes { get; } = System.Enum.GetValues<ThemeMode>();

    // Theme-aware brushes for binding in Views
    private IBrush _panelBackground = Brushes.Transparent;
    public IBrush PanelBackground
    {
        get => _panelBackground;
        private set => this.RaiseAndSetIfChanged(ref _panelBackground, value);
    }

    private IBrush _cardBackground = Brushes.Transparent;
    public IBrush CardBackground
    {
        get => _cardBackground;
        private set => this.RaiseAndSetIfChanged(ref _cardBackground, value);
    }

    private IBrush _controlBackground = Brushes.Transparent;
    public IBrush ControlBackground
    {
        get => _controlBackground;
        private set => this.RaiseAndSetIfChanged(ref _controlBackground, value);
    }

    private IBrush _textForeground = Brushes.Black;
    public IBrush TextForeground
    {
        get => _textForeground;
        private set => this.RaiseAndSetIfChanged(ref _textForeground, value);
    }

    public SettingsViewModel()
    {
        // Load from setting.json if exists
        var loaded = JsonStorage.Load<AppSettings>(AppPaths.SettingsPath);
        if (loaded is not null)
        {
            _currentLanguage = loaded.Language;
            _currentTheme = loaded.Theme;
        }
        else
        {
            // 依目前 App 設定初始化主題
            var current = Application.Current?.RequestedThemeVariant;
            if (current == ThemeVariant.Light)
                _currentTheme = ThemeMode.Light;
            else if (current == ThemeVariant.Dark)
                _currentTheme = ThemeMode.Dark;
            else
                _currentTheme = ThemeMode.System;
        }

        // 初始化語系到 Localizer & 主題到 App
        Localizer.CurrentLanguage = _currentLanguage;
        ApplyThemeAndBrushes(_currentTheme);
    }

    private void ApplyThemeAndBrushes(ThemeMode mode)
    {
        if (Application.Current is not null)
        {
            Application.Current.RequestedThemeVariant = mode switch
            {
                ThemeMode.Light => ThemeVariant.Light,
                ThemeMode.Dark => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
        }

        // Update local brushes to match theme for immediate visual update
        switch (mode)
        {
            case ThemeMode.Light:
                PanelBackground = new SolidColorBrush(Color.Parse("#f5f5f5"));
                CardBackground = new SolidColorBrush(Color.Parse("#ffffff"));
                ControlBackground = new SolidColorBrush(Color.Parse("#f0f0f0"));
                TextForeground = Brushes.Black;
                break;
            case ThemeMode.Dark:
                PanelBackground = new SolidColorBrush(Color.Parse("#0b0b0b"));
                CardBackground = new SolidColorBrush(Color.Parse("#121212"));
                ControlBackground = new SolidColorBrush(Color.Parse("#1a1a1a"));
                TextForeground = Brushes.White;
                break;
            default:
                // System: follow Application.Current RequestedThemeVariant if available
                var rv = Application.Current?.RequestedThemeVariant ?? ThemeVariant.Default;
                if (rv == ThemeVariant.Light)
                {
                    PanelBackground = new SolidColorBrush(Color.Parse("#f5f5f5"));
                    CardBackground = new SolidColorBrush(Color.Parse("#ffffff"));
                    ControlBackground = new SolidColorBrush(Color.Parse("#f0f0f0"));
                    TextForeground = Brushes.Black;
                }
                else
                {
                    PanelBackground = new SolidColorBrush(Color.Parse("#0b0b0b"));
                    CardBackground = new SolidColorBrush(Color.Parse("#121212"));
                    ControlBackground = new SolidColorBrush(Color.Parse("#1a1a1a"));
                    TextForeground = Brushes.White;
                }
                break;
        }
    }

    private void Save()
    {
        var dto = new AppSettings
        {
            Language = _currentLanguage,
            Theme = _currentTheme
        };
        JsonStorage.Save(AppPaths.SettingsPath, dto);
    }
}
