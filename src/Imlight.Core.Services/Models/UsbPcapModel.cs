namespace Imlight.Core.Services.Models;

public class UsbPcapModel
{
    public string Path { get; set; }
    public ICollection<UsbPcapInteractive> Interactives { get; set; }
}