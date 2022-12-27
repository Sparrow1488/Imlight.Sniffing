using System.ComponentModel;

namespace Imlight.Client.Desktop.Models;

public sealed class UsbPcapFilter : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private bool _isSelected;
    
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            OnPropertyChanged(nameof(IsSelected));
            _isSelected = value;
        }
    }
    public string Name { get; set; } = string.Empty;

    private void OnPropertyChanged(string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}