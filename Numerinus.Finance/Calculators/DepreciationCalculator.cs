using Numerinus.Core.Enums;
using Numerinus.Finance.Interfaces;

namespace Numerinus.Finance.Calculators;

/// <summary>
/// Provides asset depreciation calculations using multiple standard accounting methods.
/// </summary>
public class DepreciationCalculator : IFinanceCalculator
{
    /// <summary>
    /// Calculates the annual depreciation for a given year using the specified method.
    /// </summary>
    /// <param name="cost">The original cost of the asset.</param>
    /// <param name="salvageValue">The estimated residual value of the asset at the end of its useful life.</param>
    /// <param name="usefulLifeYears">The total useful life of the asset in years.</param>
    /// <param name="year">The specific year for which depreciation is calculated (1-based).</param>
    /// <param name="method">The depreciation method to apply.</param>
    /// <returns>The depreciation charge for the specified year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when cost, salvageValue, or usefulLifeYears are invalid.</exception>
    public double AnnualDepreciation(double cost, double salvageValue, int usefulLifeYears, int year, DepreciationMethod method)
    {
        ValidateInputs(cost, salvageValue, usefulLifeYears, year);

        return method switch
        {
            DepreciationMethod.StraightLine => StraightLine(cost, salvageValue, usefulLifeYears),
            DepreciationMethod.DecliningBalance => DecliningBalance(cost, salvageValue, usefulLifeYears, year),
            DepreciationMethod.SumOfYearsDigits => SumOfYearsDigits(cost, salvageValue, usefulLifeYears, year),
            _ => throw new ArgumentException("Unsupported depreciation method.", nameof(method))
        };
    }

    /// <summary>
    /// Returns the straight-line depreciation charge per year.
    /// </summary>
    /// <param name="cost">The original cost of the asset.</param>
    /// <param name="salvageValue">The estimated residual value at end of useful life.</param>
    /// <param name="usefulLifeYears">The total useful life in years.</param>
    /// <returns>The equal annual depreciation amount.</returns>
    public double StraightLine(double cost, double salvageValue, int usefulLifeYears)
    {
        ValidateInputs(cost, salvageValue, usefulLifeYears, 1);
        return (cost - salvageValue) / usefulLifeYears;
    }

    /// <summary>
    /// Returns the declining balance depreciation charge for a specific year.
    /// Uses a depreciation rate of 2 / usefulLifeYears (double declining balance).
    /// </summary>
    /// <param name="cost">The original cost of the asset.</param>
    /// <param name="salvageValue">The estimated residual value at end of useful life.</param>
    /// <param name="usefulLifeYears">The total useful life in years.</param>
    /// <param name="year">The year for which to calculate depreciation (1-based).</param>
    /// <returns>The depreciation charge for the specified year.</returns>
    public double DecliningBalance(double cost, double salvageValue, int usefulLifeYears, int year)
    {
        ValidateInputs(cost, salvageValue, usefulLifeYears, year);

        double rate = 2.0 / usefulLifeYears;
        double bookValue = cost * Math.Pow(1 - rate, year - 1);
        double depreciation = bookValue * rate;

        double remainingDepreciable = bookValue - salvageValue;
        return Math.Max(0, Math.Min(depreciation, remainingDepreciable));
    }

    /// <summary>
    /// Returns the sum-of-years-digits depreciation charge for a specific year.
    /// </summary>
    /// <param name="cost">The original cost of the asset.</param>
    /// <param name="salvageValue">The estimated residual value at end of useful life.</param>
    /// <param name="usefulLifeYears">The total useful life in years.</param>
    /// <param name="year">The year for which to calculate depreciation (1-based).</param>
    /// <returns>The depreciation charge for the specified year.</returns>
    public double SumOfYearsDigits(double cost, double salvageValue, int usefulLifeYears, int year)
    {
        ValidateInputs(cost, salvageValue, usefulLifeYears, year);

        double sumOfDigits = usefulLifeYears * (usefulLifeYears + 1) / 2.0;
        double remainingLife = usefulLifeYears - year + 1;
        return (cost - salvageValue) * (remainingLife / sumOfDigits);
    }

    /// <summary>
    /// Returns the book value of an asset at the end of a given year using the specified method.
    /// </summary>
    /// <param name="cost">The original cost of the asset.</param>
    /// <param name="salvageValue">The estimated residual value at end of useful life.</param>
    /// <param name="usefulLifeYears">The total useful life in years.</param>
    /// <param name="year">The year at the end of which the book value is calculated (1-based).</param>
    /// <param name="method">The depreciation method to apply.</param>
    /// <returns>The book value at the end of the specified year.</returns>
    public double BookValue(double cost, double salvageValue, int usefulLifeYears, int year, DepreciationMethod method)
    {
        ValidateInputs(cost, salvageValue, usefulLifeYears, year);

        double accumulatedDepreciation = 0;
        for (int y = 1; y <= year; y++)
            accumulatedDepreciation += AnnualDepreciation(cost, salvageValue, usefulLifeYears, y, method);

        return Math.Max(salvageValue, cost - accumulatedDepreciation);
    }

    private static void ValidateInputs(double cost, double salvageValue, int usefulLifeYears, int year)
    {
        if (cost <= 0) throw new ArgumentOutOfRangeException(nameof(cost), "Cost must be positive.");
        if (salvageValue < 0) throw new ArgumentOutOfRangeException(nameof(salvageValue), "Salvage value must be non-negative.");
        if (salvageValue >= cost) throw new ArgumentOutOfRangeException(nameof(salvageValue), "Salvage value must be less than cost.");
        if (usefulLifeYears <= 0) throw new ArgumentOutOfRangeException(nameof(usefulLifeYears), "Useful life must be positive.");
        if (year <= 0 || year > usefulLifeYears) throw new ArgumentOutOfRangeException(nameof(year), $"Year must be between 1 and {usefulLifeYears}.");
    }
}
