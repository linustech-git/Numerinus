using Numerinus.Core.Enums;
using Numerinus.Finance.Interfaces;

namespace Numerinus.Finance.Calculators;

/// <summary>
/// Provides simple interest, compound interest, present value, and future value calculations.
/// </summary>
public class InterestCalculator : IFinanceCalculator
{
    /// <summary>
    /// Calculates simple interest.
    /// </summary>
    /// <param name="principal">The initial principal amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal (e.g. 0.05 for 5%).</param>
    /// <param name="years">The number of years.</param>
    /// <returns>The simple interest earned.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when principal, rate or years are negative.</exception>
    public double SimpleInterest(double principal, double annualRate, double years)
    {
        if (principal < 0) throw new ArgumentOutOfRangeException(nameof(principal), "Principal must be non-negative.");
        if (annualRate < 0) throw new ArgumentOutOfRangeException(nameof(annualRate), "Annual rate must be non-negative.");
        if (years < 0) throw new ArgumentOutOfRangeException(nameof(years), "Years must be non-negative.");

        return principal * annualRate * years;
    }

    /// <summary>
    /// Calculates the total amount (principal + simple interest).
    /// </summary>
    /// <param name="principal">The initial principal amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal (e.g. 0.05 for 5%).</param>
    /// <param name="years">The number of years.</param>
    /// <returns>The total amount after simple interest.</returns>
    public double SimpleInterestAmount(double principal, double annualRate, double years)
        => principal + SimpleInterest(principal, annualRate, years);

    /// <summary>
    /// Calculates compound interest earned.
    /// </summary>
    /// <param name="principal">The initial principal amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal (e.g. 0.05 for 5%).</param>
    /// <param name="years">The number of years.</param>
    /// <param name="frequency">The compounding frequency per year.</param>
    /// <returns>The compound interest earned.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when principal, rate or years are negative.</exception>
    public double CompoundInterest(double principal, double annualRate, double years, CompoundingFrequency frequency = CompoundingFrequency.Annually)
    {
        if (principal < 0) throw new ArgumentOutOfRangeException(nameof(principal), "Principal must be non-negative.");
        if (annualRate < 0) throw new ArgumentOutOfRangeException(nameof(annualRate), "Annual rate must be non-negative.");
        if (years < 0) throw new ArgumentOutOfRangeException(nameof(years), "Years must be non-negative.");

        double n = (double)frequency;
        double amount = principal * Math.Pow(1 + annualRate / n, n * years);
        return amount - principal;
    }

    /// <summary>
    /// Calculates the total amount after compound interest (principal + compound interest).
    /// </summary>
    /// <param name="principal">The initial principal amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal (e.g. 0.05 for 5%).</param>
    /// <param name="years">The number of years.</param>
    /// <param name="frequency">The compounding frequency per year.</param>
    /// <returns>The total amount after compounding.</returns>
    public double CompoundInterestAmount(double principal, double annualRate, double years, CompoundingFrequency frequency = CompoundingFrequency.Annually)
        => principal + CompoundInterest(principal, annualRate, years, frequency);

    /// <summary>
    /// Calculates the future value of a present sum.
    /// </summary>
    /// <param name="presentValue">The current value of the investment.</param>
    /// <param name="annualRate">The annual interest rate as a decimal (e.g. 0.05 for 5%).</param>
    /// <param name="years">The number of years.</param>
    /// <param name="frequency">The compounding frequency per year.</param>
    /// <returns>The future value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when presentValue or years are negative.</exception>
    public double FutureValue(double presentValue, double annualRate, double years, CompoundingFrequency frequency = CompoundingFrequency.Annually)
    {
        if (presentValue < 0) throw new ArgumentOutOfRangeException(nameof(presentValue), "Present value must be non-negative.");
        if (years < 0) throw new ArgumentOutOfRangeException(nameof(years), "Years must be non-negative.");

        double n = (double)frequency;
        return presentValue * Math.Pow(1 + annualRate / n, n * years);
    }

    /// <summary>
    /// Calculates the present value of a future sum.
    /// </summary>
    /// <param name="futureValue">The value at a future date.</param>
    /// <param name="annualRate">The annual discount rate as a decimal (e.g. 0.05 for 5%).</param>
    /// <param name="years">The number of years until the future value is received.</param>
    /// <param name="frequency">The compounding frequency per year.</param>
    /// <returns>The present value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when futureValue or years are negative.</exception>
    /// <exception cref="ArgumentException">Thrown when annualRate and frequency would produce a zero divisor.</exception>
    public double PresentValue(double futureValue, double annualRate, double years, CompoundingFrequency frequency = CompoundingFrequency.Annually)
    {
        if (futureValue < 0) throw new ArgumentOutOfRangeException(nameof(futureValue), "Future value must be non-negative.");
        if (years < 0) throw new ArgumentOutOfRangeException(nameof(years), "Years must be non-negative.");

        double n = (double)frequency;
        double divisor = Math.Pow(1 + annualRate / n, n * years);
        if (divisor == 0) throw new ArgumentException("The rate and frequency combination produces an invalid divisor.");
        return futureValue / divisor;
    }

    /// <summary>
    /// Calculates the effective annual rate (EAR) from a nominal rate.
    /// </summary>
    /// <param name="nominalRate">The nominal annual interest rate as a decimal (e.g. 0.05 for 5%).</param>
    /// <param name="frequency">The compounding frequency per year.</param>
    /// <returns>The effective annual rate as a decimal.</returns>
    public double EffectiveAnnualRate(double nominalRate, CompoundingFrequency frequency)
    {
        double n = (double)frequency;
        return Math.Pow(1 + nominalRate / n, n) - 1;
    }
}
