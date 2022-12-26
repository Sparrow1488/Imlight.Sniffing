namespace Imlight.Core.Services.Network.Sniffers.Options;

public class UsbSnifferConfig
{
    public string Filter { get; set; } = string.Empty;
    public int DeviceIdFilter { get; set; } = -1;
}