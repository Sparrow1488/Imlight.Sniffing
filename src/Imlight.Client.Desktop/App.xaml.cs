using System.Windows;
using Imlight.Client.Desktop.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Imlight.Client.Desktop;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();
        MainWindow = _host.Services.GetRequiredService<MainWindow>();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}
