namespace Imlight.Core.Services.Network.Sniffers.Abstractions;

public interface ISniffer : IDisposable
{
    bool IsStarted();
    void StartCaptureAsync();
    void StopCapture();
}