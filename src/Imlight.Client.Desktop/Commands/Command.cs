using System;
using System.Windows.Input;

namespace Imlight.Client.Desktop.Commands;

public abstract class Command : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => Execute();

    public abstract void Execute();
}
