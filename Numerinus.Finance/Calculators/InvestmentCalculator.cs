using Numerinus.Finance.Interfaces;

namespace Numerinus.Finance.Calculators;

/// <summary>
/// Provides investment performance calculations including ROI, NPV, IRR, and CAGR.
/// </summary>
public class InvestmentCalculator : IFinanceCalculator
{
    /// <summary>
    /// Calculates the Return on Investment (ROI) as a decimal.
    /// </summary>
    /// <param name="netProfit">The net profit (gain minus cost of investment).</param>
    /// <param name="costOfInvestment">The total cost of the investment.</param>
    /// <returns>ROI as a decimal (e.g. 0.25 represents 25%).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when costOfInvestment is zero or negative.</exception>
    public double ROI(double netProfit, double costOfInvestment)
    {
        if (costOfInvestment <= 0)
            throw new ArgumentOutOfRangeException(nameof(costOfInvestment), "Cost of investment must be positive.");

        return netProfit / costOfInvestment;
    }

    /// <summary>
    /// Calculates the Compound Annual Growth Rate (CAGR).
    /// </summary>
    /// <param name="beginningValue">The value of the investment at the start.</param>
    /// <param name="endingValue">The value of the investment at the end.</param>
    /// <param name="years">The number of years of the investment period.</param>
    /// <returns>CAGR as a decimal (e.g. 0.10 represents 10%).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when beginningValue is zero or negative, or years is not positive.</exception>
    public double CAGR(double beginningValue, double endingValue, double years)
    {
        if (beginningValue <= 0)
            throw new ArgumentOutOfRangeException(nameof(beginningValue), "Beginning value must be positive.");
        if (endingValue < 0)
            throw new ArgumentOutOfRangeException(nameof(endingValue), "Ending value must be non-negative.");
        if (years <= 0)
            throw new ArgumentOutOfRangeException(nameof(years), "Years must be positive.");

        return Math.Pow(endingValue / beginningValue, 1.0 / years) - 1;
    }

    /// <summary>
    /// Calculates the Net Present Value (NPV) of a series of cash flows discounted at a given rate.
    /// </summary>
    /// <param name="discountRate">The annual discount rate as a decimal (e.g. 0.10 for 10%).</param>
    /// <param name="initialInvestment">The upfront investment cost (positive value, will be subtracted).</param>
    /// <param name="cashFlows">The expected cash flows for each subsequent period (year 1, 2, …).</param>
    /// <returns>The net present value. A positive NPV indicates a profitable investment.</returns>
    /// <exception cref="ArgumentNullException">Thrown when cashFlows is null.</exception>
    public double NPV(double discountRate, double initialInvestment, IEnumerable<double> cashFlows)
    {
        if (cashFlows is null) throw new ArgumentNullException(nameof(cashFlows));

        double npv = -initialInvestment;
        int period = 1;
        foreach (double cashFlow in cashFlows)
        {
            npv += cashFlow / Math.Pow(1 + discountRate, period);
            period++;
        }
        return npv;
    }

    /// <summary>
    /// Estimates the Internal Rate of Return (IRR) using the Newton-Raphson method.
    /// The IRR is the discount rate that makes the NPV of all cash flows equal to zero.
    /// </summary>
    /// <param name="initialInvestment">The upfront investment cost (positive value, will be subtracted).</param>
    /// <param name="cashFlows">The expected cash flows for each subsequent period (year 1, 2, …).</param>
    /// <param name="tolerance">The convergence tolerance. Defaults to 1e-6.</param>
    /// <param name="maxIterations">The maximum number of iterations. Defaults to 1000.</param>
    /// <returns>The estimated IRR as a decimal.</returns>
    /// <exception cref="ArgumentNullException">Thrown when cashFlows is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when IRR does not converge within the maximum iterations.</exception>
    public double IRR(double initialInvestment, IEnumerable<double> cashFlows, double tolerance = 1e-6, int maxIterations = 1000)
    {
        if (cashFlows is null) throw new ArgumentNullException(nameof(cashFlows));

        double[] flows = cashFlows.ToArray();
        double rate = 0.1;

        for (int i = 0; i < maxIterations; i++)
        {
            double npv = -initialInvestment;
            double dNpv = 0;

            for (int t = 1; t <= flows.Length; t++)
            {
                double discountFactor = Math.Pow(1 + rate, t);
                npv += flows[t - 1] / discountFactor;
                dNpv -= t * flows[t - 1] / (discountFactor * (1 + rate));
            }

            if (Math.Abs(dNpv) < 1e-12)
                throw new InvalidOperationException("IRR derivative is too small; unable to converge.");

            double newRate = rate - npv / dNpv;

            if (Math.Abs(newRate - rate) < tolerance)
                return newRate;

            rate = newRate;
        }

        throw new InvalidOperationException($"IRR did not converge after {maxIterations} iterations.");
    }

    /// <summary>
    /// Calculates the Payback Period — the number of periods required to recover the initial investment.
    /// </summary>
    /// <param name="initialInvestment">The upfront investment cost.</param>
    /// <param name="cashFlows">The expected cash flows for each subsequent period.</param>
    /// <returns>
    /// The fractional period at which the cumulative cash flow equals the initial investment,
    /// or <see cref="double.PositiveInfinity"/> if the investment is never recovered.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when cashFlows is null.</exception>
    public double PaybackPeriod(double initialInvestment, IEnumerable<double> cashFlows)
    {
        if (cashFlows is null) throw new ArgumentNullException(nameof(cashFlows));

        double cumulative = 0;
        int period = 0;
        foreach (double cashFlow in cashFlows)
        {
            period++;
            double previous = cumulative;
            cumulative += cashFlow;
            if (cumulative >= initialInvestment)
            {
                double fraction = (initialInvestment - previous) / cashFlow;
                return period - 1 + fraction;
            }
        }
        return double.PositiveInfinity;
    }
}
