namespace Imlight.Core.Services.Network.Packets;

public abstract class Packet
{
    public Packet(object sender, byte[] data)
    {
        Sender = sender;
        Data = data;
    }
    
    public object Sender { get; }
    public byte[] Data { get; }
}