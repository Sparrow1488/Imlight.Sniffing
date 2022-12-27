using System.ComponentModel;

namespace Imlight.Client.Desktop.Models;

public class UsbPcapFilter : INotifyPropertyChanged
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

    protected virtual void OnPropertyChanged(string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}