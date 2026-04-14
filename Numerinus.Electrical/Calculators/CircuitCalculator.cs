using Numerinus.Core.Enums;
using Numerinus.Electrical.Interfaces;

namespace Numerinus.Electrical.Calculators;

/// <summary>
/// Provides equivalent resistance, capacitance, and inductance calculations
/// for components connected in series or parallel.
/// </summary>
public class CircuitCalculator : IElectricalCalculator
{
    /// <summary>
    /// Calculates the equivalent resistance of resistors connected in series: R = R1 + R2 + …
    /// </summary>
    /// <param name="resistances">The individual resistance values in ohms (Ω). Each must be positive.</param>
    /// <returns>The total equivalent resistance in ohms (Ω).</returns>
    /// <exception cref="ArgumentNullException">Thrown when resistances is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the collection is empty or any value is not positive.</exception>
    public double ResistorsInSeries(IEnumerable<double> resistances)
    {
        var values = ValidateComponents(resistances, nameof(resistances));
        return values.Sum();
    }

    /// <summary>
    /// Calculates the equivalent resistance of resistors connected in parallel: 1/R = 1/R1 + 1/R2 + …
    /// </summary>
    /// <param name="resistances">The individual resistance values in ohms (Ω). Each must be positive.</param>
    /// <returns>The equivalent parallel resistance in ohms (Ω).</returns>
    /// <exception cref="ArgumentNullException">Thrown when resistances is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the collection is empty or any value is not positive.</exception>
    public double ResistorsInParallel(IEnumerable<double> resistances)
    {
        var values = ValidateComponents(resistances, nameof(resistances));
        double reciprocalSum = values.Sum(r => 1.0 / r);
        return 1.0 / reciprocalSum;
    }

    /// <summary>
    /// Calculates the equivalent resistance for the given circuit type.
    /// </summary>
    /// <param name="resistances">The individual resistance values in ohms (Ω).</param>
    /// <param name="circuitType">Whether components are in series or parallel.</param>
    /// <returns>The equivalent resistance in ohms (Ω).</returns>
    public double EquivalentResistance(IEnumerable<double> resistances, CircuitType circuitType) =>
        circuitType switch
        {
            CircuitType.Series => ResistorsInSeries(resistances),
            CircuitType.Parallel => ResistorsInParallel(resistances),
            _ => throw new ArgumentException("Unsupported circuit type.", nameof(circuitType))
        };

    /// <summary>
    /// Calculates the equivalent capacitance of capacitors connected in series: 1/C = 1/C1 + 1/C2 + …
    /// </summary>
    /// <param name="capacitances">The individual capacitance values in farads (F). Each must be positive.</param>
    /// <returns>The equivalent series capacitance in farads (F).</returns>
    /// <exception cref="ArgumentNullException">Thrown when capacitances is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the collection is empty or any value is not positive.</exception>
    public double CapacitorsInSeries(IEnumerable<double> capacitances)
    {
        var values = ValidateComponents(capacitances, nameof(capacitances));
        double reciprocalSum = values.Sum(c => 1.0 / c);
        return 1.0 / reciprocalSum;
    }

    /// <summary>
    /// Calculates the equivalent capacitance of capacitors connected in parallel: C = C1 + C2 + …
    /// </summary>
    /// <param name="capacitances">The individual capacitance values in farads (F). Each must be positive.</param>
    /// <returns>The total equivalent capacitance in farads (F).</returns>
    /// <exception cref="ArgumentNullException">Thrown when capacitances is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the collection is empty or any value is not positive.</exception>
    public double CapacitorsInParallel(IEnumerable<double> capacitances)
    {
        var values = ValidateComponents(capacitances, nameof(capacitances));
        return values.Sum();
    }

    /// <summary>
    /// Calculates the equivalent capacitance for the given circuit type.
    /// </summary>
    /// <param name="capacitances">The individual capacitance values in farads (F).</param>
    /// <param name="circuitType">Whether components are in series or parallel.</param>
    /// <returns>The equivalent capacitance in farads (F).</returns>
    public double EquivalentCapacitance(IEnumerable<double> capacitances, CircuitType circuitType) =>
        circuitType switch
        {
            CircuitType.Series => CapacitorsInSeries(capacitances),
            CircuitType.Parallel => CapacitorsInParallel(capacitances),
            _ => throw new ArgumentException("Unsupported circuit type.", nameof(circuitType))
        };

    /// <summary>
    /// Calculates the equivalent inductance of inductors connected in series: L = L1 + L2 + …
    /// </summary>
    /// <param name="inductances">The individual inductance values in henries (H). Each must be positive.</param>
    /// <returns>The total equivalent inductance in henries (H).</returns>
    /// <exception cref="ArgumentNullException">Thrown when inductances is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the collection is empty or any value is not positive.</exception>
    public double InductorsInSeries(IEnumerable<double> inductances)
    {
        var values = ValidateComponents(inductances, nameof(inductances));
        return values.Sum();
    }

    /// <summary>
    /// Calculates the equivalent inductance of inductors connected in parallel: 1/L = 1/L1 + 1/L2 + …
    /// </summary>
    /// <param name="inductances">The individual inductance values in henries (H). Each must be positive.</param>
    /// <returns>The equivalent parallel inductance in henries (H).</returns>
    /// <exception cref="ArgumentNullException">Thrown when inductances is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the collection is empty or any value is not positive.</exception>
    public double InductorsInParallel(IEnumerable<double> inductances)
    {
        var values = ValidateComponents(inductances, nameof(inductances));
        double reciprocalSum = values.Sum(l => 1.0 / l);
        return 1.0 / reciprocalSum;
    }

    /// <summary>
    /// Calculates the equivalent inductance for the given circuit type.
    /// </summary>
    /// <param name="inductances">The individual inductance values in henries (H).</param>
    /// <param name="circuitType">Whether components are in series or parallel.</param>
    /// <returns>The equivalent inductance in henries (H).</returns>
    public double EquivalentInductance(IEnumerable<double> inductances, CircuitType circuitType) =>
        circuitType switch
        {
            CircuitType.Series => InductorsInSeries(inductances),
            CircuitType.Parallel => InductorsInParallel(inductances),
            _ => throw new ArgumentException("Unsupported circuit type.", nameof(circuitType))
        };

    /// <summary>
    /// Calculates the voltage divider output for a series resistor network.
    /// </summary>
    /// <param name="inputVoltage">The source voltage in volts (V).</param>
    /// <param name="resistanceOutput">The resistance across which the output is measured in ohms (Ω). Must be positive.</param>
    /// <param name="resistanceTotal">The total series resistance in ohms (Ω). Must be positive and at least as large as resistanceOutput.</param>
    /// <returns>The output voltage in volts (V).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any resistance is not positive or resistanceOutput exceeds resistanceTotal.</exception>
    public double VoltageDivider(double inputVoltage, double resistanceOutput, double resistanceTotal)
    {
        if (resistanceOutput <= 0) throw new ArgumentOutOfRangeException(nameof(resistanceOutput), "Resistance must be positive.");
        if (resistanceTotal <= 0) throw new ArgumentOutOfRangeException(nameof(resistanceTotal), "Total resistance must be positive.");
        if (resistanceOutput > resistanceTotal) throw new ArgumentOutOfRangeException(nameof(resistanceOutput), "Output resistance cannot exceed total resistance.");

        return inputVoltage * (resistanceOutput / resistanceTotal);
    }

    /// <summary>
    /// Calculates the current divider output for a parallel resistor network.
    /// </summary>
    /// <param name="totalCurrent">The total current entering the parallel network in amperes (A).</param>
    /// <param name="resistanceBranch">The resistance of the branch for which current is calculated in ohms (Ω). Must be positive.</param>
    /// <param name="resistanceParallel">The equivalent parallel resistance of all branches in ohms (Ω). Must be positive.</param>
    /// <returns>The current through the specified branch in amperes (A).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any resistance is not positive.</exception>
    public double CurrentDivider(double totalCurrent, double resistanceBranch, double resistanceParallel)
    {
        if (resistanceBranch <= 0) throw new ArgumentOutOfRangeException(nameof(resistanceBranch), "Branch resistance must be positive.");
        if (resistanceParallel <= 0) throw new ArgumentOutOfRangeException(nameof(resistanceParallel), "Parallel resistance must be positive.");

        return totalCurrent * (resistanceParallel / resistanceBranch);
    }

    private static double[] ValidateComponents(IEnumerable<double> values, string paramName)
    {
        if (values is null) throw new ArgumentNullException(paramName);
        double[] array = values.ToArray();
        if (array.Length == 0) throw new ArgumentException("At least one component value is required.", paramName);
        if (array.Any(v => v <= 0)) throw new ArgumentException("All component values must be positive.", paramName);
        return array;
    }
}
