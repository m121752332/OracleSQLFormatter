using System.Reactive;
using ReactiveUI;
using OracleSQL.Formatter.Core.Models;
using OracleSQL.Formatter.Core.Services;
using OracleSQL.Formatter.Common.Enums;
using OracleSQL.Formatter.UI.Localization;
using OracleSQL.Formatter.UI.Services;

namespace OracleSQL.Formatter.UI.ViewModels;

public class FormatterViewModel : ViewModelBase
{
    private string _input = string.Empty;
    private string _output = string.Empty;
    private readonly IFormatterService _service;

    public FormatterOptions Options { get; } = new();

    public CaseOption[] CaseOptions { get; } = System.Enum.GetValues<CaseOption>();

    // String-based options, stored as English keys (persisted)
    public string[] CommaOptions { get; } = new[] { "After", "Before", "Before with space" };
    public string[] ListStyleOptions { get; } = new[] { "Stacked", "Not Stacked" };
    public string[] AlignOptions { get; } = new[] { "Align left", "Align right" };

    public string Input
    {
        get => _input;
        set => this.RaiseAndSetIfChanged(ref _input, value);
    }

    public string Output
    {
        get => _output;
        private set => this.RaiseAndSetIfChanged(ref _output, value);
    }

    public ReactiveCommand<Unit, Unit> FormatCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    public ReactiveCommand<Unit, Unit> CopyCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearInputCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearOutputCommand { get; }

    public FormatterViewModel(IFormatterService? service = null)
    {
        _service = service ?? new FormatterService();
        FormatCommand = ReactiveUI.ReactiveCommand.Create(Format);
        ClearCommand = ReactiveUI.ReactiveCommand.Create(Clear);
        CopyCommand = ReactiveUI.ReactiveCommand.Create(Copy);
        ClearInputCommand = ReactiveUI.ReactiveCommand.Create(ClearInput);
        ClearOutputCommand = ReactiveUI.ReactiveCommand.Create(ClearOutput);

        // 當語系改變時，通知選項集合重新評估（以刷新顯示文字）
        Localizer.LanguageChanged += () =>
        {
            this.RaisePropertyChanged(nameof(CaseOptions));
            this.RaisePropertyChanged(nameof(CommaOptions));
            this.RaisePropertyChanged(nameof(ListStyleOptions));
            this.RaisePropertyChanged(nameof(AlignOptions));
        };

        // Try load saved formatter options
        var saved = JsonStorage.Load<FormatterOptions>(AppPaths.FormatterOptionsPath);
        if (saved is not null)
        {
            CopyOptions(saved, Options);
        }

        // Save whenever any option changes
        Options.PropertyChanged += (_, __) => SaveOptions();
    }

    private static void CopyOptions(FormatterOptions from, FormatterOptions to)
    {
        to.KeywordCase = from.KeywordCase;
        to.TableCase = from.TableCase;
        to.ColumnCase = from.ColumnCase;
        to.FunctionCase = from.FunctionCase;
        to.DataTypeCase = from.DataTypeCase;
        to.VariableCase = from.VariableCase;
        to.AliasCase = from.AliasCase;
        to.QuotedIdentifierCase = from.QuotedIdentifierCase;
        to.OtherIdentifierCase = from.OtherIdentifierCase;
        to.MaxLineLength = from.MaxLineLength;
        to.CommaPosition = from.CommaPosition;
        to.ListStyle = from.ListStyle;
        to.Align = from.Align;
        to.AndOrUnderWhere = from.AndOrUnderWhere;
        to.RemoveExistingNewLines = from.RemoveExistingNewLines;
        to.TrimQuotesEachLine = from.TrimQuotesEachLine;
        to.CompactMode = from.CompactMode;
        to.CompactModeMaxLineLength = from.CompactModeMaxLineLength;
    }

    private void SaveOptions() => JsonStorage.Save(AppPaths.FormatterOptionsPath, Options);

    private void Format()
    {
        var result = _service.Format(Input, Options);
        Output = result.Formatted;
    }

    private void Clear()
    {
        Input = string.Empty;
        Output = string.Empty;
    }

    private void ClearInput() => Input = string.Empty;
    private void ClearOutput() => Output = string.Empty;

    private void Copy()
    {
        // For desktop app this would copy to clipboard. Left empty for testability.
    }
}
