using Imlight.Core.Services.Handlers;
using Imlight.Core.Services.Network.Events;
using Imlight.Core.Services.Network.Sniffers;
using Imlight.Core.Services.Network.Sniffers.Abstractions;
using Imlight.Core.Services.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

const string ENVIRONMENT_NAME = "Imlight.Client.Desktop";

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IUsbSniffer, UsbSniffer>();
        services.AddSingleton<UsbSnifferEvents, DefaultSnifferEvents>();
        services.AddLogging(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
        
        // Handlers
        services.AddSingleton<IPacketHandler, SharedPacketHandler>();
        
        // Action parsers
        // services.AddSingleton<IUsbPacketActionParser, ImlightButtonActionParser>();
        services.AddSingleton<IUsbPacketActionParser, BluetoothMouseActionParser>();
    }).Build();

var services = host.Services;
var logger = services.GetRequiredService<ILogger<Program>>();

logger.LogInformation(ENVIRONMENT_NAME + " started");

var sniffer = services.GetRequiredService<IUsbSniffer>();
sniffer.Events = services.GetRequiredService<UsbSnifferEvents>();

sniffer.StartCaptureAsync();

while (true)
{
    await Task.Delay(50);
}