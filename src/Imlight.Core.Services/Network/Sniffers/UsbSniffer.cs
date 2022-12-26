using Imlight.Core.Services.Network.Events;
using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Network.Sniffers.Abstractions;
using USBPcapLib;

namespace Imlight.Core.Services.Network.Sniffers;

#pragma warning disable CS8766

public sealed class UsbSniffer : IUsbSniffer
{
    private readonly USBPcapClient _client;

    public UsbSniffer()
    {
        var pcapInterface = USBPcapClient.find_usbpcap_filters().First();
        _client = new USBPcapClient(pcapInterface, 2); // TODO: add check for admin mode is enabled
    }
    
    public UsbSnifferEvents? Events { get; set; }
    
    public void StartCaptureAsync()
    {
        _client.DataRead += (sender, eventArgs) =>
        {
            if (eventArgs is null) return;
            
            var packet = new UsbPacket(sender ?? new object(), eventArgs.Data);
            Events?.OnReadPacket(packet);
        };

        Task.Run(() =>
        {
            _client.start_capture();
            _client.wait_for_exit_signal();
        });
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
