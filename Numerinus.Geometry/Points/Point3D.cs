// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Numerics;
using Numerinus.Geometry.Vectors;

namespace Numerinus.Geometry.Points;

/// <summary>
/// Represents an immutable 3D point with X, Y and Z coordinates.
/// </summary>
public readonly struct Point3D : IEquatable<Point3D>
{
    public Scalar X { get; }
    public Scalar Y { get; }
    public Scalar Z { get; }

    public Point3D(Scalar x, Scalar y, Scalar z) => (X, Y, Z) = (x, y, z);

    // --- Identity ---
    public static Point3D Origin => new(0, 0, 0);

    // --- Distance ---

    /// <summary>Returns the Euclidean distance between two points: √((x₂-x₁)² + (y₂-y₁)² + (z₂-z₁)²)</summary>
    public static Scalar Distance(Point3D a, Point3D b)
    {
        Scalar dx = a.X - b.X;
        Scalar dy = a.Y - b.Y;
        Scalar dz = a.Z - b.Z;
        return new(Math.Sqrt((dx * dx + dy * dy + dz * dz).Value));
    }

    /// <summary>Returns the distance from this point to another.</summary>
    public Scalar DistanceTo(Point3D other) => Distance(this, other);

    /// <summary>Returns the squared distance — faster when only comparison is needed, avoids sqrt.</summary>
    public static Scalar DistanceSquared(Point3D a, Point3D b)
    {
        Scalar dx = a.X - b.X;
        Scalar dy = a.Y - b.Y;
        Scalar dz = a.Z - b.Z;
        return dx * dx + dy * dy + dz * dz;
    }

    // --- Midpoint ---

    /// <summary>Returns the midpoint between two points: ((x₁+x₂)/2, (y₁+y₂)/2, (z₁+z₂)/2)</summary>
    public static Point3D Midpoint(Point3D a, Point3D b) =>
        new((a.X + b.X) / 2.0, (a.Y + b.Y) / 2.0, (a.Z + b.Z) / 2.0);

    /// <summary>Returns the midpoint between this point and another.</summary>
    public Point3D MidpointTo(Point3D other) => Midpoint(this, other);

    // --- Translation ---

    /// <summary>Translates the point by a given vector offset.</summary>
    public static Point3D operator +(Point3D p, Vector3 v) => new(p.X + v.X, p.Y + v.Y, p.Z + v.Z);

    /// <summary>Translates the point by the negation of a given vector offset.</summary>
    public static Point3D operator -(Point3D p, Vector3 v) => new(p.X - v.X, p.Y - v.Y, p.Z - v.Z);

    /// <summary>Returns the vector difference between two points: B - A</summary>
    public static Vector3 operator -(Point3D a, Point3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    // --- Conversion ---

    /// <summary>Converts this point to a position vector from the origin.</summary>
    public Vector3 ToVector3() => new(X, Y, Z);

    /// <summary>Creates a point from a position vector.</summary>
    public static Point3D FromVector3(Vector3 v) => new(v.X, v.Y, v.Z);

    // --- Equality ---
    public static bool operator ==(Point3D a, Point3D b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Point3D a, Point3D b) => !(a == b);
    public bool Equals(Point3D other) => this == other;
    public override bool Equals(object? obj) => obj is Point3D p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"({X}, {Y}, {Z})";
}