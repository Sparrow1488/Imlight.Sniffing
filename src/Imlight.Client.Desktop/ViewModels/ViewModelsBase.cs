using System.ComponentModel;

namespace Imlight.Client.Desktop.ViewModels;

public abstract class ViewModelsBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void SetProperty(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
