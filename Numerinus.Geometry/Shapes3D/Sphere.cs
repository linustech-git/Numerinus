// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Shapes;

/// <summary>
/// Represents an immutable sphere defined by its radius.
/// Provides volume, surface area, and cross-section calculations.
/// </summary>
public sealed class Sphere
{
    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>The radius of the sphere.</summary>
    public Scalar Radius { get; }

    /// <param name="radius">The radius. Must be greater than zero.</param>
    /// <exception cref="ArgumentException">Thrown if radius is not positive.</exception>
    public Sphere(Scalar radius)
    {
        if (radius.Value <= 0)
            throw new ArgumentException("Radius must be greater than zero.");
        Radius = radius;
    }

    // -------------------------------------------------------------------------
    // Factory Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates a sphere from a known diameter.
    /// r = d / 2
    /// </summary>
    public static Sphere FromDiameter(Scalar diameter)
    {
        if (diameter.Value <= 0)
            throw new ArgumentException("Diameter must be greater than zero.");
        return new(new(diameter.Value / 2.0));
    }

    /// <summary>
    /// Creates a sphere from a known volume.
    /// r = ∛(3V / 4π)
    /// </summary>
    public static Sphere FromVolume(Scalar volume)
    {
        if (volume.Value <= 0)
            throw new ArgumentException("Volume must be greater than zero.");
        return new(new(Math.Cbrt(3.0 * volume.Value / (4.0 * NumerinusConstants.Pi))));
    }

    /// <summary>
    /// Creates a sphere from a known surface area.
    /// r = √(A / 4π)
    /// </summary>
    public static Sphere FromSurfaceArea(Scalar surfaceArea)
    {
        if (surfaceArea.Value <= 0)
            throw new ArgumentException("Surface area must be greater than zero.");
        return new(new(Math.Sqrt(surfaceArea.Value / (4.0 * NumerinusConstants.Pi))));
    }

    // -------------------------------------------------------------------------
    // Basic Properties
    // -------------------------------------------------------------------------

    /// <summary>Diameter: d = 2r</summary>
    public Scalar Diameter => new(2.0 * Radius.Value);

    /// <summary>
    /// Surface area: A = 4πr²
    /// </summary>
    public Scalar SurfaceArea => new(4.0 * NumerinusConstants.Pi * Radius.Value * Radius.Value);

    /// <summary>
    /// Volume: V = (4/3)πr³
    /// </summary>
    public Scalar Volume => new((4.0 / 3.0) * NumerinusConstants.Pi * Math.Pow(Radius.Value, 3));

    /// <summary>
    /// Great circle circumference: C = 2πr
    /// The circumference of the largest circle that fits on the sphere's surface.
    /// </summary>
    public Scalar GreatCircleCircumference => new(2.0 * NumerinusConstants.Pi * Radius.Value);

    /// <summary>
    /// Great circle area: A = πr²
    /// The area of the largest circular cross-section through the centre.
    /// </summary>
    public Scalar GreatCircleArea => new(NumerinusConstants.Pi * Radius.Value * Radius.Value);

    /// <summary>Returns the great circle as a <see cref="Circle"/> instance.</summary>
    public Circle GreatCircle => new(Radius);

    // -------------------------------------------------------------------------
    // String
    // -------------------------------------------------------------------------

    public override string ToString() =>
        $"Sphere(radius={Radius}) | Volume={Volume}, SurfaceArea={SurfaceArea}";
}
