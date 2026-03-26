// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Shapes;

/// <summary>
/// Represents an immutable circle defined by its radius.
/// Provides area, circumference, and arc calculations.
/// </summary>
public sealed class Circle
{
    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>The radius of the circle.</summary>
    public Scalar Radius { get; }

    /// <param name="radius">The radius. Must be greater than zero.</param>
    /// <exception cref="ArgumentException">Thrown if radius is not positive.</exception>
    public Circle(Scalar radius)
    {
        if (radius.Value <= 0)
            throw new ArgumentException("Radius must be greater than zero.");
        Radius = radius;
    }

    /// <summary>
    /// Creates a circle from a known diameter.
    /// r = d / 2
    /// </summary>
    public static Circle FromDiameter(Scalar diameter)
    {
        if (diameter.Value <= 0)
            throw new ArgumentException("Diameter must be greater than zero.");
        return new(diameter / 2.0);
    }

    /// <summary>
    /// Creates a circle from a known circumference.
    /// r = C / (2π)
    /// </summary>
    public static Circle FromCircumference(Scalar circumference)
    {
        if (circumference.Value <= 0)
            throw new ArgumentException("Circumference must be greater than zero.");
        return new(circumference.Value / (2.0 * NumerinusConstants.Pi));
    }

    /// <summary>
    /// Creates a circle from a known area.
    /// r = √(A / π)
    /// </summary>
    public static Circle FromArea(Scalar area)
    {
        if (area.Value <= 0)
            throw new ArgumentException("Area must be greater than zero.");
        return new(Math.Sqrt(area.Value / NumerinusConstants.Pi));
    }

    // -------------------------------------------------------------------------
    // Basic Properties
    // -------------------------------------------------------------------------

    /// <summary>Diameter = 2r</summary>
    public Scalar Diameter => new(2.0 * Radius.Value);

    /// <summary>Circumference (perimeter) = 2πr</summary>
    public Scalar Circumference => new(2.0 * NumerinusConstants.Pi * Radius.Value);

    /// <summary>Area of the full circle = πr²</summary>
    public Scalar Area => new(NumerinusConstants.Pi * Radius.Value * Radius.Value);

    // -------------------------------------------------------------------------
    // Arc — angle in radians
    // -------------------------------------------------------------------------

    /// <summary>
    /// Length of an arc for a given central angle θ (radians).
    /// Arc Length = r × θ
    /// </summary>
    /// <param name="angleRadians">Central angle in radians. Must be between 0 and 2π.</param>
    public Scalar ArcLength(Scalar angleRadians)
    {
        ValidateAngleRadians(angleRadians);
        return new(Radius.Value * angleRadians.Value);
    }

    /// <summary>
    /// Area of a circular sector for a given central angle θ (radians).
    /// Sector Area = ½ × r² × θ
    /// </summary>
    /// <param name="angleRadians">Central angle in radians. Must be between 0 and 2π.</param>
    public Scalar ArcArea(Scalar angleRadians)
    {
        ValidateAngleRadians(angleRadians);
        return new(0.5 * Radius.Value * Radius.Value * angleRadians.Value);
    }

    /// <summary>
    /// Total boundary length of a sector (arc + two radii).
    /// Sector Boundary = Arc Length + 2r
    /// </summary>
    /// <param name="angleRadians">Central angle in radians. Must be between 0 and 2π.</param>
    public Scalar ArcPlusRadiusLength(Scalar angleRadians)
    {
        ValidateAngleRadians(angleRadians);
        return new(ArcLength(angleRadians).Value + 2.0 * Radius.Value);
    }

    // -------------------------------------------------------------------------
    // Arc — angle in degrees (convenience overloads)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Length of an arc for a given central angle (degrees).
    /// Arc Length = r × (θ° × π / 180)
    /// </summary>
    public Scalar ArcLengthDegrees(Scalar angleDegrees)
        => ArcLength(ToRadians(angleDegrees));

    /// <summary>
    /// Area of a circular sector for a given central angle (degrees).
    /// </summary>
    public Scalar ArcAreaDegrees(Scalar angleDegrees)
        => ArcArea(ToRadians(angleDegrees));

    /// <summary>
    /// Total boundary length of a sector (arc + two radii) for a given central angle (degrees).
    /// </summary>
    public Scalar ArcPlusRadiusLengthDegrees(Scalar angleDegrees)
        => ArcPlusRadiusLength(ToRadians(angleDegrees));

    // -------------------------------------------------------------------------
    // Reverse — find angle from arc length
    // -------------------------------------------------------------------------

    /// <summary>
    /// Returns the central angle in radians for a given arc length.
    /// θ = Arc Length / r
    /// </summary>
    /// <param name="arcLength">Arc length. Must be positive and ≤ circumference.</param>
    public Scalar AngleFromArcLength(Scalar arcLength)
    {
        if (arcLength.Value <= 0)
            throw new ArgumentException("Arc length must be greater than zero.");
        if (arcLength.Value > Circumference.Value + 1e-15)
            throw new ArgumentException("Arc length cannot exceed the full circumference.");
        return new(arcLength.Value / Radius.Value);
    }

    /// <summary>
    /// Returns the central angle in degrees for a given arc length.
    /// </summary>
    public Scalar AngleDegreesFromArcLength(Scalar arcLength)
        => new(AngleFromArcLength(arcLength).Value * 180.0 / NumerinusConstants.Pi);

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static Scalar ToRadians(Scalar degrees)
        => new(degrees.Value * NumerinusConstants.Pi / 180.0);

    private static void ValidateAngleRadians(Scalar angleRadians)
    {
        if (angleRadians.Value <= 0)
            throw new ArgumentException("Angle must be greater than zero.");
        if (angleRadians.Value > 2.0 * NumerinusConstants.Pi + 1e-15)
            throw new ArgumentException("Angle cannot exceed 2π radians (full circle).");
    }

    public override string ToString() =>
        $"Circle(r={Radius}) | Area={Area}, Circumference={Circumference}";
}