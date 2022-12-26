using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Parsers;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Handlers;

public class SharedPacketHandler : PacketHandler
{
    private readonly IUsbPacketActionParser _actionParser;

    public SharedPacketHandler(
        ILogger<PacketHandler> logger,
        IUsbPacketActionParser actionParser) 
    : base(logger)
    {
        _actionParser = actionParser;
    }

    public override void Handle(Packet packet)
    {
        if (packet is not UsbPacket usbPacket) return;
        
        var action = _actionParser.GetFromPacket(usbPacket);
        Logger.LogInformation("Device action is '{action}'", action);
    }
}