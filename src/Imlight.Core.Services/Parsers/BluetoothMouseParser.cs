using Imlight.Core.Services.Constants;
using Imlight.Core.Services.Network.Packets;

namespace Imlight.Core.Services.Parsers;

public class BluetoothMouseParser : IUsbPacketParser
{
    private static readonly byte[] StartsClickCompareData = { 0x01, 0x06, 0x01, 0x00 };
    private static readonly byte[] StartsMoveCompareData = { 0x01, 0x07, 0x01 };

    public DeviceAction GetAction(UsbPacket packet)
    {
        if (StartsMoveCompareData.SequenceEqual(packet.Data.Take(StartsMoveCompareData.Length))) {
            return DeviceAction.Move;
        }
        if (StartsClickCompareData.SequenceEqual(packet.Data.Take(StartsClickCompareData.Length))) {
            return DeviceAction.Click;
        }
        return DeviceAction.Undefined;
    }
}