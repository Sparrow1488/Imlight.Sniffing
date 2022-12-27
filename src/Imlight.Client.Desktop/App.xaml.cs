using System;
using System.Windows;
using Imlight.Client.Desktop.Commands;
using Imlight.Client.Desktop.Sniffing.Events;
using Imlight.Client.Desktop.ViewModels;
using Imlight.Client.Desktop.Windows;
using Imlight.Core.Services.Handlers;
using Imlight.Core.Services.Network.Contexts;
using Imlight.Core.Services.Network.Events;
using Imlight.Core.Services.Network.Sniffers;
using Imlight.Core.Services.Network.Sniffers.Abstractions;
using Imlight.Core.Services.Network.Sniffers.Options;
using Imlight.Core.Services.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using USBPcapLib;

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
                
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<SnifferViewModel>();
                
                services.AddSingleton<StartSniffingCommand>();
                services.AddSingleton<StopSniffingCommand>();

                #region UsbPcap services

                services.AddSingleton<SnifferContext>();
                
                services.AddSingleton<IUsbSniffer, UsbSniffer>();
                services.AddSingleton<UsbSnifferEvents, DefaultSnifferEvents>();
                services.AddLogging(builder => 
                    builder.AddConsole().SetMinimumLevel(LogLevel.Debug));

                services.AddOptions<UsbSnifferConfig>();
                services.Configure<UsbSnifferConfig>(opt =>
                {
                    // TODO: as model
                    opt.Filter = USBPcapClient.find_usbpcap_filters()[1];
                    opt.DeviceIdFilter = 2;
                });
        
                services.AddSingleton<IPacketHandler, SharedPacketHandler>();
                services.AddSingleton<IUsbPacketParser, BluetoothMouseParser>();

                #endregion
                
            })
            .Build();
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();
        MainWindow = GetWindow<MainWindow, MainViewModel>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    private TWindow GetWindow<TWindow, TVm>()
        where TVm : ViewModelsBase
        where TWindow : Window
    {
        var window = _host.Services.GetRequiredService<TWindow>();
        window.DataContext = _host.Services.GetRequiredService<TVm>();
        return window;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync().Wait();
        _host.Dispose();

        base.OnExit(e);
    }
}
