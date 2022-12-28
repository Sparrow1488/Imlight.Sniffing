using Imlight.Core.Services.Handlers;
using Imlight.Core.Services.Network.Contexts;
using Imlight.Core.Services.Network.Events;
using Imlight.Core.Services.Network.Sniffers;
using Imlight.Core.Services.Network.Sniffers.Abstractions;
using Imlight.Core.Services.Network.Sniffers.Options;
using Imlight.Core.Services.Parsers;
using Imlight.Core.Services.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using USBPcapLib;

const string ENVIRONMENT_NAME = "Imlight.Client.Desktop";

var models = UsbPcapHelper.GetAvailableModels();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<SnifferContext>();
        services.AddSingleton<IUsbSniffer, UsbSniffer>();
        services.AddSingleton<UsbSnifferEvents, DefaultSnifferEvents>();
        services.AddLogging(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug));

        services.AddOptions<UsbSnifferConfig>();
        services.Configure<UsbSnifferConfig>(opt =>
        {
            opt.Filter = USBPcapClient.find_usbpcap_filters()[0];
            var all = USBPcapClient.enumerate_print_usbpcap_interactive(opt.Filter);
            opt.DeviceIdFilter = 2;
        });
        
        services.AddSingleton<IPacketHandler, SharedPacketHandler>();
        services.AddSingleton<IUsbPacketParser, BluetoothMouseParser>();
    }).Build();

var services = host.Services;
var logger = services.GetRequiredService<ILogger<Program>>();

logger.LogInformation(ENVIRONMENT_NAME + " started");

var sniffer = services.GetRequiredService<IUsbSniffer>();
sniffer.Events = services.GetRequiredService<UsbSnifferEvents>();

sniffer.StartCaptureAsync();

while (true)
{
    await Task.Delay(150);
}