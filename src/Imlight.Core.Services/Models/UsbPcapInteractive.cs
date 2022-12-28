namespace Imlight.Core.Services.Models;

public class UsbPcapInteractive
{
    public int DeviceId { get; set; }
    public int DevicePort { get; set; }
    public Dictionary<string, List<string>> Devices { get; set; }
    public UsbPcapModel ParentModel { get; set; }
}