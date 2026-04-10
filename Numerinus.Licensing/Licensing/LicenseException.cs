namespace Numerinus.UnitConversion.Licensing;

/// <summary>
/// Thrown when a Numerinus package method is called without a valid active license.
/// </summary>
public sealed class LicenseException : Exception
{
    public LicenseException(string message) : base(message) { }
}
