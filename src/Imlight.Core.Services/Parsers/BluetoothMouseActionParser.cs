using Imlight.Core.Services.Constants;
using Imlight.Core.Services.Network.Packets;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Parsers;

public class BluetoothMouseActionParser : IUsbPacketActionParser
{
    private static readonly byte[] StartsClickCompareData = { 0x01, 0x06, 0x01, 0x00 };
    private static readonly byte[] StartsMoveCompareData = { 0x01, 0x07, 0x01 };

    public PacketAction GetFromPacket(UsbPacket packet)
    {
        if (StartsMoveCompareData.SequenceEqual(packet.Data.Take(StartsMoveCompareData.Length))) {
            return PacketAction.Move;
        }
        if (StartsClickCompareData.SequenceEqual(packet.Data.Take(StartsClickCompareData.Length))) {
            return PacketAction.Click;
        }
        return PacketAction.Undefined;
    }
}