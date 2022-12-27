using Imlight.Client.Desktop.Commands;

namespace Imlight.Client.Desktop.ViewModels;

public class SnifferViewModel : ViewModelsBase
{
    public SnifferViewModel(StartSniffingCommand sniffingCommand)
    {
        SniffingCommand = sniffingCommand;
    }

    public StartSniffingCommand SniffingCommand { get; }
}