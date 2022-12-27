using Imlight.Client.Desktop.ViewModels;
using Imlight.Core.Services.Network.Sniffers.Options;
using Microsoft.Extensions.Options;

namespace Imlight.Client.Desktop.Commands;

public class ApplySnifferSettingsCommand : Command
{
    private readonly IOptions<UsbSnifferConfig> _options;

    public ApplySnifferSettingsCommand(IOptions<UsbSnifferConfig> options)
    {
        _options = options;
    }
    
    public override void Execute()
    {
        _options.Value.Filter = "Huid";
    }
}
