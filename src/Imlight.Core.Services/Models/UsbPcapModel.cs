namespace Imlight.Core.Services.Models;

public class UsbPcapModel
{
    public string Filter { get; set; } = string.Empty; // USBPcap{num}
    public string Path { get; set; } = string.Empty;   // \??\USB....
    public ICollection<UsbPcapInteractive> Interactives { get; set; } = ArraySegment<UsbPcapInteractive>.Empty;
}