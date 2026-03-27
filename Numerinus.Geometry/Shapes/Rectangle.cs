// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Shapes;

/// <summary>
/// Represents an immutable rectangle defined by width and height.
/// All four angles are 90°. Sides are aligned to axes.
/// </summary>
public class Rectangle
{
    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>The width of the rectangle (horizontal side).</summary>
    public Scalar Width { get; }

    /// <summary>The height of the rectangle (vertical side).</summary>
    public Scalar Height { get; }

    /// <param name="width">Width. Must be greater than zero.</param>
    /// <param name="height">Height. Must be greater than zero.</param>
    /// <exception cref="ArgumentException">Thrown if width or height is not positive.</exception>
    public Rectangle(Scalar width, Scalar height)
    {
        if (width.Value <= 0)
            throw new ArgumentException("Width must be greater than zero.");
        if (height.Value <= 0)
            throw new ArgumentException("Height must be greater than zero.");

        (Width, Height) = (width, height);
    }

    // -------------------------------------------------------------------------
    // Sides
    // -------------------------------------------------------------------------

    /// <summary>Length of the longer side.</summary>
    public Scalar LongerSide => new(Math.Max(Width.Value, Height.Value));

    /// <summary>Length of the shorter side.</summary>
    public Scalar ShorterSide => new(Math.Min(Width.Value, Height.Value));

    // -------------------------------------------------------------------------
    // Basic Properties
    // -------------------------------------------------------------------------

    /// <summary>Perimeter = 2 × (width + height)</summary>
    public Scalar Perimeter => new(2.0 * (Width.Value + Height.Value));

    /// <summary>Area = width × height</summary>
    public Scalar Area => new(Width.Value * Height.Value);

    // -------------------------------------------------------------------------
    // Diagonal
    // -------------------------------------------------------------------------

    /// <summary>
    /// Full diagonal length: d = √(width² + height²)
    /// The diagonal connects two opposite corners.
    /// </summary>
    public Scalar Diagonal => new(Math.Sqrt(Width.Value * Width.Value + Height.Value * Height.Value));

    /// <summary>
    /// Half diagonal: d / 2
    /// This is also the circumradius — the distance from the centre to any corner.
    /// </summary>
    public Scalar HalfDiagonal => new(Diagonal.Value / 2.0);

    /// <summary>
    /// Angle the diagonal makes with the width (in radians).
    /// θ = atan(height / width)
    /// </summary>
    public Scalar DiagonalAngleRadians => new(Math.Atan2(Height.Value, Width.Value));

    /// <summary>Angle the diagonal makes with the width in degrees.</summary>
    public Scalar DiagonalAngleDegrees => new(DiagonalAngleRadians.Value * 180.0 / NumerinusConstants.Pi);

    // -------------------------------------------------------------------------
    // Centre
    // -------------------------------------------------------------------------

    /// <summary>
    /// Distance from the centre to the longer side (the shorter half-dimension).
    /// For a rectangle placed at origin this equals Height / 2.
    /// This is also the inradius — the radius of the largest inscribed circle.
    /// </summary>
    public Scalar CentreToLongerSide => new(ShorterSide.Value / 2.0);

    /// <summary>
    /// Distance from the centre to the shorter side (the longer half-dimension).
    /// For a rectangle placed at origin this equals Width / 2.
    /// </summary>
    public Scalar CentreToShorterSide => new(LongerSide.Value / 2.0);

    // -------------------------------------------------------------------------
    // Incircle
    // -------------------------------------------------------------------------

    /// <summary>
    /// Inradius — radius of the largest circle that fits inside the rectangle,
    /// touching both longer sides.
    /// r = shorter side / 2
    /// </summary>
    public Scalar IncircleRadius => new(ShorterSide.Value / 2.0);

    /// <summary>
    /// Returns the incircle as a Circle instance.
    /// The circle touches both longer sides and is centred in the rectangle.
    /// </summary>
    public Circle Incircle => new(IncircleRadius);

    // -------------------------------------------------------------------------
    // Circumcircle
    // -------------------------------------------------------------------------

    /// <summary>
    /// Circumradius — radius of the circle passing through all four corners.
    /// R = diagonal / 2 = √(width² + height²) / 2
    /// </summary>
    public Scalar CircumcircleRadius => HalfDiagonal;

    /// <summary>
    /// Returns the circumcircle as a Circle instance.
    /// The circle passes through all four corners of the rectangle.
    /// </summary>
    public Circle Circumcircle => new(CircumcircleRadius);

    // -------------------------------------------------------------------------
    // Classification
    // -------------------------------------------------------------------------

    /// <summary>True if width equals height (i.e. this rectangle is a square).</summary>
    public bool IsSquare => Math.Abs(Width.Value - Height.Value) < 1e-15;

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------

    /// <summary>Creates a rectangle from a known area and width. height = area / width</summary>
    public static Rectangle FromAreaAndWidth(Scalar area, Scalar width)
    {
        if (area.Value <= 0) throw new ArgumentException("Area must be greater than zero.");
        if (width.Value <= 0) throw new ArgumentException("Width must be greater than zero.");
        return new(width, new(area.Value / width.Value));
    }

    /// <summary>Creates a rectangle from a known perimeter and width. height = (perimeter/2) - width</summary>
    public static Rectangle FromPerimeterAndWidth(Scalar perimeter, Scalar width)
    {
        if (perimeter.Value <= 0) throw new ArgumentException("Perimeter must be greater than zero.");
        if (width.Value <= 0) throw new ArgumentException("Width must be greater than zero.");
        double height = perimeter.Value / 2.0 - width.Value;
        if (height <= 0)
            throw new ArgumentException("Width is too large for the given perimeter.");
        return new(width, new(height));
    }

    /// <summary>Creates a rectangle from a known diagonal and width. height = √(diagonal² - width²)</summary>
    public static Rectangle FromDiagonalAndWidth(Scalar diagonal, Scalar width)
    {
        if (diagonal.Value <= 0) throw new ArgumentException("Diagonal must be greater than zero.");
        if (width.Value <= 0) throw new ArgumentException("Width must be greater than zero.");
        double h2 = diagonal.Value * diagonal.Value - width.Value * width.Value;
        if (h2 <= 0)
            throw new ArgumentException("Width is larger than or equal to the diagonal.");
        return new(width, new(Math.Sqrt(h2)));
    }

    public override string ToString() =>
        $"Rectangle({Width}×{Height}) | Area={Area}, Perimeter={Perimeter}, Diagonal={Diagonal}";
}