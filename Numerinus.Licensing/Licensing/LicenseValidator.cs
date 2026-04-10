namespace Numerinus.UnitConversion.Licensing;

/// <summary>
/// Holds the result of a successful license validation and manages the local cache.
/// </summary>
internal static class LicenseValidator
{
    private static string?   _licenseKey;
    private static string[]? _unlockedPackages;
    private static DateTime? _expiresAt;
    private static DateTime  _cacheValidUntil = DateTime.MinValue;
    private static readonly object _lock = new();

    internal static void Store(
        string licenseKey,
        string[] unlockedPackages,
        DateTime? expiresAt,
        int cacheTtlSeconds)
    {
        lock (_lock)
        {
            _licenseKey       = licenseKey;
            _unlockedPackages = unlockedPackages;
            _expiresAt        = expiresAt;
            _cacheValidUntil  = DateTime.UtcNow.AddSeconds(cacheTtlSeconds);
        }
    }

    /// <summary>
    /// Ensures the calling package is covered by the active license.
    /// Throws <see cref="LicenseException"/> if not activated, expired, or not unlocked.
    /// </summary>
    internal static void EnsureValid(string requiredPackage)
    {
        lock (_lock)
        {
            if (_licenseKey is null)
                throw new LicenseException(
                    "Numerinus license not activated. Call NumerinusLicense.ActivateAsync() at startup.");

            if (DateTime.UtcNow > _cacheValidUntil)
                throw new LicenseException(
                    "Numerinus license cache has expired. Call NumerinusLicense.ActivateAsync() again.");

            if (_expiresAt.HasValue && DateTime.UtcNow > _expiresAt.Value)
                throw new LicenseException(
                    $"Your Numerinus license expired on {_expiresAt.Value:yyyy-MM-dd}. Renew at numerinus.dev/pricing.");

            if (_unlockedPackages is not null
                && !_unlockedPackages.Contains(requiredPackage, StringComparer.OrdinalIgnoreCase))
                throw new LicenseException(
                    $"Your Numerinus license does not include '{requiredPackage}'. Upgrade at numerinus.dev/pricing.");
        }
    }
}
