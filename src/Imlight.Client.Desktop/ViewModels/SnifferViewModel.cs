using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Imlight.Client.Desktop.Commands;
using Imlight.Client.Desktop.Models;
using Imlight.Core.Services.Constants;
using Imlight.Core.Services.Network.Contexts;
using Imlight.Core.Services.Network.Packets;
using Imlight.Core.Services.Network.Sniffers.Options;
using Imlight.Core.Services.Utility;
using Microsoft.Extensions.Options;
using USBPcapLib;

// ReSharper disable always MemberCanBePrivate.Global

namespace Imlight.Client.Desktop.ViewModels;

public class SnifferViewModel : ViewModelsBase
{
    private string? _interactivity;
    
    public SnifferViewModel(
        SnifferContext context,
        StopSniffingCommand stopSniffingCommand,
        StartSniffingCommand startSniffingCommand,
        IOptions<UsbSnifferConfig> snifferOptions)
    {
        #region Commands initialization

        StartSniffingCommand = startSniffingCommand;
        StopSniffingCommand = stopSniffingCommand;

        #endregion
        
        #region Interactivity initialization

        Filters = new ObservableCollection<UsbPcapFilter>();
        foreach (var filter in USBPcapClient.find_usbpcap_filters()) {
            Filters.Add(new UsbPcapFilter {
                Name = filter
            });
        }

        Interactivity = string.Join("\n", Filters.Select(x => USBPcapClient.enumerate_print_usbpcap_interactive(x.Name)));

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

        #region Packets initialization

        Packets = new ObservableCollection<UsbPacketModel>();
        PacketsBuffer = new List<UsbPacketModel>();

        #endregion
        
        #region Sniffer context initialization

        context.PropertyChanged += (_, args) => {
            if (args.PropertyName == nameof(context.Packet))
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    var usbPacket = context.Packet as UsbPacket;
                    PacketsBuffer.Add(new UsbPacketModel
                    {
                        Id = context?.Packet?.Id ?? -1,
                        Action = usbPacket?.DeviceAction ?? DeviceAction.Undefined,
                        Bytes = context?.Packet?.Data ?? Array.Empty<byte>()
                    });

                    if (PacketsBuffer.Count >= 50)
                    {
                        foreach (var packet in PacketsBuffer)
                            Packets?.Add(packet);
                        PacketsBuffer.RemoveAll(x => true);
                    }
                });
            }
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
    public ObservableCollection<UsbPacketModel> Packets { get; }
    public List<UsbPacketModel> PacketsBuffer { get; }

    #endregion
    
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