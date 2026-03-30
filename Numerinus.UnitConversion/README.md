
# Project Title

A brief description of what this project does and who it's for

# Numerinus.UnitConversion

Unit conversion module for the **Numerinus** mathematical suite. Built on top of `Numerinus.Core`, this package provides accurate, strongly-typed unit conversions across a range of physical measurement categories.

[![NuGet](https://img.shields.io/nuget/v/Numerinus.UnitConversion)](https://www.nuget.org/packages/Numerinus.UnitConversion)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## Features

- **`IUnitConversion<TUnit>`** — Generic interface for all unit converters, enabling consistent and type-safe conversions
- **`LengthConverter`** — Convert between metric and imperial length units
- **`VolumeConverter`** — Convert between metric and imperial volume units
- All converters use a **base unit strategy** for accurate multi-step conversions

---

## Installation

---

## Length Conversion

Base unit: **Meter**

### Supported Units (`LengthEnum`)

| Enum Value | Unit |
|---|---|
| `Nanometer` | Nanometer (nm) |
| `Micrometer` | Micrometer (µm) |
| `Millimeter` | Millimeter (mm) |
| `Centimeter` | Centimeter (cm) |
| `Decimeter` | Decimeter (dm) |
| `Meter` | Meter (m) — base unit |
| `Decameter` | Decameter (dam) |
| `Hectometer` | Hectometer (hm) |
| `Kilometer` | Kilometer (km) |
| `Inch` | Inch (in) |
| `Foot` | Foot (ft) |
| `Yard` | Yard (yd) |
| `Furlong` | Furlong |
| `Mile` | Mile (mi) |
| `NauticalMile` | Nautical Mile (nmi) |

### Usage


using Numerinus.Core.Enums; using Numerinus.UnitConversion.Calculators;
var converter = new LengthConverter();

// Kilometers to Miles double miles = converter.Convert(10, LengthEnum.Kilometer, LengthEnum.Mile); Console.WriteLine(miles); // ~6.2137

// Feet to Meters double meters = converter.Convert(100, LengthEnum.Foot, LengthEnum.Meter); Console.WriteLine(meters); // 30.48

// To base unit (Meter) double baseValue = converter.ConvertToBaseUnit(5, LengthEnum.Kilometer); Console.WriteLine(baseValue); // 5000

// From base unit (Meter) double inches = converter.ConvertFromBaseUnit(1, LengthEnum.Inch); Console.WriteLine(inches); // ~39.3701
---

## Volume Conversion

Base unit: **Cubic Meter**

### Supported Units (`VolumeEnum`)

| Enum Value | Unit |
|---|---|
| `CubicMillimeter` | Cubic Millimeter (mm³) |
| `CubicCentimeter` | Cubic Centimeter (cm³) |
| `CubicDecimeter` | Cubic Decimeter (dm³) |
| `CubicMeter` | Cubic Meter (m³) — base unit |
| `CubicKilometer` | Cubic Kilometer (km³) |
| `Milliliter` | Milliliter (mL) |
| `Liter` | Liter (L) |
| `Deciliter` | Deciliter (dL) |
| `Hectoliter` | Hectoliter (hL) |
| `Kiloliter` | Kiloliter (kL) |
| `FluidOunce` | Fluid Ounce (US fl oz) |
| `Pint` | Pint (US liquid pt) |
| `Quart` | Quart (US liquid qt) |
| `Gallon` | Gallon (US liquid gal) |

### Usage


using Numerinus.Core.Enums; using Numerinus.UnitConversion.Calculators;
var converter = new VolumeConverter();

// Liters to Gallons double gallons = converter.Convert(10, VolumeEnum.Liter, VolumeEnum.Gallon); Console.WriteLine(gallons); // ~2.6417

// Cubic Meters to Liters double liters = converter.Convert(1, VolumeEnum.CubicMeter, VolumeEnum.Liter); Console.WriteLine(liters); // 1000

// Fluid Ounces to Milliliters double ml = converter.Convert(8, VolumeEnum.FluidOunce, VolumeEnum.Milliliter); Console.WriteLine(ml); // ~236.588

// To base unit (Cubic Meter) double baseValue = converter.ConvertToBaseUnit(500, VolumeEnum.Liter); Console.WriteLine(baseValue); // 0.5

// From base unit (Cubic Meter) double pints = converter.ConvertFromBaseUnit(1, VolumeEnum.Pint); Console.WriteLine(pints); // ~2113.38

---

## Architecture

All converters implement the generic `IUnitConversion<TUnit>` interface from `Numerinus.Core`:


public interface IUnitConversion<TUnit> where TUnit : Enum { double Convert(double value, TUnit fromUnit, TUnit toUnit); double ConvertToBaseUnit(double value, TUnit fromUnit); double ConvertFromBaseUnit(double baseValue, TUnit toUnit); }

Conversions follow a **two-step base unit strategy**:
1. Convert the input value **to** the base unit
2. Convert the base unit **to** the target unit

This ensures accuracy across any combination of units without requiring an N×N conversion table.

---

## Dependencies

- **`Numerinus.Core`** — For the `IUnitConversion<TUnit>` interface and unit enums

---

## Related Modules

| Package | Description |
|---|---|
| [`Numerinus.Core`](https://www.nuget.org/packages/Numerinus.Core) | Foundational types and interfaces |
| [`Numerinus.Algebra`](https://www.nuget.org/packages/Numerinus.Algebra) | Polynomial, rational functions, and linear systems |
| [`Numerinus.Geometry`](https://www.nuget.org/packages/Numerinus.Geometry) | Geometric types and transformations |

---

## Support & Maintenance

Numerinus is an actively maintained suite of .NET libraries. To ensure updates, bug fixes, and new converters continue to ship, consider supporting the project:

- [Sponsor on GitHub](https://github.com/sponsors/linustech-git)
- [Connect on LinkedIn](https://www.linkedin.com/in/sunil-chaware-9035a646/)

---

## License

Copyright (c) 2026 Sunil Chaware. Licensed under the MIT License.
https://opensource.org/licenses/MIT