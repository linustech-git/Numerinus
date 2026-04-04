using Numerinus.Core.Enums;
using Numerinus.Core.Interfaces;

namespace Numerinus.UnitConversion.Calculators;

public class LengthConverter : IUnitConversion<LengthEnum>
{
    public double Convert(double value, LengthEnum fromUnit, LengthEnum toUnit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be a finite number.");

        double baseValue = ConvertToBaseUnit(value, fromUnit);
        return ConvertFromBaseUnit(baseValue, toUnit);
    }

    public double ConvertToBaseUnit(double value, LengthEnum fromUnit)
    {
        return fromUnit switch
        {
            LengthEnum.Nanometer => value * 1e-9,
            LengthEnum.Micrometer => value * 1e-6,
            LengthEnum.Millimeter => value * 1e-3,
            LengthEnum.Centimeter => value * 1e-2,
            LengthEnum.Decimeter => value * 1e-1,
            LengthEnum.Meter => value,
            LengthEnum.Decameter => value * 10,
            LengthEnum.Hectometer => value * 100,
            LengthEnum.Kilometer => value * 1000,
            LengthEnum.Inch => value * 0.0254,
            LengthEnum.Foot => value * 0.3048,
            LengthEnum.Yard => value * 0.9144,
            LengthEnum.Furlong => value * 201.168,
            LengthEnum.Mile => value * 1609.344,
            LengthEnum.NauticalMile => value * 1852,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, $"Unrecognised length unit '{fromUnit}'.")
        };
    }

    public double ConvertFromBaseUnit(double baseValue, LengthEnum toUnit)
    {
        return toUnit switch
        {
            LengthEnum.Nanometer => baseValue * 1e9,
            LengthEnum.Micrometer => baseValue * 1e6,
            LengthEnum.Millimeter => baseValue * 1e3,
            LengthEnum.Centimeter => baseValue * 1e2,
            LengthEnum.Decimeter => baseValue * 1e1,
            LengthEnum.Meter => baseValue,
            LengthEnum.Decameter => baseValue * 0.1,
            LengthEnum.Hectometer => baseValue * 0.01,
            LengthEnum.Kilometer => baseValue * 0.001,
            LengthEnum.Inch => baseValue * (1.0 / 0.0254),
            LengthEnum.Foot => baseValue * (1.0 / 0.3048),
            LengthEnum.Yard => baseValue * (1.0 / 0.9144),
            LengthEnum.Furlong => baseValue * (1.0 / 201.168),
            LengthEnum.Mile => baseValue * (1.0 / 1609.344),
            LengthEnum.NauticalMile => baseValue * (1.0 / 1852),
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), toUnit, $"Unrecognised length unit '{toUnit}'.")
        };
    }
}