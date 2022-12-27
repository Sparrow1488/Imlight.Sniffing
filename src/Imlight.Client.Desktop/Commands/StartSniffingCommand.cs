using Imlight.Core.Services.Network.Sniffers.Abstractions;

namespace Imlight.Client.Desktop.Commands;

public class StartSniffingCommand : Command
{
    private readonly IUsbSniffer _sniffer;

    public StartSniffingCommand(IUsbSniffer sniffer)
    {
        _sniffer = sniffer;
    }

    public override void Execute()
    {
        _sniffer.StartCaptureAsync();
    }
}
