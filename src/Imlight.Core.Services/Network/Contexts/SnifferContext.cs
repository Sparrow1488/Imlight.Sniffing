using System.ComponentModel;
using System.Runtime.CompilerServices;
using Imlight.Core.Services.Network.Packets;

namespace Imlight.Core.Services.Network.Contexts;

public sealed class SnifferContext : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private Packet? _packet;

    public Packet? Packet
    {
        get => _packet;
        set
        {
            if (value is not null)
            {
                OnPropertyChanged(nameof(Packet));
            }
            _packet = value;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}