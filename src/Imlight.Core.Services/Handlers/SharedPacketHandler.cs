using Imlight.Core.Services.Network.Contexts;
using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Parsers;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Handlers;

public class SharedPacketHandler : PacketHandler
{
    private readonly IUsbPacketParser _parser;
    private readonly SnifferContext _context;

    public SharedPacketHandler(
        ILogger<PacketHandler> logger,
        IUsbPacketParser parser,
        SnifferContext context) 
    : base(logger)
    {
        _parser = parser;
        _context = context;
    }

    public override void Handle(Packet packet)
    {
        _context.Packet = packet;
        
        if (packet is not UsbPacket usbPacket) return;
        
        usbPacket.DeviceAction = _parser.GetAction(usbPacket);
        Logger.LogInformation("Device action is '{action}'", usbPacket.DeviceAction);
    }
}