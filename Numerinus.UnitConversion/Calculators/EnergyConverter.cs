using Numerinus.Core.Enums;
using Numerinus.Core.Interfaces;

namespace Numerinus.UnitConversion.Calculators;

public class EnergyConverter : IUnitConversion<EnergyEnum>
{
    public double Convert(double value, EnergyEnum fromUnit, EnergyEnum toUnit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be a finite number.");

        double baseValue = ConvertToBaseUnit(value, fromUnit);
        return ConvertFromBaseUnit(baseValue, toUnit);
    }

    public double ConvertToBaseUnit(double value, EnergyEnum fromUnit)
    {
        return fromUnit switch
        {
            EnergyEnum.Joule            => value,
            EnergyEnum.Kilojoule        => value * 1_000,
            EnergyEnum.Megajoule        => value * 1_000_000,
            EnergyEnum.Gigajoule        => value * 1_000_000_000,
            EnergyEnum.WattHour         => value * 3_600,
            EnergyEnum.KilowattHour     => value * 3_600_000,
            EnergyEnum.MegawattHour     => value * 3_600_000_000,
            EnergyEnum.Calorie          => value * 4.184,
            EnergyEnum.Kilocalorie      => value * 4_184,
            EnergyEnum.BritishThermalUnit => value * 1_055.05585,
            EnergyEnum.Electronvolt     => value * 1.602176634e-19,
            EnergyEnum.KiloElectronvolt => value * 1.602176634e-16,
            EnergyEnum.MegaElectronvolt => value * 1.602176634e-13,
            EnergyEnum.FootPound        => value * 1.3558179483,
            EnergyEnum.Erg              => value * 1e-7,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, $"Unrecognised energy unit '{fromUnit}'.")
        };
    }

    public double ConvertFromBaseUnit(double baseValue, EnergyEnum toUnit)
    {
        return toUnit switch
        {
            EnergyEnum.Joule            => baseValue,
            EnergyEnum.Kilojoule        => baseValue * 1e-3,
            EnergyEnum.Megajoule        => baseValue * 1e-6,
            EnergyEnum.Gigajoule        => baseValue * 1e-9,
            EnergyEnum.WattHour         => baseValue * (1.0 / 3_600),
            EnergyEnum.KilowattHour     => baseValue * (1.0 / 3_600_000),
            EnergyEnum.MegawattHour     => baseValue * (1.0 / 3_600_000_000),
            EnergyEnum.Calorie          => baseValue * (1.0 / 4.184),
            EnergyEnum.Kilocalorie      => baseValue * (1.0 / 4_184),
            EnergyEnum.BritishThermalUnit => baseValue * (1.0 / 1_055.05585),
            EnergyEnum.Electronvolt     => baseValue * (1.0 / 1.602176634e-19),
            EnergyEnum.KiloElectronvolt => baseValue * (1.0 / 1.602176634e-16),
            EnergyEnum.MegaElectronvolt => baseValue * (1.0 / 1.602176634e-13),
            EnergyEnum.FootPound        => baseValue * (1.0 / 1.3558179483),
            EnergyEnum.Erg              => baseValue * 1e7,
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), toUnit, $"Unrecognised energy unit '{toUnit}'.")
        };
    }
}
