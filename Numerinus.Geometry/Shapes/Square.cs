// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Shapes;

/// <summary>
/// Represents an immutable square — a rectangle where all four sides are equal.
/// Inherits all Rectangle properties and adds square-specific convenience members.
/// </summary>
public sealed class Square : Rectangle
{
    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>The side length of the square.</summary>
    public Scalar Side => Width;

    /// <param name="side">Side length. Must be greater than zero.</param>
    public Square(Scalar side) : base(side, side) { }

    // -------------------------------------------------------------------------
    // Factory Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates a square from a known area.
    /// side = √area
    /// </summary>
    public static Square FromArea(Scalar area)
    {
        if (area.Value <= 0)
            throw new ArgumentException("Area must be greater than zero.");
        return new(new(Math.Sqrt(area.Value)));
    }

    /// <summary>
    /// Creates a square from a known perimeter.
    /// side = perimeter / 4
    /// </summary>
    public static Square FromPerimeter(Scalar perimeter)
    {
        if (perimeter.Value <= 0)
            throw new ArgumentException("Perimeter must be greater than zero.");
        return new(new(perimeter.Value / 4.0));
    }

    /// <summary>
    /// Creates a square from a known diagonal.
    /// side = diagonal / √2
    /// </summary>
    public static Square FromDiagonal(Scalar diagonal)
    {
        if (diagonal.Value <= 0)
            throw new ArgumentException("Diagonal must be greater than zero.");
        return new(new(diagonal.Value / Math.Sqrt(2.0)));
    }

    /// <summary>
    /// Creates a square from a known circumradius (centre to corner).
    /// side = R × √2
    /// </summary>
    public static Square FromCircumradius(Scalar circumradius)
    {
        if (circumradius.Value <= 0)
            throw new ArgumentException("Circumradius must be greater than zero.");
        return new(new(circumradius.Value * Math.Sqrt(2.0)));
    }

    /// <summary>
    /// Creates a square from a known inradius (centre to side).
    /// side = 2r
    /// </summary>
    public static Square FromInradius(Scalar inradius)
    {
        if (inradius.Value <= 0)
            throw new ArgumentException("Inradius must be greater than zero.");
        return new(new(inradius.Value * 2.0));
    }

    // -------------------------------------------------------------------------
    // Square-specific Properties
    // -------------------------------------------------------------------------

    /// <summary>
    /// Diagonal of a square: d = side × √2
    /// All four vertices lie on the circumcircle.
    /// </summary>
    public new Scalar Diagonal => new(Side.Value * Math.Sqrt(2.0));

    /// <summary>Half diagonal = side × √2 / 2 = side / √2</summary>
    public new Scalar HalfDiagonal => new(Diagonal.Value / 2.0);

    /// <summary>
    /// Inradius — distance from the centre to the midpoint of any side.
    /// r = side / 2
    /// All four sides are tangent to the incircle.
    /// </summary>
    public new Scalar IncircleRadius => new(Side.Value / 2.0);

    /// <summary>
    /// Circumradius — distance from the centre to any corner.
    /// R = side × √2 / 2
    /// All four corners lie on the circumcircle.
    /// </summary>
    public new Scalar CircumcircleRadius => HalfDiagonal;

    /// <summary>Returns the incircle — touches all four sides.</summary>
    public new Circle Incircle => new(IncircleRadius);

    /// <summary>Returns the circumcircle — passes through all four corners.</summary>
    public new Circle Circumcircle => new(CircumcircleRadius);

    /// <summary>
    /// Diagonal angle is always 45° for a square.
    /// </summary>
    public new Scalar DiagonalAngleDegrees => new(45.0);

    /// <summary>
    /// Diagonal angle in radians — always π/4 for a square.
    /// </summary>
    public new Scalar DiagonalAngleRadians => new(NumerinusConstants.Pi / 4.0);

    public override string ToString() =>
        $"Square(side={Side}) | Area={Area}, Perimeter={Perimeter}, Diagonal={Diagonal}";
}