using Numerinus.Core.Enums;
using Numerinus.Core.Interfaces;

namespace Numerinus.UnitConversion.Calculators;

public class TemperatureConverter : IUnitConversion<TemperatureEnum>
{
    public double Convert(double value, TemperatureEnum fromUnit, TemperatureEnum toUnit)
    {
        double celsius = ConvertToBaseUnit(value, fromUnit);
        return ConvertFromBaseUnit(celsius, toUnit);
    }

    public double ConvertToBaseUnit(double value, TemperatureEnum fromUnit)
    {
        return fromUnit switch
        {
            TemperatureEnum.Celsius => value,
            TemperatureEnum.Fahrenheit => (value - 32) * 5 / 9,
            TemperatureEnum.Kelvin => value - 273.15,
            TemperatureEnum.Rankine => (value - 491.67) * 5 / 9,
            TemperatureEnum.Delisle => 100 - value * 2 / 3,
            TemperatureEnum.Newton => value * 100 / 33,
            TemperatureEnum.Reaumur => value * 5 / 4,
            TemperatureEnum.Romer => (value - 7.5) * 40 / 21,
            _ => throw new ArgumentException("Invalid temperature unit")
        };
    }

    public double ConvertFromBaseUnit(double celsius, TemperatureEnum toUnit)
    {
        return toUnit switch
        {
            TemperatureEnum.Celsius => celsius,
            TemperatureEnum.Fahrenheit => celsius * 9 / 5 + 32,
            TemperatureEnum.Kelvin => celsius + 273.15,
            TemperatureEnum.Rankine => (celsius + 273.15) * 9 / 5,
            TemperatureEnum.Delisle => (100 - celsius) * 3 / 2,
            TemperatureEnum.Newton => celsius * 33 / 100,
            TemperatureEnum.Reaumur => celsius * 4 / 5,
            TemperatureEnum.Romer => celsius * 21 / 40 + 7.5,
            _ => throw new ArgumentException("Invalid temperature unit")
        };
    }
}