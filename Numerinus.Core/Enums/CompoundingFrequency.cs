namespace Numerinus.Core.Enums;

/// <summary>
/// Defines how frequently interest is compounded per year.
/// </summary>
public enum CompoundingFrequency
{
    /// <summary>Compounded once per year.</summary>
    Annually = 1,

    /// <summary>Compounded twice per year.</summary>
    SemiAnnually = 2,

    /// <summary>Compounded four times per year.</summary>
    Quarterly = 4,

    /// <summary>Compounded twelve times per year.</summary>
    Monthly = 12,

    /// <summary>Compounded fifty-two times per year.</summary>
    Weekly = 52,

    /// <summary>Compounded three hundred and sixty-five times per year.</summary>
    Daily = 365
}
