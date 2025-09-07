using FluentAssertions;
using OracleSQL.Formatter.UI.ViewModels;
using Xunit;
using System.Reactive;
using System;

namespace OracleSQL.Formatter.UI.Tests.ViewModels;

public class MainWindowViewModelTests
{
    private sealed class UnitObserver : IObserver<Unit>
    {
        public void OnCompleted() { }
        public void OnError(Exception error) => throw error;
        public void OnNext(Unit value) { }
    }

    [Fact]
    public void Default_View_Is_Formatter()
    {
        var vm = new MainWindowViewModel();
        vm.CurrentView.Should().Be(vm.Formatter);
    }

    [Fact]
    public void Show_Settings_Command_Switches_View()
    {
        var vm = new MainWindowViewModel();
        vm.ShowSettingsCommand.Execute().Subscribe(new UnitObserver());
        vm.CurrentView.Should().Be(vm.Settings);
    }

    [Fact]
    public void Show_Formatter_Command_Switches_Back()
    {
        var vm = new MainWindowViewModel();
        vm.ShowSettingsCommand.Execute().Subscribe(new UnitObserver());
        vm.ShowFormatterCommand.Execute().Subscribe(new UnitObserver());
        vm.CurrentView.Should().Be(vm.Formatter);
    }
}
