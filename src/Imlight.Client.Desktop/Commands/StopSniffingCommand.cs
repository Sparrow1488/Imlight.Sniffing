using Imlight.Core.Services.Network.Sniffers.Abstractions;

namespace Imlight.Client.Desktop.Commands;

public class StopSniffingCommand : Command
{
    private readonly IUsbSniffer _sniffer;

    public StopSniffingCommand(IUsbSniffer sniffer)
    {
        _sniffer = sniffer;
    }
    
    public override void Execute()
    {
        if (_sniffer.IsStarted())
        {
            _sniffer.StopCapture();
        }
    }
}