using Numerinus.Core.Constants;
using Numerinus.Electrical.Interfaces;

namespace Numerinus.Electrical.Calculators;

/// <summary>
/// Provides AC (alternating current) circuit calculations including reactance,
/// impedance, power factor, resonance frequency, and AC power quantities.
/// </summary>
public class ACCircuitCalculator : IElectricalCalculator
{
    /// <summary>
    /// Calculates capacitive reactance: Xc = 1 / (2π × f × C).
    /// </summary>
    /// <param name="frequency">Frequency in hertz (Hz). Must be positive.</param>
    /// <param name="capacitance">Capacitance in farads (F). Must be positive.</param>
    /// <returns>Capacitive reactance in ohms (Ω).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when frequency or capacitance is not positive.</exception>
    public double CapacitiveReactance(double frequency, double capacitance)
    {
        if (frequency <= 0) throw new ArgumentOutOfRangeException(nameof(frequency), "Frequency must be positive.");
        if (capacitance <= 0) throw new ArgumentOutOfRangeException(nameof(capacitance), "Capacitance must be positive.");

        return 1.0 / (2 * NumerinusConstants.Pi * frequency * capacitance);
    }

    /// <summary>
    /// Calculates inductive reactance: XL = 2π × f × L.
    /// </summary>
    /// <param name="frequency">Frequency in hertz (Hz). Must be positive.</param>
    /// <param name="inductance">Inductance in henries (H). Must be positive.</param>
    /// <returns>Inductive reactance in ohms (Ω).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when frequency or inductance is not positive.</exception>
    public double InductiveReactance(double frequency, double inductance)
    {
        if (frequency <= 0) throw new ArgumentOutOfRangeException(nameof(frequency), "Frequency must be positive.");
        if (inductance <= 0) throw new ArgumentOutOfRangeException(nameof(inductance), "Inductance must be positive.");

        return 2 * NumerinusConstants.Pi * frequency * inductance;
    }

    /// <summary>
    /// Calculates impedance from resistance and net reactance: Z = √(R² + X²).
    /// </summary>
    /// <param name="resistance">Resistance in ohms (Ω). Must be non-negative.</param>
    /// <param name="reactance">Net reactance (XL − Xc) in ohms (Ω).</param>
    /// <returns>Impedance in ohms (Ω).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when resistance is negative.</exception>
    public double Impedance(double resistance, double reactance)
    {
        if (resistance < 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be non-negative.");

        return Math.Sqrt(resistance * resistance + reactance * reactance);
    }

    /// <summary>
    /// Calculates the total impedance of a series RLC circuit at a given frequency:
    /// Z = √(R² + (XL − Xc)²).
    /// </summary>
    /// <param name="resistance">Resistance in ohms (Ω). Must be non-negative.</param>
    /// <param name="inductance">Inductance in henries (H). Must be positive.</param>
    /// <param name="capacitance">Capacitance in farads (F). Must be positive.</param>
    /// <param name="frequency">Frequency in hertz (Hz). Must be positive.</param>
    /// <returns>Impedance of the RLC circuit in ohms (Ω).</returns>
    public double RLCImpedance(double resistance, double inductance, double capacitance, double frequency)
    {
        double xl = InductiveReactance(frequency, inductance);
        double xc = CapacitiveReactance(frequency, capacitance);
        return Impedance(resistance, xl - xc);
    }

    /// <summary>
    /// Calculates the resonance frequency of an LC circuit: f = 1 / (2π × √(L × C)).
    /// At resonance, inductive and capacitive reactances cancel, impedance is minimised.
    /// </summary>
    /// <param name="inductance">Inductance in henries (H). Must be positive.</param>
    /// <param name="capacitance">Capacitance in farads (F). Must be positive.</param>
    /// <returns>Resonance frequency in hertz (Hz).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when inductance or capacitance is not positive.</exception>
    public double ResonanceFrequency(double inductance, double capacitance)
    {
        if (inductance <= 0) throw new ArgumentOutOfRangeException(nameof(inductance), "Inductance must be positive.");
        if (capacitance <= 0) throw new ArgumentOutOfRangeException(nameof(capacitance), "Capacitance must be positive.");

        return 1.0 / (2 * NumerinusConstants.Pi * Math.Sqrt(inductance * capacitance));
    }

    /// <summary>
    /// Calculates the power factor of an AC circuit: PF = R / Z.
    /// A power factor of 1.0 means purely resistive; 0 means purely reactive.
    /// </summary>
    /// <param name="resistance">Resistance in ohms (Ω). Must be non-negative.</param>
    /// <param name="impedance">Impedance in ohms (Ω). Must be positive.</param>
    /// <returns>Power factor as a value between 0 and 1.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when resistance is negative or impedance is not positive.</exception>
    public double PowerFactor(double resistance, double impedance)
    {
        if (resistance < 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be non-negative.");
        if (impedance <= 0) throw new ArgumentOutOfRangeException(nameof(impedance), "Impedance must be positive.");

        return resistance / impedance;
    }

    /// <summary>
    /// Calculates the apparent power in an AC circuit: S = V × I.
    /// </summary>
    /// <param name="voltage">RMS voltage in volts (V). Must be non-negative.</param>
    /// <param name="current">RMS current in amperes (A). Must be non-negative.</param>
    /// <returns>Apparent power in volt-amperes (VA).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when voltage or current is negative.</exception>
    public double ApparentPower(double voltage, double current)
    {
        if (voltage < 0) throw new ArgumentOutOfRangeException(nameof(voltage), "Voltage must be non-negative.");
        if (current < 0) throw new ArgumentOutOfRangeException(nameof(current), "Current must be non-negative.");

        return voltage * current;
    }

    /// <summary>
    /// Calculates the real (active) power consumed in an AC circuit: P = S × PF.
    /// </summary>
    /// <param name="apparentPower">Apparent power in volt-amperes (VA). Must be non-negative.</param>
    /// <param name="powerFactor">Power factor (0 to 1 inclusive).</param>
    /// <returns>Real power in watts (W).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when apparentPower is negative or powerFactor is outside [0, 1].</exception>
    public double RealPower(double apparentPower, double powerFactor)
    {
        if (apparentPower < 0) throw new ArgumentOutOfRangeException(nameof(apparentPower), "Apparent power must be non-negative.");
        if (powerFactor < 0 || powerFactor > 1) throw new ArgumentOutOfRangeException(nameof(powerFactor), "Power factor must be between 0 and 1.");

        return apparentPower * powerFactor;
    }

    /// <summary>
    /// Calculates the reactive power in an AC circuit: Q = √(S² − P²).
    /// </summary>
    /// <param name="apparentPower">Apparent power in volt-amperes (VA). Must be non-negative.</param>
    /// <param name="realPower">Real power in watts (W). Must be non-negative and not exceed apparent power.</param>
    /// <returns>Reactive power in volt-amperes reactive (VAR).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when apparentPower or realPower is negative, or realPower exceeds apparentPower.</exception>
    public double ReactivePower(double apparentPower, double realPower)
    {
        if (apparentPower < 0) throw new ArgumentOutOfRangeException(nameof(apparentPower), "Apparent power must be non-negative.");
        if (realPower < 0) throw new ArgumentOutOfRangeException(nameof(realPower), "Real power must be non-negative.");
        if (realPower > apparentPower) throw new ArgumentOutOfRangeException(nameof(realPower), "Real power cannot exceed apparent power.");

        return Math.Sqrt(apparentPower * apparentPower - realPower * realPower);
    }

    /// <summary>
    /// Calculates the RMS (root mean square) value of a sinusoidal AC signal from its peak value:
    /// V_rms = V_peak / √2.
    /// </summary>
    /// <param name="peakValue">The peak (maximum) value of the AC signal. Must be non-negative.</param>
    /// <returns>The RMS value of the signal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when peakValue is negative.</exception>
    public double RMSValue(double peakValue)
    {
        if (peakValue < 0) throw new ArgumentOutOfRangeException(nameof(peakValue), "Peak value must be non-negative.");

        return peakValue / Math.Sqrt(2);
    }

    /// <summary>
    /// Calculates the peak value of a sinusoidal AC signal from its RMS value:
    /// V_peak = V_rms × √2.
    /// </summary>
    /// <param name="rmsValue">The RMS value of the AC signal. Must be non-negative.</param>
    /// <returns>The peak value of the signal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when rmsValue is negative.</exception>
    public double PeakValue(double rmsValue)
    {
        if (rmsValue < 0) throw new ArgumentOutOfRangeException(nameof(rmsValue), "RMS value must be non-negative.");

        return rmsValue * Math.Sqrt(2);
    }

    /// <summary>
    /// Calculates the angular frequency (ω) from frequency: ω = 2π × f.
    /// </summary>
    /// <param name="frequency">Frequency in hertz (Hz). Must be positive.</param>
    /// <returns>Angular frequency in radians per second (rad/s).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when frequency is not positive.</exception>
    public double AngularFrequency(double frequency)
    {
        if (frequency <= 0) throw new ArgumentOutOfRangeException(nameof(frequency), "Frequency must be positive.");

        return 2 * NumerinusConstants.Pi * frequency;
    }
}
