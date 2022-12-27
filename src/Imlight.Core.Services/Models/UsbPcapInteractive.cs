namespace Imlight.Core.Services.Models;

public struct UsbPcapInteractive
{
    public int DeviceId { get; set; }
    public int DevicePort { get; set; }
    public string Filter { get; set; }
    public IEnumerable<string> Devices { get; set; }
}