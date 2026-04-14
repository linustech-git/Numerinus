using Numerinus.Electrical.Interfaces;

namespace Numerinus.Electrical.Calculators;

/// <summary>
/// Provides calculations based on Ohm's Law: V = I × R.
/// Any one quantity can be derived from the other two.
/// </summary>
public class OhmsLawCalculator : IElectricalCalculator
{
    /// <summary>
    /// Calculates voltage (V) given current and resistance.
    /// </summary>
    /// <param name="current">Current in amperes (A). Must be non-negative.</param>
    /// <param name="resistance">Resistance in ohms (Ω). Must be non-negative.</param>
    /// <returns>Voltage in volts (V).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when current or resistance is negative.</exception>
    public double Voltage(double current, double resistance)
    {
        if (current < 0) throw new ArgumentOutOfRangeException(nameof(current), "Current must be non-negative.");
        if (resistance < 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be non-negative.");

        return current * resistance;
    }

    /// <summary>
    /// Calculates current (I) given voltage and resistance.
    /// </summary>
    /// <param name="voltage">Voltage in volts (V). Must be non-negative.</param>
    /// <param name="resistance">Resistance in ohms (Ω). Must be positive.</param>
    /// <returns>Current in amperes (A).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when voltage is negative or resistance is not positive.</exception>
    public double Current(double voltage, double resistance)
    {
        if (voltage < 0) throw new ArgumentOutOfRangeException(nameof(voltage), "Voltage must be non-negative.");
        if (resistance <= 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be positive.");

        return voltage / resistance;
    }

    /// <summary>
    /// Calculates resistance (R) given voltage and current.
    /// </summary>
    /// <param name="voltage">Voltage in volts (V). Must be non-negative.</param>
    /// <param name="current">Current in amperes (A). Must be positive.</param>
    /// <returns>Resistance in ohms (Ω).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when voltage is negative or current is not positive.</exception>
    public double Resistance(double voltage, double current)
    {
        if (voltage < 0) throw new ArgumentOutOfRangeException(nameof(voltage), "Voltage must be non-negative.");
        if (current <= 0) throw new ArgumentOutOfRangeException(nameof(current), "Current must be positive.");

        return voltage / current;
    }

    /// <summary>
    /// Calculates conductance (G), the reciprocal of resistance: G = 1 / R.
    /// </summary>
    /// <param name="resistance">Resistance in ohms (Ω). Must be positive.</param>
    /// <returns>Conductance in siemens (S).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when resistance is not positive.</exception>
    public double Conductance(double resistance)
    {
        if (resistance <= 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be positive.");

        return 1.0 / resistance;
    }
}
