using Imlight.Core.Services.Constants;
using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Parsers;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Parsers;

public class ImlightButtonParser : IUsbPacketParser
{
    private static readonly byte[] ClickCompareData = { 0xA5, 0x5A, 0xEE };

    public DeviceAction GetAction(UsbPacket packet)
    {
        var filledCaptureData = packet.Data.Where(x => x > 0).ToArray();
        return ClickCompareData.SequenceEqual(filledCaptureData) ? DeviceAction.Click : DeviceAction.Undefined;
    }
}