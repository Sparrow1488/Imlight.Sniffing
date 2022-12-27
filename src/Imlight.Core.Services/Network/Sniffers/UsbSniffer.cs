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
    private readonly IOptions<UsbSnifferConfig> _options;

    private bool _isStarted;
    private USBPcapClient? _client;
    private CancellationTokenSource? _captureTaskSource;
    private int _currentPacketId = 0;

    public UsbSniffer(IOptions<UsbSnifferConfig> options)
    {
        _options = options;
        if (!WindowsHelper.IsAdministrator())
        {
            throw new ForbiddenException("You must run this process as Administrator");
        }
    }
    
    public UsbSnifferEvents? Events { get; set; }

    public bool IsStarted() => _isStarted;

    public void StartCaptureAsync()
    {
        InitUsbPcapClient(_options);

        _client!.DataRead += (sender, eventArgs) =>
        {
            if (eventArgs is null) return;
            
            var packet = new UsbPacket(sender ?? new object(), eventArgs.Data)
            {
                Id = _currentPacketId++
            };
            Events?.OnReadPacket(packet);
        };

        _captureTaskSource = new CancellationTokenSource();
        var captureTaskToken = _captureTaskSource.Token;
        
        Task.Run(() =>
        {
            _client.start_capture();
            _isStarted = true;
            _client.wait_for_exit_signal();
        }, captureTaskToken);
    }

    private void InitUsbPcapClient(IOptions<UsbSnifferConfig> options)
    {
        _client = new USBPcapClient(options.Value.Filter, options.Value.DeviceIdFilter);
    }

    public void StopCapture()
    {
        _captureTaskSource?.Cancel();
        _isStarted = false;
        _client?.Dispose();
    }

    public void Dispose()
    {
        try
        {
            StopCapture();
            _client?.Dispose();
        }
        catch { /*ignored*/ }
    }
}
