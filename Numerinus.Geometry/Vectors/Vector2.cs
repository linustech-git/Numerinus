// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Vectors;

/// <summary>
/// Represents an immutable 2D vector with X and Y components.
/// </summary>
public readonly struct Vector2 : IEquatable<Vector2>
{
    public Scalar X { get; }
    public Scalar Y { get; }

    public Vector2(Scalar x, Scalar y) => (X, Y) = (x, y);

    // --- Identity ---
    public static Vector2 Zero => new(0, 0);
    public static Vector2 UnitX => new(1, 0);
    public static Vector2 UnitY => new(0, 1);

    // --- Arithmetic Operators ---
    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 v, Scalar s) => new(v.X * s, v.Y * s);
    public static Vector2 operator *(Scalar s, Vector2 v) => v * s;
    public static Vector2 operator /(Vector2 v, Scalar s) => new(v.X / s, v.Y / s);
    public static Vector2 operator -(Vector2 v) => new(-v.X, -v.Y);

    // --- Vector Operations ---

    /// <summary>Returns the dot product of two vectors: A · B = Ax*Bx + Ay*By</summary>
    public static Scalar Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;

    /// <summary>Returns the squared magnitude, avoiding a costly sqrt when only comparison is needed.</summary>
    public Scalar MagnitudeSquared => Dot(this, this);

    /// <summary>Returns the magnitude (length) of the vector: √(x² + y²)</summary>
    public Scalar Magnitude => new(Math.Sqrt(MagnitudeSquared.Value));

    /// <summary>Returns a unit vector in the same direction. Throws if the vector is zero.</summary>
    public Vector2 Normalise()
    {
        Scalar mag = Magnitude;
        if (mag.IsZero())
            throw new InvalidOperationException("Cannot normalise a zero vector.");
        return this / mag;
    }

    /// <summary>Returns the angle in radians between two vectors.</summary>
    public static Scalar AngleBetween(Vector2 a, Vector2 b)
    {
        Scalar denominator = new(a.Magnitude.Value * b.Magnitude.Value);
        if (denominator.IsZero())
            throw new InvalidOperationException("Cannot compute angle with a zero vector.");
        double cos = Math.Clamp(Dot(a, b).Value / denominator.Value, -1.0, 1.0);
        return new(Math.Acos(cos));
    }

    /// <summary>Returns the distance between two points represented as vectors.</summary>
    public static Scalar Distance(Vector2 a, Vector2 b) => (a - b).Magnitude;

    // --- Equality ---
    public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
    public bool Equals(Vector2 other) => this == other;
    public override bool Equals(object? obj) => obj is Vector2 v && Equals(v);
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"({X}, {Y})";
}