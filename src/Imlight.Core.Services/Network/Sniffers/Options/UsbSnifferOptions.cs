using Microsoft.Extensions.Options;

namespace Imlight.Core.Services.Network.Sniffers.Options;

public class UsbSnifferOptions : IOptions<UsbSnifferConfig>
{
    public UsbSnifferConfig Value { get; } = new();
}