// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Numerics;
using Numerinus.Geometry.Vectors;

namespace Numerinus.Geometry.Points;

/// <summary>
/// Represents an immutable 2D point with X and Y coordinates.
/// </summary>
public readonly struct Point2D : IEquatable<Point2D>
{
    public Scalar X { get; }
    public Scalar Y { get; }

    public Point2D(Scalar x, Scalar y) => (X, Y) = (x, y);

    // --- Identity ---
    public static Point2D Origin => new(0, 0);

    // --- Distance ---

    /// <summary>Returns the Euclidean distance between two points: √((x₂-x₁)² + (y₂-y₁)²)</summary>
    public static Scalar Distance(Point2D a, Point2D b)
    {
        Scalar dx = a.X - b.X;
        Scalar dy = a.Y - b.Y;
        return new(Math.Sqrt((dx * dx + dy * dy).Value));
    }

    /// <summary>Returns the distance from this point to another.</summary>
    public Scalar DistanceTo(Point2D other) => Distance(this, other);

    /// <summary>Returns the squared distance — faster when only comparison is needed, avoids sqrt.</summary>
    public static Scalar DistanceSquared(Point2D a, Point2D b)
    {
        Scalar dx = a.X - b.X;
        Scalar dy = a.Y - b.Y;
        return dx * dx + dy * dy;
    }

    // --- Midpoint ---

    /// <summary>Returns the midpoint between two points: ((x₁+x₂)/2, (y₁+y₂)/2)</summary>
    public static Point2D Midpoint(Point2D a, Point2D b) =>
        new((a.X + b.X) / 2.0, (a.Y + b.Y) / 2.0);

    /// <summary>Returns the midpoint between this point and another.</summary>
    public Point2D MidpointTo(Point2D other) => Midpoint(this, other);

    // --- Translation ---

    /// <summary>Translates the point by a given vector offset.</summary>
    public static Point2D operator +(Point2D p, Vector2 v) => new(p.X + v.X, p.Y + v.Y);

    /// <summary>Translates the point by the negation of a given vector offset.</summary>
    public static Point2D operator -(Point2D p, Vector2 v) => new(p.X - v.X, p.Y - v.Y);

    /// <summary>Returns the vector difference between two points: B - A</summary>
    public static Vector2 operator -(Point2D a, Point2D b) => new(a.X - b.X, a.Y - b.Y);

    // --- Conversion ---

    /// <summary>Converts this point to a position vector from the origin.</summary>
    public Vector2 ToVector2() => new(X, Y);

    /// <summary>Creates a point from a position vector.</summary>
    public static Point2D FromVector2(Vector2 v) => new(v.X, v.Y);

    // --- Equality ---
    public static bool operator ==(Point2D a, Point2D b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Point2D a, Point2D b) => !(a == b);
    public bool Equals(Point2D other) => this == other;
    public override bool Equals(object? obj) => obj is Point2D p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"({X}, {Y})";
}