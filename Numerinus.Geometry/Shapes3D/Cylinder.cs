// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Shapes;

/// <summary>
/// Represents an immutable right circular cylinder defined by its radius and height.
/// Provides volume, surface area, and geometric property calculations.
/// </summary>
public sealed class Cylinder
{
    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>The radius of the circular base.</summary>
    public Scalar Radius { get; }

    /// <summary>The height (altitude) of the cylinder.</summary>
    public Scalar Height { get; }

    /// <param name="radius">The base radius. Must be greater than zero.</param>
    /// <param name="height">The height. Must be greater than zero.</param>
    /// <exception cref="ArgumentException">Thrown if radius or height is not positive.</exception>
    public Cylinder(Scalar radius, Scalar height)
    {
        if (radius.Value <= 0)
            throw new ArgumentException("Radius must be greater than zero.");
        if (height.Value <= 0)
            throw new ArgumentException("Height must be greater than zero.");
        Radius = radius;
        Height = height;
    }

    // -------------------------------------------------------------------------
    // Factory Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates a cylinder from a known diameter and height.
    /// r = d / 2
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if diameter or height is not positive.</exception>
    public static Cylinder FromDiameter(Scalar diameter, Scalar height)
    {
        if (diameter.Value <= 0)
            throw new ArgumentException("Diameter must be greater than zero.");
        return new(new(diameter.Value / 2.0), height);
    }

    /// <summary>
    /// Creates a cylinder from a known volume and height.
    /// r = √(V / (π · h))
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if volume or height is not positive.</exception>
    public static Cylinder FromVolumeAndHeight(Scalar volume, Scalar height)
    {
        if (volume.Value <= 0)
            throw new ArgumentException("Volume must be greater than zero.");
        if (height.Value <= 0)
            throw new ArgumentException("Height must be greater than zero.");
        return new(new(Math.Sqrt(volume.Value / (NumerinusConstants.Pi * height.Value))), height);
    }

    /// <summary>
    /// Creates a cylinder from a known volume and radius.
    /// h = V / (π · r²)
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if volume or radius is not positive.</exception>
    public static Cylinder FromVolumeAndRadius(Scalar volume, Scalar radius)
    {
        if (volume.Value <= 0)
            throw new ArgumentException("Volume must be greater than zero.");
        if (radius.Value <= 0)
            throw new ArgumentException("Radius must be greater than zero.");
        return new(radius, new(volume.Value / (NumerinusConstants.Pi * radius.Value * radius.Value)));
    }

    /// <summary>
    /// Creates a cylinder from a known lateral surface area and height.
    /// r = A / (2π · h)
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if lateral surface area or height is not positive.</exception>
    public static Cylinder FromLateralSurfaceAreaAndHeight(Scalar lateralSurfaceArea, Scalar height)
    {
        if (lateralSurfaceArea.Value <= 0)
            throw new ArgumentException("Lateral surface area must be greater than zero.");
        if (height.Value <= 0)
            throw new ArgumentException("Height must be greater than zero.");
        return new(new(lateralSurfaceArea.Value / (2.0 * NumerinusConstants.Pi * height.Value)), height);
    }

    // -------------------------------------------------------------------------
    // Basic Properties
    // -------------------------------------------------------------------------

    /// <summary>Diameter of the base: d = 2r</summary>
    public Scalar Diameter => new(2.0 * Radius.Value);

    /// <summary>
    /// Base circumference: C = 2πr
    /// The perimeter of the circular base.
    /// </summary>
    public Scalar BaseCircumference => new(2.0 * NumerinusConstants.Pi * Radius.Value);

    // -------------------------------------------------------------------------
    // Volume & Surface
    // -------------------------------------------------------------------------

    /// <summary>
    /// Volume: V = π · r² · h
    /// </summary>
    public Scalar Volume => new(NumerinusConstants.Pi * Radius.Value * Radius.Value * Height.Value);

    /// <summary>
    /// Base area (area of one circular cap): A = π · r²
    /// </summary>
    public Scalar BaseArea => new(NumerinusConstants.Pi * Radius.Value * Radius.Value);

    /// <summary>
    /// Lateral (curved) surface area: A = 2π · r · h
    /// The area of the curved side, excluding the two circular caps.
    /// </summary>
    public Scalar LateralSurfaceArea => new(2.0 * NumerinusConstants.Pi * Radius.Value * Height.Value);

    /// <summary>
    /// Total surface area: A = 2π · r · (r + h)
    /// Includes the lateral surface and both circular caps.
    /// </summary>
    public Scalar SurfaceArea => new(2.0 * NumerinusConstants.Pi * Radius.Value * (Radius.Value + Height.Value));

    // -------------------------------------------------------------------------
    // Diagonal
    // -------------------------------------------------------------------------

    /// <summary>
    /// Diagonal of the cylinder — the longest straight line that fits inside:
    /// d = √(4r² + h²)
    /// Connects two points on opposite base circumferences passing through the interior.
    /// </summary>
    public Scalar Diagonal => new(Math.Sqrt(4.0 * Radius.Value * Radius.Value + Height.Value * Height.Value));

    /// <summary>
    /// Slant height — the straight-line distance along the curved surface from
    /// the edge of the top cap to the edge of the bottom cap:
    /// l = √(r² + h²)
    /// Equivalent to the hypotenuse of the axial right triangle.
    /// </summary>
    public Scalar SlantHeight => new(Math.Sqrt(Radius.Value * Radius.Value + Height.Value * Height.Value));

    // -------------------------------------------------------------------------
    // Inscribed & Circumscribed Spheres
    // -------------------------------------------------------------------------

    /// <summary>
    /// Insphere radius — radius of the largest sphere that fits inside the cylinder.
    /// The insphere exists only when the diameter equals the height (2r = h); in the
    /// general case the limiting dimension is min(r, h/2).
    /// ρ = min(r, h/2)
    /// </summary>
    public Scalar InsphereRadius => new(Math.Min(Radius.Value, Height.Value / 2.0));

    /// <summary>
    /// Circumsphere radius — radius of the smallest sphere that contains the cylinder,
    /// passing through all points on both base circles.
    /// R = √(r² + (h/2)²)
    /// </summary>
    public Scalar CircumsphereRadius => new(Math.Sqrt(Radius.Value * Radius.Value + Math.Pow(Height.Value / 2.0, 2)));

    /// <summary>Returns the circumsphere as a <see cref="Sphere"/> instance.</summary>
    public Sphere Circumsphere => new(CircumsphereRadius);

    // -------------------------------------------------------------------------
    // Base Shapes
    // -------------------------------------------------------------------------

    /// <summary>Returns the base of the cylinder as a <see cref="Circle"/> instance.</summary>
    public Circle Base => new(Radius);

    // -------------------------------------------------------------------------
    // Angles
    // -------------------------------------------------------------------------

    /// <summary>
    /// Apex half-angle — the half-angle at the axis formed by the slant height
    /// and the axis of the cylinder (always 90° for a right cylinder).
    /// Provided here as the angle between the diagonal and the axis:
    /// θ = arctan(2r / h)  (in degrees)
    /// </summary>
    public Scalar DiagonalToAxisAngleDegrees =>
        new(Math.Atan2(2.0 * Radius.Value, Height.Value) * 180.0 / NumerinusConstants.Pi);

    // -------------------------------------------------------------------------
    // String
    // -------------------------------------------------------------------------

    public override string ToString() =>
        $"Cylinder(radius={Radius}, height={Height}) | Volume={Volume}, SurfaceArea={SurfaceArea}, LateralSurfaceArea={LateralSurfaceArea}";
}
