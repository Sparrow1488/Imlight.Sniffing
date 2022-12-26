using Imlight.Core.Services.Network.Packets;

namespace Imlight.Core.Services.Network.Events;

public abstract class UsbSnifferEvents
{
    public abstract void OnReadPacket(UsbPacket packet);
}