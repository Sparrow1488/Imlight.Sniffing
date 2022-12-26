namespace Imlight.Core.Services.Network.Sniffers.Abstractions;

public interface ISniffer : IDisposable
{
    void StartCaptureAsync();
}