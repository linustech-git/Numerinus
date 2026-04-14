using Numerinus.Electrical.Interfaces;

namespace Numerinus.Electrical.Calculators;

/// <summary>
/// Provides electrical power and energy calculations.
/// </summary>
public class PowerCalculator : IElectricalCalculator
{
    /// <summary>
    /// Calculates power from voltage and current: P = V × I.
    /// </summary>
    /// <param name="voltage">Voltage in volts (V). Must be non-negative.</param>
    /// <param name="current">Current in amperes (A). Must be non-negative.</param>
    /// <returns>Power in watts (W).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when voltage or current is negative.</exception>
    public double PowerFromVoltageAndCurrent(double voltage, double current)
    {
        if (voltage < 0) throw new ArgumentOutOfRangeException(nameof(voltage), "Voltage must be non-negative.");
        if (current < 0) throw new ArgumentOutOfRangeException(nameof(current), "Current must be non-negative.");

        return voltage * current;
    }

    /// <summary>
    /// Calculates power from current and resistance: P = I² × R.
    /// </summary>
    /// <param name="current">Current in amperes (A). Must be non-negative.</param>
    /// <param name="resistance">Resistance in ohms (Ω). Must be non-negative.</param>
    /// <returns>Power in watts (W).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when current or resistance is negative.</exception>
    public double PowerFromCurrentAndResistance(double current, double resistance)
    {
        if (current < 0) throw new ArgumentOutOfRangeException(nameof(current), "Current must be non-negative.");
        if (resistance < 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be non-negative.");

        return current * current * resistance;
    }

    /// <summary>
    /// Calculates power from voltage and resistance: P = V² / R.
    /// </summary>
    /// <param name="voltage">Voltage in volts (V). Must be non-negative.</param>
    /// <param name="resistance">Resistance in ohms (Ω). Must be positive.</param>
    /// <returns>Power in watts (W).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when voltage is negative or resistance is not positive.</exception>
    public double PowerFromVoltageAndResistance(double voltage, double resistance)
    {
        if (voltage < 0) throw new ArgumentOutOfRangeException(nameof(voltage), "Voltage must be non-negative.");
        if (resistance <= 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be positive.");

        return voltage * voltage / resistance;
    }

    /// <summary>
    /// Calculates electrical energy consumed: E = P × t.
    /// </summary>
    /// <param name="power">Power in watts (W). Must be non-negative.</param>
    /// <param name="timeSeconds">Time duration in seconds. Must be non-negative.</param>
    /// <returns>Energy in joules (J).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when power or timeSeconds is negative.</exception>
    public double Energy(double power, double timeSeconds)
    {
        if (power < 0) throw new ArgumentOutOfRangeException(nameof(power), "Power must be non-negative.");
        if (timeSeconds < 0) throw new ArgumentOutOfRangeException(nameof(timeSeconds), "Time must be non-negative.");

        return power * timeSeconds;
    }

    /// <summary>
    /// Calculates electrical efficiency: η = P_out / P_in.
    /// </summary>
    /// <param name="outputPower">The useful output power in watts (W). Must be non-negative.</param>
    /// <param name="inputPower">The total input power in watts (W). Must be positive.</param>
    /// <returns>Efficiency as a decimal (e.g. 0.90 represents 90%).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when outputPower is negative or inputPower is not positive.</exception>
    /// <exception cref="ArgumentException">Thrown when outputPower exceeds inputPower.</exception>
    public double Efficiency(double outputPower, double inputPower)
    {
        if (outputPower < 0) throw new ArgumentOutOfRangeException(nameof(outputPower), "Output power must be non-negative.");
        if (inputPower <= 0) throw new ArgumentOutOfRangeException(nameof(inputPower), "Input power must be positive.");
        if (outputPower > inputPower) throw new ArgumentException("Output power cannot exceed input power.");

        return outputPower / inputPower;
    }

    /// <summary>
    /// Calculates power loss as the difference between input and output power.
    /// </summary>
    /// <param name="inputPower">The total input power in watts (W). Must be positive.</param>
    /// <param name="outputPower">The useful output power in watts (W). Must be non-negative.</param>
    /// <returns>Power loss in watts (W).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when outputPower is negative or inputPower is not positive.</exception>
    /// <exception cref="ArgumentException">Thrown when outputPower exceeds inputPower.</exception>
    public double PowerLoss(double inputPower, double outputPower)
    {
        if (outputPower < 0) throw new ArgumentOutOfRangeException(nameof(outputPower), "Output power must be non-negative.");
        if (inputPower <= 0) throw new ArgumentOutOfRangeException(nameof(inputPower), "Input power must be positive.");
        if (outputPower > inputPower) throw new ArgumentException("Output power cannot exceed input power.");

        return inputPower - outputPower;
    }
}
