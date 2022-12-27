using System;
using System.ComponentModel;
using Imlight.Core.Services.Network.Events;
using Imlight.Core.Services.Network.Packets;

namespace Imlight.Client.Desktop.Sniffing.Events;

public class WpfSnifferEvents : UsbSnifferEvents, INotifyPropertyChanged
{
    private string _packetData = string.Empty;
    
    public string PacketData
    {
        get => _packetData;
        set
        {
            OnPropertyChanged(nameof(PacketData));
            _packetData = value;
        }
    }
    
    public override void OnReadPacket(UsbPacket packet)
    {
        PacketData = string.Join(" ", packet.Data);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}