using System.ComponentModel;

namespace Imlight.Client.Desktop.Models;

public sealed class UsbPcapDeviceId : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private bool _isSelected;
    
    public int Value { get; set; }
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            OnPropertyChanged(nameof(IsSelected));
            _isSelected = value;
        }
    }

    private void OnPropertyChanged(string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public override string ToString() => Value.ToString();
}