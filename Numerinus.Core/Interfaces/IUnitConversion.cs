namespace Numerinus.Core.Interfaces
{
    public interface IUnitConversion<TUnit> where TUnit : Enum
    {
        double Convert(double value, TUnit fromUnit, TUnit toUnit);
        double ConvertToBaseUnit(double value, TUnit fromUnit);
        double ConvertFromBaseUnit(double baseValue, TUnit toUnit);
    }
}
