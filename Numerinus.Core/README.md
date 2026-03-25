# Numerinus.Core 🧮

The foundational engine for the **Numerinus** mathematical suite. This package provides the core interfaces, high-precision constants, and base numerical types used across all Numerinus modules.

[![NuGet](https://img.shields.io/nuget/v/Numerinus.Core)](https://www.nuget.org/packages/Numerinus.Core)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## Features

- **Generic Math Infrastructure** — `IArithmetic<T>` interface for building type-safe, generic numerical algorithms.
- **Advanced Numerics** — Built-in `ComplexNumber` and `Scalar` types ready to use out of the box.
- **High Precision** — `Accuracy` utility for reliable floating-point epsilon comparisons.
- **Mathematical Constants** — High-precision values for π, *e*, and φ available globally.

---

## Installation
dotnet add package Numerinus.Core

using Numerinus.Core.Numerics;
var c1 = new ComplexNumber(10, 5); 
var c2 = new ComplexNumber(2, 3);
var sum      = ComplexNumber.Add(c1, c2);      
// 12 + 8i var diff     = ComplexNumber.Subtract(c1, c2); // 8 + 2i 
var product  = ComplexNumber.Multiply(c1, c2); // 5 + 40i var quotient = ComplexNumber.Divide(c1, c2);   // 2.846... + (-0.769...)i
Console.WriteLine(sum);         // 
12 + 8i Console.WriteLine(c1.IsZero()); // False


### Scalar
using Numerinus.Core.Numerics;
Scalar a = 6.0; // implicit conversion from double Scalar b = 2.0;
var result = Scalar.Multiply(a, b); double value = result; // implicit conversion back to double → 12.0
Console.WriteLine(result);       // 12 Console.WriteLine(Scalar.Zero);  // 0 Console.WriteLine(Scalar.One);   // 1


### Accuracy
using Numerinus.Core.Precision;
// Direct equality is unreliable for floating-point bool wrong   = (0.1 + 0.2 == 0.3);                // False ❌ bool correct = Accuracy.AreEqual(0.1 + 0.2, 0.3); // True  ✅
// High precision comparison bool precise = Accuracy.AreEqual(a, b, Accuracy.HighPrecisionEpsilon);


### Constants
using Numerinus.Core.Constants;
double circumference = 2 * NumerinusConstants.Pi * radius; double growth        = Math.Pow(NumerinusConstants.E, x); double spiral        = NumerinusConstants.GoldenRatio * n;


---

## API Reference

### `IArithmetic<T>` — `Numerinus.Core.Interfaces`

The central contract every numeric type in the Numerinus suite implements.
public interface IArithmetic<T> where T : IArithmetic<T>


| Member | Kind | Description |
|---|---|---|
| `Add(T left, T right)` | Static method | Returns `left + right` |
| `Subtract(T left, T right)` | Static method | Returns `left - right` |
| `Multiply(T left, T right)` | Static method | Returns `left × right` |
| `Divide(T left, T right)` | Static method | Returns `left ÷ right` |
| `Zero` | Static property | Additive identity |
| `One` | Static property | Multiplicative identity |
| `IsZero(double epsilon)` | Instance method | Precision-aware zero check (default ε = `1e-15`) |

---

### `ComplexNumber` — `Numerinus.Core.Numerics`

Immutable class representing a complex number of the form **a + bi**.

| Member | Type | Description |
|---|---|---|
| `Real` | `double` | The real component `a` |
| `Imaginary` | `double` | The imaginary component `b` |
| `Zero` | `ComplexNumber` | `0 + 0i` |
| `One` | `ComplexNumber` | `1 + 0i` |
| `IsZero(epsilon)` | `bool` | True if both components are within epsilon of zero |
| `ToString()` | `string` | Returns `"a + bi"` |

> All arithmetic operations return a **new** instance — `ComplexNumber` is fully immutable.

---

### `Scalar` — `Numerinus.Core.Numerics`

Immutable `readonly struct` wrapping a `double`. Supports implicit conversion to and from `double`.

| Member | Type | Description |
|---|---|---|
| `Value` | `double` | The underlying value |
| `Zero` | `Scalar` | `Scalar(0)` |
| `One` | `Scalar` | `Scalar(1)` |
| `implicit operator Scalar(double)` | — | Allows `Scalar s = 5.0` |
| `implicit operator double(Scalar)` | — | Allows `double d = s` |

---

### `Accuracy` — `Numerinus.Core.Precision`

| Member | Value | Description |
|---|---|---|
| `StandardEpsilon` | `1e-15` | Default precision threshold |
| `HighPrecisionEpsilon` | `1e-18` | High-precision threshold |
| `AreEqual(a, b, epsilon)` | — | Returns `true` if `\|a − b\| < epsilon` |

---

### `NumerinusConstants` — `Numerinus.Core.Constants`

| Constant | Value | Description |
|---|---|---|
| `Pi` | `3.14159265358979323846` | π |
| `E` | `2.71828182845904523536` | Euler's number |
| `GoldenRatio` | `1.61803398874989484820` | φ (phi) |

---

## Writing Generic Algorithms

`IArithmetic<T>` enables fully generic numerical algorithms that work across any compatible type:

using Numerinus.Core.Interfaces;
static T Sum<T>(IEnumerable<T> items) where T : IArithmetic<T> { T result = T.Zero; foreach (var item in items) result = T.Add(result, item); return result; }
// Works with ComplexNumber var complexSum = Sum(new[] { new ComplexNumber(1, 2), new ComplexNumber(3, 4) }); // → 4 + 6i
// Works with Scalar Scalar scalarSum = Sum(new Scalar[] { 1.0, 2.0, 3.0 }); // → 6


---

## Related Modules

| Package | Description |
|---|---|
| [`Numerinus.Algebra`](https://www.nuget.org/packages/Numerinus.Algebra) | Matrix operations and linear algebra |
| [`Numerinus.Geometry`](https://www.nuget.org/packages/Numerinus.Geometry) | Geometric types and transformations |
| [`Numerinus.Statistics`](https://www.nuget.org/packages/Numerinus.Statistics) | Statistical functions and distributions |

---

## License

Copyright (c) 2026 Sunil Chaware. Licensed under the [MIT License](https://opensource.org/licenses/MIT).