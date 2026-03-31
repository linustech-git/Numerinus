namespace Numerinus.Core.Enums;

public enum SpeedEnum
{
    // Metric
    MeterPerSecond,       // base unit
    KilometerPerHour,
    MeterPerMinute,
    CentimeterPerSecond,

    // Imperial
    MilePerHour,
    FootPerSecond,
    FootPerMinute,
    InchPerSecond,
    Knot,

    // Scientific
    MachNumber,           // Mach 1 ≈ 343 m/s at sea level
    SpeedOfLight          // c ≈ 299,792,458 m/s
}