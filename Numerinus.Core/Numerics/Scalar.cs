using System;
using Numerinus.Core.Interfaces;

namespace Numerinus.Core.Numerics;

public readonly struct Scalar : IArithmetic<Scalar>, IEquatable<Scalar>
{
    public double Value { get; }
    public Scalar(double value) => Value = value;

    // Implementation of the IArithmetic Rulebook
    public static Scalar Add(Scalar l, Scalar r) => new(l.Value + r.Value);
    public static Scalar Subtract(Scalar l, Scalar r) => new(l.Value - r.Value);
    public static Scalar Multiply(Scalar l, Scalar r) => new(l.Value * r.Value);
    public static Scalar Divide(Scalar l, Scalar r) => new(l.Value / r.Value);

    public static Scalar Zero => new(0);
    public static Scalar One => new(1);     

    public bool IsZero(double epsilon = 1e-15) => Math.Abs(Value) < epsilon;

    // THE MAGIC: Allows 'Scalar s = 5.0;' and 'double d = s;'
    public static implicit operator Scalar(double d) => new(d);
    public static implicit operator double(Scalar s) => s.Value;

    public override string ToString() => Value.ToString();

    // Operator overloads
    public static Scalar operator +(Scalar l, Scalar r) => Add(l, r);
    public static Scalar operator -(Scalar l, Scalar r) => Subtract(l, r);
    public static Scalar operator *(Scalar l, Scalar r) => Multiply(l, r);
    public static Scalar operator /(Scalar l, Scalar r) => Divide(l, r);

    public static Scalar operator +(Scalar v) => v;
    public static Scalar operator -(Scalar v) => new(-v.Value);

    // Equality
    public static bool operator ==(Scalar l, Scalar r) => l.Value == r.Value;
    public static bool operator !=(Scalar l, Scalar r) => !(l == r);

    public bool Equals(Scalar other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is Scalar s && Equals(s);
    public override int GetHashCode() => HashCode.Combine(Value);
}
