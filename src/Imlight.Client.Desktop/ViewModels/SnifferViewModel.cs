using System;
using System.Collections.ObjectModel;
using System.Linq;
using Imlight.Client.Desktop.Commands;
using Imlight.Client.Desktop.Models;
using Imlight.Core.Services.Network.Contexts;
using Imlight.Core.Services.Network.Sniffers.Options;
using Imlight.Core.Services.Utility;
using Microsoft.Extensions.Options;
using USBPcapLib;

// ReSharper disable always MemberCanBePrivate.Global

namespace Imlight.Client.Desktop.ViewModels;

public class SnifferViewModel : ViewModelsBase
{
    private string? _readPacketMessage;
    private string? _interactivity;
    
    public SnifferViewModel(
        StartSniffingCommand startSniffingCommand,
        StopSniffingCommand stopSniffingCommand,
        SnifferContext context,
        IOptions<UsbSnifferConfig> snifferOptions)
    {
        #region Commands initialization

        StartSniffingCommand = startSniffingCommand;
        StopSniffingCommand = stopSniffingCommand;

        #endregion
        
        #region Interactivity initialization

        Interactivity = USBPcapClient.enumerate_print_usbpcap_interactive(snifferOptions.Value.Filter);
        Filters = new ObservableCollection<UsbPcapFilter>();
        foreach (var filter in USBPcapClient.find_usbpcap_filters()) {
            Filters.Add(new UsbPcapFilter {
                Name = filter
            });
        }

        #endregion

        #region Filters initialization

        foreach (var pcapFilter in Filters) {
            pcapFilter.PropertyChanged += (_, args) => {
                if (args.PropertyName == nameof(pcapFilter.IsSelected))
                    snifferOptions.Value.Filter = pcapFilter.Name;
            };
        }

        #endregion

        #region Devices initialization

        var models = UsbPcapHelper.GetAvailableModels();
        var availableDevices = models
            .SelectMany(x => x.Interactives)
            .Select(x => x.DeviceId)
            .Where(x => x != 0)
            .Distinct();
        Devices = new ObservableCollection<UsbPcapDeviceId>(
            availableDevices.Select(x => new UsbPcapDeviceId{
                Value = x
            }));
        foreach (var device in Devices) {
            device.PropertyChanged += (_, args) => {
                if (args.PropertyName == nameof(device.IsSelected)) {
                    snifferOptions.Value.DeviceIdFilter = device.Value;
                }
            };
        }

        #endregion

        #region Sniffer context initialization

        context.PropertyChanged += (_, args) => {
            if (args.PropertyName == nameof(context.Packet))
                ReadPacketMessage += string.Join(" ", context?.Packet?.Data ?? ArraySegment<byte>.Empty) + "\n";
        };

        #endregion
    }

    #region Commands

    public StartSniffingCommand StartSniffingCommand { get; }
    public StopSniffingCommand StopSniffingCommand { get; }

    #endregion

    #region Collections

    public ObservableCollection<UsbPcapDeviceId> Devices { get; }
    public ObservableCollection<UsbPcapFilter> Filters { get; }

    #endregion
    
    public string? ReadPacketMessage
    {
        get => _readPacketMessage;
        set
        {
            SetProperty(nameof(ReadPacketMessage));
            _readPacketMessage = value;
        }
    }
    public string? Interactivity
    {
        get => _interactivity;
        set
        {
            SetProperty(nameof(Interactivity));
            _interactivity = value;
        }
    }
}