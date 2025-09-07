using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OracleSQL.Formatter.UI.Localization;

namespace OracleSQL.Formatter.UI.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        Localizer.LanguageChanged += () => Dispatcher.UIThread.Post(() => {
            // force visual refresh if needed
            // no-op; binding to SettingsViewModel will update CurrentLanguage
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
