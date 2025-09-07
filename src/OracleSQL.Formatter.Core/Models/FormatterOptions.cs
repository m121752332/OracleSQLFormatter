using System.ComponentModel;
using System.Runtime.CompilerServices;
using OracleSQL.Formatter.Common.Enums;

namespace OracleSQL.Formatter.Core.Models;

public class FormatterOptions : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (Equals(field, value)) return false;
        field = value!;
        OnPropertyChanged(name);
        return true;
    }

    private CaseOption _keywordCase = CaseOption.Uppercase;
    public CaseOption KeywordCase { get => _keywordCase; set => SetField(ref _keywordCase, value); }

    private CaseOption _tableCase = CaseOption.Unchanged;
    public CaseOption TableCase { get => _tableCase; set => SetField(ref _tableCase, value); }

    private CaseOption _columnCase = CaseOption.Unchanged;
    public CaseOption ColumnCase { get => _columnCase; set => SetField(ref _columnCase, value); }

    private CaseOption _functionCase = CaseOption.InitCap;
    public CaseOption FunctionCase { get => _functionCase; set => SetField(ref _functionCase, value); }

    private CaseOption _dataTypeCase = CaseOption.Uppercase;
    public CaseOption DataTypeCase { get => _dataTypeCase; set => SetField(ref _dataTypeCase, value); }

    private CaseOption _variableCase = CaseOption.Unchanged;
    public CaseOption VariableCase { get => _variableCase; set => SetField(ref _variableCase, value); }

    private CaseOption _aliasCase = CaseOption.Unchanged;
    public CaseOption AliasCase { get => _aliasCase; set => SetField(ref _aliasCase, value); }

    private CaseOption _quotedIdentifierCase = CaseOption.Unchanged;
    public CaseOption QuotedIdentifierCase { get => _quotedIdentifierCase; set => SetField(ref _quotedIdentifierCase, value); }

    private CaseOption _otherIdentifierCase = CaseOption.Unchanged;
    public CaseOption OtherIdentifierCase { get => _otherIdentifierCase; set => SetField(ref _otherIdentifierCase, value); }

    private int _maxLineLength = 80;
    public int MaxLineLength { get => _maxLineLength; set => SetField(ref _maxLineLength, value); }

    private string _commaPosition = "After"; // After | Before | Before with space
    public string CommaPosition { get => _commaPosition; set => SetField(ref _commaPosition, value); }

    private string _listStyle = "Stacked"; // Stacked | Not Stacked
    public string ListStyle { get => _listStyle; set => SetField(ref _listStyle, value); }

    private string _align = "Align left"; // Align left | Align right
    public string Align { get => _align; set => SetField(ref _align, value); }

    private bool _andOrUnderWhere = true;
    public bool AndOrUnderWhere { get => _andOrUnderWhere; set => SetField(ref _andOrUnderWhere, value); }

    private bool _removeExistingNewLines = true;
    public bool RemoveExistingNewLines { get => _removeExistingNewLines; set => SetField(ref _removeExistingNewLines, value); }

    private bool _trimQuotesEachLine = false;
    public bool TrimQuotesEachLine { get => _trimQuotesEachLine; set => SetField(ref _trimQuotesEachLine, value); }

    private bool _compactMode = false;
    public bool CompactMode { get => _compactMode; set => SetField(ref _compactMode, value); }

    private int _compactModeMaxLineLength = 80;
    public int CompactModeMaxLineLength { get => _compactModeMaxLineLength; set => SetField(ref _compactModeMaxLineLength, value); }
}
