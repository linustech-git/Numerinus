// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Vectors;

/// <summary>
/// Represents an immutable 3D vector with X, Y and Z components.
/// </summary>
public readonly struct Vector3 : IEquatable<Vector3>
{
    public Scalar X { get; }
    public Scalar Y { get; }
    public Scalar Z { get; }

    public Vector3(Scalar x, Scalar y, Scalar z) => (X, Y, Z) = (x, y, z);

    // --- Identity ---
    public static Vector3 Zero => new(0, 0, 0);
    public static Vector3 UnitX => new(1, 0, 0);
    public static Vector3 UnitY => new(0, 1, 0);
    public static Vector3 UnitZ => new(0, 0, 1);

    // --- Arithmetic Operators ---
    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vector3 operator *(Vector3 v, Scalar s) => new(v.X * s, v.Y * s, v.Z * s);
    public static Vector3 operator *(Scalar s, Vector3 v) => v * s;
    public static Vector3 operator /(Vector3 v, Scalar s) => new(v.X / s, v.Y / s, v.Z / s);
    public static Vector3 operator -(Vector3 v) => new(-v.X, -v.Y, -v.Z);

    // --- Vector Operations ---

    /// <summary>Returns the dot product: A · B = Ax*Bx + Ay*By + Az*Bz</summary>
    public static Scalar Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    /// <summary>
    /// Returns the cross product: A × B — a vector perpendicular to both A and B.
    /// Only defined in 3D.
    /// </summary>
    public static Vector3 Cross(Vector3 a, Vector3 b) => new(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X);

    /// <summary>Returns the squared magnitude, avoiding a costly sqrt when only comparison is needed.</summary>
    public Scalar MagnitudeSquared => Dot(this, this);

    /// <summary>Returns the magnitude (length) of the vector: √(x² + y² + z²)</summary>
    public Scalar Magnitude => new(Math.Sqrt(MagnitudeSquared.Value));

    /// <summary>Returns a unit vector in the same direction. Throws if the vector is zero.</summary>
    public Vector3 Normalise()
    {
        Scalar mag = Magnitude;
        if (mag.IsZero())
            throw new InvalidOperationException("Cannot normalise a zero vector.");
        return this / mag;
    }

    /// <summary>Returns the angle in radians between two vectors.</summary>
    public static Scalar AngleBetween(Vector3 a, Vector3 b)
    {
        Scalar denominator = new(a.Magnitude.Value * b.Magnitude.Value);
        if (denominator.IsZero())
            throw new InvalidOperationException("Cannot compute angle with a zero vector.");
        double cos = Math.Clamp(Dot(a, b).Value / denominator.Value, -1.0, 1.0);
        return new(Math.Acos(cos));
    }

    /// <summary>Returns the distance between two points represented as vectors.</summary>
    public static Scalar Distance(Vector3 a, Vector3 b) => (a - b).Magnitude;

    // --- Equality ---
    public static bool operator ==(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);
    public bool Equals(Vector3 other) => this == other;
    public override bool Equals(object? obj) => obj is Vector3 v && Equals(v);
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"({X}, {Y}, {Z})";
}