using System;
using Imlight.Core.Services.Constants;

namespace Imlight.Client.Desktop.Models;

public class UsbPacketModel
{
    public int Id { get; set; }
    public DeviceAction Action { get; set; }
    public byte[] Bytes { get; set; }
    public string Message => $"[id:{Id}][action:{Action}][data:{string.Join(string.Empty, Bytes)}]";
}