using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Parsers;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Handlers;

public class SharedPacketHandler : PacketHandler
{
    private readonly IUsbPacketParser _parser;

    public SharedPacketHandler(
        ILogger<PacketHandler> logger,
        IUsbPacketParser parser) 
    : base(logger)
    {
        _parser = parser;
    }

    public override void Handle(Packet packet)
    {
        if (packet is not UsbPacket usbPacket) return;
        
        var action = _parser.GetAction(usbPacket);
        Logger.LogInformation("Device action is '{action}'", action);
    }
}