using Numerinus.Core.Enums;
using Numerinus.Core.Interfaces;

namespace Numerinus.UnitConversion.Calculators;

public class SpeedConverter : IUnitConversion<SpeedEnum>
{
    public double Convert(double value, SpeedEnum fromUnit, SpeedEnum toUnit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be a finite number.");

        double baseValue = ConvertToBaseUnit(value, fromUnit);
        return ConvertFromBaseUnit(baseValue, toUnit);
    }

    public double ConvertToBaseUnit(double value, SpeedEnum fromUnit)
    {
        return fromUnit switch
        {
            SpeedEnum.MeterPerSecond => value,
            SpeedEnum.KilometerPerHour => value / 3.6,
            SpeedEnum.MeterPerMinute => value / 60,
            SpeedEnum.CentimeterPerSecond => value / 100,
            SpeedEnum.MilePerHour => value * 0.44704,
            SpeedEnum.FootPerSecond => value * 0.3048,
            SpeedEnum.FootPerMinute => value * 0.00508,
            SpeedEnum.InchPerSecond => value * 0.0254,
            SpeedEnum.Knot => value * 0.514444,
            SpeedEnum.MachNumber => value * 343.0,
            SpeedEnum.SpeedOfLight => value * 299_792_458.0,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, $"Unrecognised speed unit '{fromUnit}'.")
        };
    }

    public double ConvertFromBaseUnit(double baseValue, SpeedEnum toUnit)
    {
        return toUnit switch
        {
            SpeedEnum.MeterPerSecond => baseValue,
            SpeedEnum.KilometerPerHour => baseValue * 3.6,
            SpeedEnum.MeterPerMinute => baseValue * 60,
            SpeedEnum.CentimeterPerSecond => baseValue * 100,
            SpeedEnum.MilePerHour => baseValue * (1.0 / 0.44704),
            SpeedEnum.FootPerSecond => baseValue * (1.0 / 0.3048),
            SpeedEnum.FootPerMinute => baseValue * (1.0 / 0.00508),
            SpeedEnum.InchPerSecond => baseValue * (1.0 / 0.0254),
            SpeedEnum.Knot => baseValue * (1.0 / 0.514444),
            SpeedEnum.MachNumber => baseValue / 343.0,
            SpeedEnum.SpeedOfLight => baseValue / 299_792_458.0,
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), toUnit, $"Unrecognised speed unit '{toUnit}'.")
        };
    }
}