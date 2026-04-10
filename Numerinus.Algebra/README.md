# Numerinus.Algebra

Algebra module for the **Numerinus** mathematical suite. Built on top of `Numerinus.Core`, this package provides **polynomial operations**, **rational functions**, **linear systems**, and **simple arithmetic** for any numeric type.

---

## Features

- **`SimpleArithmetic<T>`** – Generic arithmetic operations (Add, Subtract, Multiply, Divide) for any numeric type
- **`Polynomial<T>`** – Universal polynomial support for `int`, `double`, `decimal`, `Scalar`, `ComplexNumber`, etc.
  - Polynomial arithmetic (Add, Subtract, Multiply)
  - Calculus: Derivative and Integral
  - Evaluation using Horner's method (numerically stable)
  - Pretty string representation

- **`PolynomialRootFinder`** – Multiple root-finding algorithms for `Polynomial<double>`
  - Newton-Raphson method (fast convergence with initial guess)
  - Bisection method (guaranteed convergence, requires bracketing)
  - Multi-root scanner (automatic root discovery in interval)

- **`RationalFunction<T>`** – Ratio of two polynomials: P(x) / Q(x)
  - Arithmetic operations with other rational functions
  - Quotient rule for derivatives
  - Pole detection (undefined points)
  - Zero detection (where function equals zero)

- **`LinearSystem`** – Solve simultaneous linear equations Ax = b
  - Gaussian elimination with partial pivoting
  - Detects unique, infinite, or inconsistent solutions
  - Identifies free variables for underdetermined systems
  - Numerically stable for ill-conditioned systems

- **`SimpleArithmetic` — Aggregation Operations** – Collection-based operations on `double[]`
  - `Sum` — total of all values
  - `Product` — product of all values
  - `Average` — arithmetic mean
  - `Min` / `Max` — smallest and largest value

---

## Polynomial Operations

### Overview

A polynomial is an expression of the form:
```
P(x) = a₀ + a₁x + a₂x² + a₃x³ + ... + aₙxⁿ
```

In Numerinus, coefficients are stored in **ascending order of degree**: `[a₀, a₁, a₂, ...]`

### Creating Polynomials

```csharp
using Numerinus.Algebra.Polynomials;

// Method 1: Using params array
var poly1 = new Polynomial<double>(1, 2, 3);  // 1 + 2x + 3x²

// Method 2: Using enumerable
var coeffs = new[] { 1.0, 2.0, 3.0 };
var poly2 = new Polynomial<double>(coeffs);

// Method 3: With different numeric types
var polyInt = new Polynomial<int>(1, 2, 3);
var polyDecimal = new Polynomial<decimal>(1m, 2m, 3m);
var polyScalar = new Polynomial<Scalar>(5.0, 2.0, 1.0);
```

### Polynomial Arithmetic

```csharp
var p1 = new Polynomial<double>(1, 2, 3);      // 1 + 2x + 3x²
var p2 = new Polynomial<double>(2, 1);          // 2 + x

// Addition: (1 + 2x + 3x²) + (2 + x) = 3 + 3x + 3x²
var sum = p1.Add(p2);

// Subtraction: (1 + 2x + 3x²) - (2 + x) = -1 + x + 3x²
var diff = p1.Subtract(p2);

// Multiplication: (1 + 2x + 3x²) × (2 + x) = 2 + 5x + 8x² + 3x³
var product = p1.Multiply(p2);
```

### Polynomial Evaluation

Polynomials are evaluated using **Horner's method**, which is both efficient and numerically stable:

```csharp
var poly = new Polynomial<double>(1, 2, 3);  // 1 + 2x + 3x²

// Evaluate at x = 5
double result = poly.Evaluate(5);
// P(5) = 1 + 2(5) + 3(5)² = 1 + 10 + 75 = 86

// Works with different types
var polyComplex = new Polynomial<ComplexNumber>(
    new ComplexNumber(1, 0),
    new ComplexNumber(2, 1)
);
var complexResult = polyComplex.Evaluate(new ComplexNumber(1, 1));
```

### Polynomial Calculus

#### Derivative

The derivative of P(x) = a₀ + a₁x + a₂x² + ... is:
```
P'(x) = a₁ + 2a₂x + 3a₃x² + ...
```

```csharp
var poly = new Polynomial<double>(1, 2, 3);  // 1 + 2x + 3x²
var derivative = poly.Derivative();           // 2 + 6x

// Verify: d/dx[1 + 2x + 3x²] = 2 + 6x ✓
Console.WriteLine(derivative);  // Output: 2 + 6x
```

#### Integral

The indefinite integral of P(x) is:
```
∫P(x)dx = a₀x + (a₁/2)x² + (a₂/3)x³ + ... + C
```
(Numerinus sets C = 0)

```csharp
var poly = new Polynomial<double>(1, 2, 3);  // 1 + 2x + 3x²
var integral = poly.Integral();               // 0 + x + x² + x³

// Verify: ∫(1 + 2x + 3x²)dx = x + x² + x³ (+ C) ✓
Console.WriteLine(integral);  // Output: 0 + x + x² + x³
```

### Polynomial Properties

```csharp
var poly = new Polynomial<double>(1, 0, 0, 2);  // 1 + 0x + 0x² + 2x³

// Degree: highest power with non-zero coefficient
int degree = poly.Degree;  // 3

// Coefficients: read-only list in ascending order
var coeffs = poly.Coefficients;  // [1, 0, 0, 2]

// String representation
Console.WriteLine(poly);  // Output: 1 + 2x³
```

---

## Root Finding

Finding zeros (roots) of polynomials is essential for solving equations and analyzing function behavior.

### Overview

For a polynomial P(x), we want to find values of x such that P(x) = 0.

Example: P(x) = x² - 4 has roots at x = -2 and x = 2

### Method 1: Newton-Raphson

**Best for:** Quick convergence when you have a good initial guess

**Algorithm:**
```
x_{n+1} = x_n - f(x_n) / f'(x_n)
```

```csharp
using Numerinus.Algebra.Polynomials;

// Find root of: P(x) = x² - 4
var poly = new Polynomial<double>(-4, 0, 1);

// Newton-Raphson starting from x = 1.5
double? root = PolynomialRootFinder.NewtonRaphson(
    poly,
    initialGuess: 1.5,
    maxIterations: 100,
    tolerance: 1e-10
);

Console.WriteLine(root);  // Output: ~2.0
```

**Pros:**
- Very fast convergence (quadratic)
- Works well near roots

**Cons:**
- Needs good initial guess
- May diverge with poor guess
- Fails if derivative is zero

### Method 2: Bisection

**Best for:** Guaranteed convergence when you know a bracketing interval

**Algorithm:**
- Repeatedly split interval in half
- Keep the half containing the root
- Stop when interval is small enough

```csharp
// Find root of: P(x) = x² - 4
var poly = new Polynomial<double>(-4, 0, 1);

// Bisection on interval [1, 3] (must have opposite signs at endpoints)
double? root = PolynomialRootFinder.Bisection(
    poly,
    a: 1.0,      // P(1) = -3 (negative)
    b: 3.0,      // P(3) = 5 (positive)
    maxIterations: 100,
    tolerance: 1e-10
);

Console.WriteLine(root);  // Output: ~2.0
```

**Pros:**
- Always converges (guaranteed)
- Doesn't need derivative
- Robust

**Cons:**
- Slower than Newton-Raphson
- Requires bracketing interval

### Method 3: Multi-Root Scanner

**Best for:** Finding all roots in an interval without prior knowledge

**Algorithm:**
- Sample polynomial at many points
- Detect sign changes (indicating roots)
- Use bisection on each interval with sign change

```csharp
// Find all roots of: P(x) = x³ - 6x² + 11x - 6
// (has roots at x = 1, 2, 3)
var poly = new Polynomial<double>(-6, 11, -6, 1);

var roots = PolynomialRootFinder.FindRoots(
    poly,
    searchMin: -5,
    searchMax: 5,
    samplePoints: 100,
    tolerance: 1e-10
);

foreach (var root in roots)
{
    Console.WriteLine($"Root: {root}");
}
// Output:
// Root: 1.0
// Root: 2.0
// Root: 3.0
```

---

## Rational Functions

A rational function is the ratio of two polynomials:
```
R(x) = P(x) / Q(x)
```

where P(x) is the numerator and Q(x) is the denominator.

### Creating Rational Functions

```csharp
using Numerinus.Algebra.Polynomials;

// Create: (x + 1) / (x² - 1)
var numerator = new Polynomial<double>(1, 1);        // 1 + x
var denominator = new Polynomial<double>(-1, 0, 1);  // -1 + x²
var rational = new RationalFunction<double>(numerator, denominator);
```

### Evaluation

```csharp
// Evaluate at x = 2
double result = rational.Evaluate(2);
// R(2) = (1 + 2) / (-1 + 4) = 3 / 3 = 1

// Safe evaluation (returns null at poles)
double? safeResult = rational.TryEvaluate(1);  // null (pole: denominator = 0)
```

### Arithmetic with Rational Functions

```csharp
var r1 = new RationalFunction<double>(
    new Polynomial<double>(1, 1),      // 1 + x
    new Polynomial<double>(1)          // 1
);

var r2 = new RationalFunction<double>(
    new Polynomial<double>(2),         // 2
    new Polynomial<double>(1)          // 1
);

// Addition: (1 + x)/1 + 2/1 = (1 + x + 2) = 3 + x
var sum = r1.Add(r2);

// Multiplication: [(1 + x)/1] × [2/1] = 2(1 + x)
var product = r1.Multiply(r2);

// Division: [(1 + x)/1] ÷ [2/1] = (1 + x)/2
var quotient = r1.Divide(r2);
```

### Derivatives (Quotient Rule)

The derivative of R(x) = P(x) / Q(x) is:
```
R'(x) = [P'(x)Q(x) - P(x)Q'(x)] / [Q(x)]²
```

```csharp
var rational = new RationalFunction<double>(
    new Polynomial<double>(1, 1),      // 1 + x
    new Polynomial<double>(-1, 0, 1)   // -1 + x²
);

var derivative = rational.Derivative();
// Uses quotient rule automatically
```

### Finding Poles and Zeros

**Poles** are x-values where the function is undefined (denominator = 0)  
**Zeros** are x-values where the function equals zero (numerator = 0)

```csharp
var rational = new RationalFunction<double>(
    new Polynomial<double>(1, 1),        // 1 + x (zero at x = -1)
    new Polynomial<double>(-1, 0, 1)     // -1 + x² (poles at x = ±1)
);

// Find poles
var poles = rational.GetPoles(-10, 10);
// Output: [-1, 1]

// Find zeros
var zeros = rational.GetZeros(-10, 10);
// Output: [-1]
```

---

## Linear Systems (Simultaneous Equations)

Solve systems of linear equations in the form **Ax = b** using Gaussian elimination.

### Overview

A system of m linear equations in n unknowns:
```
a₁₁x₁ + a₁₂x₂ + ... + a₁ₙxₙ = b₁
a₂₁x₁ + a₂₂x₂ + ... + a₂ₙxₙ = b₂
...
aₘ₁x₁ + aₘ₂x₂ + ... + aₘₙxₙ = bₘ
```

Can be written as: **Ax = b** where:
- A is m×n coefficient matrix
- x is n×1 unknown vector
- b is m×1 constant vector

### Creating a Linear System

```csharp
using Numerinus.Algebra;

// System:
// 2x + 3y = 8
// 4x - y = 10

// Method 1: Coefficient matrix and constants vector
double[][] coefficients = new[]
{
    new[] { 2.0, 3.0 },
    new[] { 4.0, -1.0 }
};
double[] constants = new[] { 8.0, 10.0 };

var system = new LinearSystem(coefficients, constants);

// Method 2: Augmented matrix [A|b]
double[][] augmented = new[]
{
    new[] { 2.0, 3.0, 8.0 },
    new[] { 4.0, -1.0, 10.0 }
};

var system2 = new LinearSystem(augmented);
```

### Solving Linear Systems

```csharp
var solution = system.Solve();

// Check solution type
if (solution.Type == SystemType.UniqueSolution)
{
    Console.WriteLine($"x = {solution.Solution[0]}");
    Console.WriteLine($"y = {solution.Solution[1]}");
    // Output:
    // x = 2
    // y = 0.333...
}
```

### Three Possible Outcomes

#### 1. Unique Solution

**Condition:** Number of independent equations = Number of unknowns

```csharp
// System:
// 2x + 3y = 8      (Line 1)
// 4x - y = 10      (Line 2)
// These lines intersect at one point: x = 2, y = 0.333...

var system = new LinearSystem(
    new[] { new[] { 2.0, 3.0 }, new[] { 4.0, -1.0 } },
    new[] { 8.0, 10.0 }
);

var solution = system.Solve();
// Type: UniqueSolution
// Solution: [2, 0.333...]
// Rank: 2
```

#### 2. Infinite Solutions

**Condition:** Equations are dependent (represent same line/plane)

```csharp
// System:
// 2x + 4y = 6      (Line 1)
// x + 2y = 3       (Line 2 = Line 1 / 2, same line)

var system = new LinearSystem(
    new[] { new[] { 2.0, 4.0 }, new[] { 1.0, 2.0 } },
    new[] { 6.0, 3.0 }
);

var solution = system.Solve();
// Type: InfiniteSolutions
// Free variables: [1] (y is free)
// Rank: 1
// Particular solution: [3, 0] (one of infinitely many)

Console.WriteLine($"Free variable: x{solution.FreeVariables[0] + 1}");  // x2 (y)
```

#### 3. Inconsistent (No Solution)

**Condition:** Equations are contradictory (parallel lines)

```csharp
// System:
// x + y = 1        (Line 1)
// x + y = 2        (Line 2, parallel but not same)

var system = new LinearSystem(
    new[] { new[] { 1.0, 1.0 }, new[] { 1.0, 1.0 } },
    new[] { 1.0, 2.0 }
);

var solution = system.Solve();
// Type: Inconsistent
// Solution: null
// No valid x and y satisfy both equations
```

### Solution Properties

```csharp
var solution = system.Solve();

// Solution vector (or particular solution for infinite cases)
double[] x = solution.Solution;

// System classification
SystemType type = solution.Type;

// Rank of coefficient matrix
int rank = solution.Rank;

// Free variables (for infinite solutions)
var freeVars = solution.FreeVariables;  // Indices of non-pivot columns

// Number of variables
int numVars = solution.NumVariables;

// Degrees of freedom
int dof = numVars - rank;
```

### Advanced Example: Underdetermined System

```csharp
// System (3 unknowns, 2 equations):
// x + 2y + 3z = 14
// 2x - y + z = 5

var system = new LinearSystem(
    new[] 
    { 
        new[] { 1.0, 2.0, 3.0 },
        new[] { 2.0, -1.0, 1.0 }
    },
    new[] { 14.0, 5.0 }
);

var solution = system.Solve();

if (solution.Type == SystemType.InfiniteSolutions)
{
    Console.WriteLine($"Rank: {solution.Rank}");           // 2
    Console.WriteLine($"Free variables: {solution.FreeVariables.Count}");  // 1
    Console.WriteLine($"Degrees of freedom: {solution.NumVariables - solution.Rank}");  // 1
    
    // One particular solution
    Console.WriteLine($"Particular solution: [{string.Join(", ", solution.Solution)}]");
}
```

---

## Aggregation Operations

`SimpleArithmetic` provides a set of collection-based operations that accept any number of `double` values via `params double[]`.

All methods throw `ArgumentException` if called with no values.

### Sum

Returns the total of all provided values.

```csharp
var arith = new SimpleArithmetic();

double total = arith.Sum(1, 2, 3, 4, 5);
Console.WriteLine(total); // 15

double[] values = { 10.5, 20.0, 30.5 };
double result = arith.Sum(values);
Console.WriteLine(result); // 61
```

### Product

Returns the product of all provided values.

```csharp
double product = arith.Product(1, 2, 3, 4, 5);
Console.WriteLine(product); // 120

double compound = arith.Product(1.05, 1.05, 1.05); // 3-year compound growth factor
Console.WriteLine(compound); // ~1.157625
```

### Average

Returns the arithmetic mean.

```csharp
double avg = arith.Average(10, 20, 30, 40, 50);
Console.WriteLine(avg); // 30

double examAvg = arith.Average(72, 85, 91, 68, 79);
Console.WriteLine(examAvg); // 79
```

### Min and Max

Return the smallest and largest values in the collection.

```csharp
double min = arith.Min(3, 1, 4, 1, 5, 9, 2, 6);
Console.WriteLine(min); // 1

double max = arith.Max(3, 1, 4, 1, 5, 9, 2, 6);
Console.WriteLine(max); // 9

// Useful for range detection
double[] data = { 14.2, 9.8, 22.1, 7.5, 18.0 };
Console.WriteLine($"Range: {arith.Min(data)} – {arith.Max(data)}"); // Range: 7.5 – 22.1
```

---

## API Reference

### `SimpleArithmetic` – `Numerinus.Algebra`

Arithmetic and aggregation operations on `double` values.

```csharp
// Basic arithmetic
public double Add(double a, double b)
public double Subtract(double a, double b)
public double Multiply(double a, double b)
public double Divide(double a, double b)       // throws DivideByZeroException if b == 0
public double Modulo(double a, double b)       // throws DivideByZeroException if b == 0
public double Power(double a, double b)
public double SquareRoot(double a)             // throws ArgumentOutOfRangeException if a < 0
public double AbsoluteValue(double a)

// Number theory
public double Factorial(int n)                 // throws ArgumentOutOfRangeException if n < 0
public long GreatestCommonDivisor(long a, long b)
public long LeastCommonMultiple(long a, long b)
public bool IsPrime(long n)

// Aggregation
public double Sum(params double[] values)
public double Product(params double[] values)
public double Average(params double[] values)
public double Min(params double[] values)
public double Max(params double[] values)
```

### `Polynomial<T>` – `Numerinus.Algebra.Polynomials`

Represents a polynomial with coefficients of type `T`.

```csharp
// Constructor
var poly = new Polynomial<T>(params T[] coefficients);
var poly = new Polynomial<T>(IEnumerable<T> coefficients);

// Properties
int Degree { get; }  // Highest power with non-zero coefficient (-1 for zero polynomial)
IReadOnlyList<T> Coefficients { get; }

// Methods
T Evaluate(T x)
Polynomial<T> Add(Polynomial<T> other)
Polynomial<T> Subtract(Polynomial<T> other)
Polynomial<T> Multiply(Polynomial<T> other)
Polynomial<T> Derivative()
Polynomial<T> Integral()
```

### `PolynomialRootFinder` – `Numerinus.Algebra.Polynomials`

Static utility for finding roots of `Polynomial<double>`.

```csharp
// Newton-Raphson method
double? root = PolynomialRootFinder.NewtonRaphson(
    polynomial, 
    initialGuess, 
    maxIterations: 100, 
    tolerance: 1e-10
);

// Bisection method (requires bracketing interval)
double? root = PolynomialRootFinder.Bisection(
    polynomial, 
    a, 
    b, 
    maxIterations: 100, 
    tolerance: 1e-10
);

// Find multiple roots
List<double> roots = PolynomialRootFinder.FindRoots(
    polynomial, 
    searchMin, 
    searchMax, 
    samplePoints: 100, 
    tolerance: 1e-10
);
```

### `RationalFunction<T>` – `Numerinus.Algebra.Polynomials`

Represents a rational function P(x) / Q(x).

```csharp
// Constructor
var rational = new RationalFunction<T>(Polynomial<T> numerator, Polynomial<T> denominator);

// Properties
Polynomial<T> Numerator { get; }
Polynomial<T> Denominator { get; }

// Methods
T Evaluate(T x)
T? TryEvaluate(T x, double tolerance = 1e-10)
RationalFunction<T> Add(RationalFunction<T> other)
RationalFunction<T> Subtract(RationalFunction<T> other)
RationalFunction<T> Multiply(RationalFunction<T> other)
RationalFunction<T> Divide(RationalFunction<T> other)
RationalFunction<T> Derivative()
List<double> GetPoles(double searchMin = -100, double searchMax = 100)
List<double> GetZeros(double searchMin = -100, double searchMax = 100)
```

### `LinearSystem` – `Numerinus.Algebra`

Solver for systems of linear equations using Gaussian elimination.

```csharp
// Constructors
var system = new LinearSystem(double[][] coefficients, double[] constants);
var system = new LinearSystem(double[][] augmentedMatrix);

// Properties
double[][] CoefficientMatrix { get; }
double[] ConstantsVector { get; }

// Methods
LinearSystemSolution Solve(double tolerance = 1e-10);
```

### `LinearSystemSolution` – `Numerinus.Algebra`

Result of solving a linear system.

```csharp
public enum SystemType
{
    UniqueSolution,
    InfiniteSolutions,
    Inconsistent
}

public class LinearSystemSolution
{
    public SystemType Type { get; }
    public double[] Solution { get; }  // null for inconsistent systems
    public int Rank { get; }
    public IReadOnlyList<int> FreeVariables { get; }
    public int NumVariables { get; }
}
```

---

## Dependencies

- **`Numerinus.Core`** – For `Scalar`, `ComplexNumber`, and `IArithmetic<T>` interface

---

## Related Modules

| Module | Description |
|---|---|
| `Numerinus.Core` | Foundational types: `IArithmetic<T>`, `Scalar`, `ComplexNumber` |
| `Numerinus.Geometry` | Geometric types and transformations |
| `Numerinus.Calculus` | Differentiation, integration, and calculus operations |
| `Numerinus.Statistics` | Statistical functions and distributions |

---

## Contributing

We welcome contributions! To contribute:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

---

## License

MIT – See LICENSE file for details.
