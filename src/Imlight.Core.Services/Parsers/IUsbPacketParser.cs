using Imlight.Core.Services.Constants;
using Imlight.Core.Services.Network.Packets;

namespace Imlight.Core.Services.Parsers;

public interface IUsbPacketParser
{
    DeviceAction GetAction(UsbPacket packet);
}