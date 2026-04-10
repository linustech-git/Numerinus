using System.Security.Cryptography;
using System.Text;

namespace Numerinus.UnitConversion.Licensing;

/// <summary>
/// Signs license validation requests with the shared HMAC secret.
/// The secret is baked in at build time — never put the real value in source control.
/// In CI, replace NUMERINUS_HMAC_SECRET via a build-time text substitution before publishing.
/// </summary>
internal static class HmacSigner
{
    // ?? Replace this placeholder at publish time via CI secret injection ??????
    // The value must match License:ValidateHmacSecret in the API's appsettings.
    private const string Secret = "NUMERINUS_HMAC_SECRET";

    public static string Sign(string licenseKey, string machineHash)
    {
        var key = Encoding.UTF8.GetBytes(Secret);
        var msg = Encoding.UTF8.GetBytes($"{licenseKey}:{machineHash}");
        return Convert.ToHexString(HMACSHA256.HashData(key, msg)).ToLowerInvariant();
    }
}
