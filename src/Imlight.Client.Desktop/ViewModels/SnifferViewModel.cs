using System;
using Imlight.Client.Desktop.Commands;
using Imlight.Core.Services.Network.Contexts;

namespace Imlight.Client.Desktop.ViewModels;

public class SnifferViewModel : ViewModelsBase
{
    private string? _readPacketMessage;
    
    public SnifferViewModel(
        StartSniffingCommand startSniffingCommand,
        StopSniffingCommand stopSniffingCommand,
        SnifferContext context)
    {
        StartSniffingCommand = startSniffingCommand;
        StopSniffingCommand = stopSniffingCommand;

        context.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(context.Packet))
                ReadPacketMessage += string.Join(" ", context?.Packet?.Data ?? ArraySegment<byte>.Empty) + "\n";
        };
    }

    public StartSniffingCommand StartSniffingCommand { get; }
    public StopSniffingCommand StopSniffingCommand { get; }

    public string? ReadPacketMessage
    {
        get => _readPacketMessage;
        set
        {
            SetProperty(nameof(ReadPacketMessage));
            _readPacketMessage = value;
        }
    }
}