using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Numerinus.UnitConversion.Licensing;

/// <summary>
/// Produces a stable machine fingerprint from hardware/OS identifiers
/// that don't change across reboots or user sessions.
/// </summary>
internal static class MachineHashProvider
{
    private static readonly Lazy<string> _hash = new(Compute);

    public static string Hash => _hash.Value;

    private static string Compute()
    {
        // Combine stable identifiers: OS, architecture, hostname, first MAC address
        var mac = GetFirstMacAddress();
        var raw = $"{Environment.MachineName}|{RuntimeInformation.OSDescription}|{RuntimeInformation.OSArchitecture}|{mac}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string GetFirstMacAddress()
    {
        try
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up
                         && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(n => n.GetPhysicalAddress().ToString())
                .FirstOrDefault(a => !string.IsNullOrWhiteSpace(a) && a != "000000000000")
                ?? "unknown";
        }
        catch
        {
            return "unknown";
        }
    }
}
