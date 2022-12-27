using System;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS8618

namespace Imlight.Client.Desktop.ViewModels;

public class MainViewModel : ViewModelsBase
{
    private ViewModelsBase _currentViewModel;

    public MainViewModel(IServiceProvider services)
    {
        CurrentViewModel = services.GetRequiredService<SnifferViewModel>();
    }
    
    public ViewModelsBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            SetProperty(nameof(CurrentViewModel));
            _currentViewModel = value;
        }
    }
}
