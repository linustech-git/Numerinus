# Numerinus.Electrical

Electrical engineering calculations module for the **Numerinus** mathematical suite.

## Features

### Ohm's Law (`OhmsLawCalculator`)
| Method | Formula | Description |
|---|---|---|
| `Voltage` | V = I × R | Voltage from current and resistance |
| `Current` | I = V / R | Current from voltage and resistance |
| `Resistance` | R = V / I | Resistance from voltage and current |
| `Conductance` | G = 1 / R | Reciprocal of resistance (siemens) |

### Power (`PowerCalculator`)
| Method | Formula | Description |
|---|---|---|
| `PowerFromVoltageAndCurrent` | P = V × I | Power from voltage and current |
| `PowerFromCurrentAndResistance` | P = I² × R | Power from current and resistance |
| `PowerFromVoltageAndResistance` | P = V² / R | Power from voltage and resistance |
| `Energy` | E = P × t | Energy in joules |
| `Efficiency` | η = P_out / P_in | System efficiency as a decimal |
| `PowerLoss` | P_loss = P_in − P_out | Power dissipated as loss |

### Circuit (`CircuitCalculator`)
| Method | Formula | Description |
|---|---|---|
| `ResistorsInSeries` | R = R1 + R2 + … | Series equivalent resistance |
| `ResistorsInParallel` | 1/R = 1/R1 + 1/R2 + … | Parallel equivalent resistance |
| `EquivalentResistance` | — | Delegates by `CircuitType` enum |
| `CapacitorsInSeries` | 1/C = 1/C1 + 1/C2 + … | Series equivalent capacitance |
| `CapacitorsInParallel` | C = C1 + C2 + … | Parallel equivalent capacitance |
| `EquivalentCapacitance` | — | Delegates by `CircuitType` enum |
| `InductorsInSeries` | L = L1 + L2 + … | Series equivalent inductance |
| `InductorsInParallel` | 1/L = 1/L1 + 1/L2 + … | Parallel equivalent inductance |
| `EquivalentInductance` | — | Delegates by `CircuitType` enum |
| `VoltageDivider` | V_out = V_in × (R2/R_total) | Voltage divider output |
| `CurrentDivider` | I_branch = I_total × (R_eq/R_branch) | Current divider branch current |

### AC Circuits (`ACCircuitCalculator`)
| Method | Formula | Description |
|---|---|---|
| `CapacitiveReactance` | Xc = 1/(2πfC) | Capacitive reactance (Ω) |
| `InductiveReactance` | XL = 2πfL | Inductive reactance (Ω) |
| `Impedance` | Z = √(R² + X²) | Impedance from R and net reactance |
| `RLCImpedance` | Z = √(R² + (XL−Xc)²) | Full series RLC impedance |
| `ResonanceFrequency` | f = 1/(2π√(LC)) | LC resonance frequency |
| `PowerFactor` | PF = R / Z | Power factor (0 to 1) |
| `ApparentPower` | S = V × I | Apparent power (VA) |
| `RealPower` | P = S × PF | Active power (W) |
| `ReactivePower` | Q = √(S²−P²) | Reactive power (VAR) |
| `RMSValue` | V_rms = V_peak / √2 | Peak → RMS conversion |
| `PeakValue` | V_peak = V_rms × √2 | RMS → Peak conversion |
| `AngularFrequency` | ω = 2πf | Angular frequency (rad/s) |

## Enums (defined in `Numerinus.Core`)

- **`CircuitType`** (`Numerinus.Core.Enums`) — `Series`, `Parallel`

## Usage

```csharp
var ohm = new OhmsLawCalculator();
double v = ohm.Voltage(current: 2.0, resistance: 10.0);   // 20 V

var power = new PowerCalculator();
double p = power.PowerFromVoltageAndCurrent(voltage: 12.0, current: 2.0); // 24 W

var circuit = new CircuitCalculator();
double r = circuit.ResistorsInParallel(new[] { 100.0, 200.0, 400.0 });  // 57.14 Ω

var ac = new ACCircuitCalculator();
double z   = ac.RLCImpedance(resistance: 10, inductance: 0.1, capacitance: 100e-6, frequency: 50);
double f0  = ac.ResonanceFrequency(inductance: 0.1, capacitance: 100e-6);
double pf  = ac.PowerFactor(resistance: 10, impedance: z);
```

## License

MIT © 2026 Sunil Chaware
