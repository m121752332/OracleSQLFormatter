using System;
using ReactiveUI;
using OracleSQL.Formatter.Core.Services;
using System.Reactive;

namespace OracleSQL.Formatter.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private object? _currentView;
    private bool _isSidebarOpen = true;

    public string Title => "Oracle SQL Formatter";

    public FormatterViewModel Formatter { get; }
    public SettingsViewModel Settings { get; } = new();

    public object? CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public bool IsSidebarOpen
    {
        get => _isSidebarOpen;
        set => this.RaiseAndSetIfChanged(ref _isSidebarOpen, value);
    }

    public ReactiveCommand<Unit, Unit> ShowFormatterCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowSettingsCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleSidebarCommand { get; }

    public MainWindowViewModel()
    {
        Formatter = new FormatterViewModel(new FormatterService());
        CurrentView = Formatter;

        ShowFormatterCommand = ReactiveCommand.Create<Unit, Unit>(_ => { CurrentView = Formatter; return Unit.Default; });
        ShowSettingsCommand = ReactiveCommand.Create<Unit, Unit>(_ => { CurrentView = Settings; return Unit.Default; });
        ToggleSidebarCommand  = ReactiveCommand.Create<Unit, Unit>(_ => { IsSidebarOpen = !IsSidebarOpen; return Unit.Default; });
    }
}
