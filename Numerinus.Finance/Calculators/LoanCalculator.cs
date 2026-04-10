using Numerinus.Finance.Interfaces;

namespace Numerinus.Finance.Calculators;

/// <summary>
/// Provides loan-related financial calculations including EMI, total repayment, and amortization schedules.
/// </summary>
public class LoanCalculator : IFinanceCalculator
{
    /// <summary>
    /// Calculates the fixed monthly Equated Monthly Instalment (EMI) for a loan.
    /// </summary>
    /// <param name="principal">The total loan amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal (e.g. 0.08 for 8%).</param>
    /// <param name="tenureMonths">The loan tenure in months.</param>
    /// <returns>The fixed monthly EMI.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when principal or tenureMonths are not positive.</exception>
    public double EMI(double principal, double annualRate, int tenureMonths)
    {
        if (principal <= 0) throw new ArgumentOutOfRangeException(nameof(principal), "Principal must be positive.");
        if (tenureMonths <= 0) throw new ArgumentOutOfRangeException(nameof(tenureMonths), "Tenure must be positive.");
        if (annualRate < 0) throw new ArgumentOutOfRangeException(nameof(annualRate), "Annual rate must be non-negative.");

        if (annualRate == 0)
            return principal / tenureMonths;

        double monthlyRate = annualRate / 12;
        double factor = Math.Pow(1 + monthlyRate, tenureMonths);
        return principal * monthlyRate * factor / (factor - 1);
    }

    /// <summary>
    /// Calculates the total amount repaid over the life of a loan.
    /// </summary>
    /// <param name="principal">The total loan amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal.</param>
    /// <param name="tenureMonths">The loan tenure in months.</param>
    /// <returns>The total repayment amount (principal + total interest).</returns>
    public double TotalRepayment(double principal, double annualRate, int tenureMonths)
        => EMI(principal, annualRate, tenureMonths) * tenureMonths;

    /// <summary>
    /// Calculates the total interest paid over the life of a loan.
    /// </summary>
    /// <param name="principal">The total loan amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal.</param>
    /// <param name="tenureMonths">The loan tenure in months.</param>
    /// <returns>The total interest paid.</returns>
    public double TotalInterestPaid(double principal, double annualRate, int tenureMonths)
        => TotalRepayment(principal, annualRate, tenureMonths) - principal;

    /// <summary>
    /// Generates a full amortization schedule for a loan.
    /// Each entry contains the period number, opening balance, EMI paid, interest component,
    /// principal component, and closing balance.
    /// </summary>
    /// <param name="principal">The total loan amount.</param>
    /// <param name="annualRate">The annual interest rate as a decimal.</param>
    /// <param name="tenureMonths">The loan tenure in months.</param>
    /// <returns>An enumerable of <see cref="AmortizationEntry"/> for each payment period.</returns>
    public IEnumerable<AmortizationEntry> AmortizationSchedule(double principal, double annualRate, int tenureMonths)
    {
        double emi = EMI(principal, annualRate, tenureMonths);
        double monthlyRate = annualRate / 12;
        double balance = principal;

        for (int period = 1; period <= tenureMonths; period++)
        {
            double interestComponent = balance * monthlyRate;
            double principalComponent = emi - interestComponent;

            if (period == tenureMonths)
                principalComponent = balance;

            double closingBalance = Math.Max(0, balance - principalComponent);

            yield return new AmortizationEntry(
                Period: period,
                OpeningBalance: balance,
                EMI: emi,
                InterestComponent: interestComponent,
                PrincipalComponent: principalComponent,
                ClosingBalance: closingBalance
            );

            balance = closingBalance;
        }
    }
}

/// <summary>
/// Represents a single period entry in a loan amortization schedule.
/// </summary>
/// <param name="Period">The payment period number (1-based).</param>
/// <param name="OpeningBalance">The outstanding principal at the start of the period.</param>
/// <param name="EMI">The fixed instalment paid in this period.</param>
/// <param name="InterestComponent">The portion of the EMI that covers interest.</param>
/// <param name="PrincipalComponent">The portion of the EMI that reduces the principal.</param>
/// <param name="ClosingBalance">The outstanding principal at the end of the period.</param>
public record AmortizationEntry(
    int Period,
    double OpeningBalance,
    double EMI,
    double InterestComponent,
    double PrincipalComponent,
    double ClosingBalance
);
