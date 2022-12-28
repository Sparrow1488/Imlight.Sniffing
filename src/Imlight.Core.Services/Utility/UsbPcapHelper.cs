using Imlight.Core.Services.Models;
using USBPcapLib;

// ReSharper disable once CollectionNeverQueried.Local

namespace Imlight.Core.Services.Utility;

public static class UsbPcapHelper
{
    public static IEnumerable<UsbPcapModel> GetAvailableModels()
    {
        var modelsList = new List<UsbPcapModel>();

        var filters = USBPcapClient.find_usbpcap_filters();
        foreach (var filter in filters)
        {
            var interactiveList = new List<UsbPcapInteractive>();
            var model = new UsbPcapModel();

            var interactiveInfo = USBPcapClient.enumerate_print_usbpcap_interactive(filter);
            var interactiveInfoLines = interactiveInfo.Split("\n");

            var currentPort = 0;
            var currentDeviceId = 0;
            for (var i = 0; i < interactiveInfoLines.Length; i++)
            {
                var line = interactiveInfoLines[i];

                var interactive = new UsbPcapInteractive();

                var fixedLine = line.Trim();

                #region Path detect

                if (i == 0)
                {
                    model.Path = fixedLine;
                    continue;
                }

                #endregion

                #region Port & DeviceId detect

                if (fixedLine.StartsWith("[Port"))
                {
                    var portChars = fixedLine.SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit);
                    var portString = string.Join(string.Empty, portChars);
                    currentPort = int.Parse(portString);

                    fixedLine = string.Join("", fixedLine.SkipWhile(x => x != '(')).Trim();
                    var deviceIdString = string.Join("",
                        fixedLine.SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit));
                    currentDeviceId = int.Parse(deviceIdString);
                }

                #endregion

                var devices = new Dictionary<string, List<string>>();
                // Group of devices
                if (line.StartsWith("      ") && char.IsLetterOrDigit(line[7]))
                {
                    var devicesNames = interactiveInfoLines
                    .Skip(i)
                    .SkipWhile(x => x != line).Skip(1)
                    .TakeWhile(x => x.StartsWith("        "))
                    .Select(x => x.Trim());
                    
                    var fixedDevicesGroup = line.Trim();
                    
                    if (!devices.ContainsKey(fixedDevicesGroup))
                        devices.Add(fixedDevicesGroup, devicesNames.ToList());
                    else
                    {
                        var keys = devices.Keys.Where(x => x.StartsWith(fixedDevicesGroup));
                        var indexedDeviceGroup = fixedDevicesGroup + $" ({keys.Count() + 1})";
                        devices.Add(indexedDeviceGroup, devicesNames.ToList());
                    }
                }

                interactive.DeviceId = currentDeviceId;
                interactive.DevicePort = currentPort;
                interactive.Devices = devices;
                interactiveList.Add(interactive);
            }

            model.Filter = filter;
            model.Interactives = interactiveList.Where(x => x.Devices.Any()).ToList();
            interactiveList.ForEach(x => x.ParentModel = model);
            
            modelsList.Add(model);
        }

        return modelsList;
    }
}