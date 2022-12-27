using Imlight.Core.Services.Network.Events;
using Imlight.Core.Services.Network.Sniffers.Abstractions;

// ReSharper disable once InvertIf

namespace Imlight.Client.Desktop.Commands;

public class StartSniffingCommand : Command
{
    private readonly IUsbSniffer _sniffer;
    private readonly UsbSnifferEvents _snifferEvents;

    public StartSniffingCommand(IUsbSniffer sniffer, UsbSnifferEvents snifferEvents)
    {
        _sniffer = sniffer;
        _snifferEvents = snifferEvents;
    }

    public override void Execute()
    {
        if (!_sniffer.IsStarted())
        {
            _sniffer.Events = _snifferEvents;
            _sniffer.StartCaptureAsync();
        }
    }
}
