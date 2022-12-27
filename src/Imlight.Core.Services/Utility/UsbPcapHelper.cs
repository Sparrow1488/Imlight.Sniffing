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
            for (var i = 0; i < interactiveInfoLines.Length; i++)
            {
                var line = interactiveInfoLines[i];

                var interactive = new UsbPcapInteractive();

                var fixedLine = line.Trim();

                if (i == 0)
                {
                    model.Path = fixedLine;
                    continue;
                }

                if (fixedLine.StartsWith("[Port"))
                {
                    var portChars = fixedLine.SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit);
                    var portString = string.Join(string.Empty, portChars);
                    interactive.DevicePort = int.Parse(portString);

                    fixedLine = string.Join("", fixedLine.SkipWhile(x => x != '(')).Trim();
                    var deviceIdString = string.Join("",
                        fixedLine.SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit));
                    interactive.DeviceId = int.Parse(deviceIdString);
                }

                interactiveList.Add(interactive);
            }

            model.Interactives = interactiveList;
            modelsList.Add(model);
        }
        return modelsList;
    }
}