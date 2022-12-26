using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace Imlight.Core.Services.Utility;

[SuppressMessage("Interoperability", "CA1416")]
public static class WindowsHelper
{
    public static bool IsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}