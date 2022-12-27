using Imlight.Core.Services.Handlers;
using Imlight.Core.Services.Network.Packets;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Network.Events;

// ReSharper disable once SuggestBaseTypeForParameter

public class DefaultSnifferEvents : UsbSnifferEvents
{
    private readonly ILogger<DefaultSnifferEvents> _logger;
    private readonly IEnumerable<IPacketHandler> _packetHandlers;

    public DefaultSnifferEvents(
        ILogger<DefaultSnifferEvents> logger,
        IEnumerable<IPacketHandler> packetHandlers)
    {
        _logger = logger;
        _packetHandlers = packetHandlers;
    }
    
    public override void OnReadPacket(UsbPacket packet)
    {
        LogPacketInfo(packet);

        if (packet.Data.Length == 0) return;

        foreach (var handler in _packetHandlers)
        {
            handler.Handle(packet);
        }
    }

    private void LogPacketInfo(UsbPacket packet)
    {
        var bytesString = string.Join(" ", packet.Data.ToArray());
        _logger.LogDebug("Captured '{data}'", bytesString);
    }
}