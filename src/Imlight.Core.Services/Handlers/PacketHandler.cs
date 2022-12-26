using Imlight.Core.Services.Network.Packets;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Handlers;

public abstract class PacketHandler : IPacketHandler
{
    public PacketHandler(ILogger<PacketHandler> logger)
    {
        Logger = logger;
    }
    
    protected ILogger<PacketHandler> Logger { get; }
    
    public abstract void Handle(Packet packet);
}