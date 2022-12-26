namespace Imlight.Core.Services.Network.Packets;

public class UsbPacket : Packet
{
    public UsbPacket(object sender, byte[] data) 
    : base(sender, data)
    {
    }
}