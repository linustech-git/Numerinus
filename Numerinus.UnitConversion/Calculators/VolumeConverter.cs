using Numerinus.Core.Enums;
using Numerinus.Core.Interfaces;

namespace Numerinus.UnitConversion.Calculators;

public class VolumeConverter : IUnitConversion<VolumeEnum>
{
    public double Convert(double value, VolumeEnum fromUnit, VolumeEnum toUnit)
    {
        double baseValue = ConvertToBaseUnit(value, fromUnit);
        return ConvertFromBaseUnit(baseValue, toUnit);
    }

    public double ConvertToBaseUnit(double value, VolumeEnum fromUnit)
    {
        return fromUnit switch
        {
            VolumeEnum.CubicMillimeter => value * 1e-9,
            VolumeEnum.CubicCentimeter => value * 1e-6,
            VolumeEnum.CubicDecimeter => value * 1e-3,
            VolumeEnum.CubicMeter => value,
            VolumeEnum.CubicKilometer => value * 1e9,
            VolumeEnum.Milliliter => value * 1e-6,
            VolumeEnum.Liter => value * 1e-3,
            VolumeEnum.Deciliter => value * 1e-4,
            VolumeEnum.Hectoliter => value * 0.1,
            VolumeEnum.Kiloliter => value * 1,
            VolumeEnum.FluidOunce => value * 2.95735e-5,
            VolumeEnum.Pint => value * 4.73176e-4,
            VolumeEnum.Quart => value * 9.46353e-4,
            VolumeEnum.Gallon => value * 3.78541e-3,
            _ => throw new ArgumentException("Invalid volume unit")
        };
    }

    public double ConvertFromBaseUnit(double baseValue, VolumeEnum toUnit)
    {
        return toUnit switch
        {
            VolumeEnum.CubicMillimeter => baseValue / 1e-9,
            VolumeEnum.CubicCentimeter => baseValue / 1e-6,
            VolumeEnum.CubicDecimeter => baseValue / 1e-3,
            VolumeEnum.CubicMeter => baseValue,
            VolumeEnum.CubicKilometer => baseValue / 1e9,
            VolumeEnum.Milliliter => baseValue / 1e-6,
            VolumeEnum.Liter => baseValue / 1e-3,
            VolumeEnum.Deciliter => baseValue / 1e-4,
            VolumeEnum.Hectoliter => baseValue / 0.1,
            VolumeEnum.Kiloliter => baseValue / 1,
            VolumeEnum.FluidOunce => baseValue / 2.95735e-5,
            VolumeEnum.Pint => baseValue / 4.73176e-4,
            VolumeEnum.Quart => baseValue / 9.46353e-4,
            VolumeEnum.Gallon => baseValue / 3.78541e-3,
            _ => throw new ArgumentException("Invalid volume unit")
        };
    }
}