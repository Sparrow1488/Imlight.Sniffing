using Imlight.Core.Services.Network.Events;

namespace Imlight.Core.Services.Network.Sniffers.Abstractions;

public interface IUsbSniffer : ISniffer
{
    UsbSnifferEvents Events { get; set; }
}