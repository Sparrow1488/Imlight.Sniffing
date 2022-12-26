using Imlight.Core.Services.Exceptions;
using Imlight.Core.Services.Network.Events;
using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Network.Sniffers.Abstractions;
using Imlight.Core.Services.Network.Sniffers.Options;
using Imlight.Core.Services.Utility;
using Microsoft.Extensions.Options;
using USBPcapLib;

namespace Imlight.Core.Services.Network.Sniffers;

#pragma warning disable CS8766

public sealed class UsbSniffer : IUsbSniffer
{
    private readonly USBPcapClient _client;

    public UsbSniffer(IOptions<UsbSnifferConfig> options)
    {
        if (!WindowsHelper.IsAdministrator())
        {
            throw new ForbiddenException("You must run this process as Administrator");
        }
        
        _client = new USBPcapClient(options.Value.Filter, options.Value.DeviceIdFilter);
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
