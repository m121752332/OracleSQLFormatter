using System;
using FluentAssertions;
using OracleSQL.Formatter.UI.ViewModels;
using OracleSQL.Formatter.Core.Services;
using OracleSQL.Formatter.Core.Models;
using Xunit;
using System.Reactive;

namespace OracleSQL.Formatter.UI.Tests.ViewModels;

public class FormatterViewModelTests
{
    private class SimpleObserver<T> : IObserver<T>
    {
        public void OnCompleted() { }
        public void OnError(Exception error) => throw error;
        public void OnNext(T value) { }
    }

    [Fact]
    public void FormatCommand_Formats_Input_To_Output()
    {
        var vm = new FormatterViewModel(new FormatterService());
        vm.Input = "select 1";
        vm.Options.RemoveExistingNewLines = true;
        vm.FormatCommand.Execute().Subscribe(new SimpleObserver<Unit>());
        vm.Output.Should().Contain("SELECT");
    }

    [Fact]
    public void ClearCommand_Clears_Input_And_Output()
    {
        var vm = new FormatterViewModel(new FormatterService());
        vm.Input = "select 1";
        vm.Options.RemoveExistingNewLines = true;
        vm.FormatCommand.Execute().Subscribe(new SimpleObserver<Unit>());
        vm.ClearCommand.Execute().Subscribe(new SimpleObserver<Unit>());
        vm.Input.Should().BeEmpty();
        vm.Output.Should().BeEmpty();
    }
}
