﻿using Imlight.Core.Services.Constants;
using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Parsers;
using Microsoft.Extensions.Logging;

namespace Imlight.Core.Services.Parsers;

public class ImlightButtonActionParser : IUsbPacketActionParser
{
    private static readonly byte[] ClickCompareData = { 0xA5, 0x5A, 0xEE };

    public PacketAction GetFromPacket(UsbPacket packet)
    {
        var filledCapturedData = packet.Data.Where(x => x > 0).ToArray();
        return ClickCompareData.SequenceEqual(filledCapturedData) ? PacketAction.Click : PacketAction.Undefined;
    }
}