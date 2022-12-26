using Imlight.Core.Services.Network.Packets;

namespace Imlight.Core.Services.Handlers;

public interface IPacketHandler
{
    void Handle(Packet packet);
}