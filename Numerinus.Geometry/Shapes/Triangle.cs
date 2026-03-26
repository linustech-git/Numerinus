// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;
using Numerinus.Geometry.Points;

namespace Numerinus.Geometry.Shapes;

/// <summary>
/// Represents an immutable triangle.
/// Can be constructed from three side lengths (SSS), two sides + included angle (SAS),
/// one side + two angles (AAS), or three 2D vertex positions.
/// When constructed from vertices, centroid, incircle and circumcircle centres are available.
/// </summary>
public sealed class Triangle
{
    // -------------------------------------------------------------------------
    // Vertices (optional — only set when constructed from Point2D)
    // -------------------------------------------------------------------------

    /// <summary>Vertex A. Only available when constructed from three Point2D positions.</summary>
    public Point2D? A { get; }

    /// <summary>Vertex B. Only available when constructed from three Point2D positions.</summary>
    public Point2D? B { get; }

    /// <summary>Vertex C. Only available when constructed from three Point2D positions.</summary>
    public Point2D? C { get; }

    /// <summary>True when this triangle was constructed from vertex positions.</summary>
    public bool HasVertices => A.HasValue && B.HasValue && C.HasValue;

    // -------------------------------------------------------------------------
    // Construction — three sides (SSS)
    // -------------------------------------------------------------------------

    /// <summary>Length of side A (opposite angle A / vertex A).</summary>
    public Scalar SideA { get; }

    /// <summary>Length of side B (opposite angle B / vertex B).</summary>
    public Scalar SideB { get; }

    /// <summary>Length of side C (opposite angle C / vertex C).</summary>
    public Scalar SideC { get; }

    /// <summary>
    /// Constructs a triangle from three side lengths (SSS).
    /// Centroid, incircle centre and circumcircle centre are NOT available.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if sides violate the triangle inequality.</exception>
    public Triangle(Scalar a, Scalar b, Scalar c)
    {
        if (a.Value <= 0 || b.Value <= 0 || c.Value <= 0)
            throw new ArgumentException("All sides must be greater than zero.");
        if (a.Value + b.Value <= c.Value ||
            b.Value + c.Value <= a.Value ||
            a.Value + c.Value <= b.Value)
            throw new ArgumentException("Sides do not satisfy the triangle inequality (a+b > c).");

        (SideA, SideB, SideC) = (a, b, c);
    }

    /// <summary>
    /// Constructs a triangle from three 2D vertex positions.
    /// Side lengths are derived from vertex distances.
    /// Centroid, incircle centre and circumcircle centre are fully available.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if vertices are collinear.</exception>
    public Triangle(Point2D a, Point2D b, Point2D c)
        : this(Point2D.Distance(b, c), Point2D.Distance(a, c), Point2D.Distance(a, b))
    {
        (A, B, C) = (a, b, c);
    }

    // -------------------------------------------------------------------------
    // Factory Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates a triangle from two sides and the included angle (SAS).
    /// c² = a² + b² - 2ab·cos(C)
    /// </summary>
    public static Triangle FromTwoSidesAndAngle(Scalar a, Scalar b, Scalar includedAngleRadians)
    {
        ValidateAngle(includedAngleRadians);
        double c = Math.Sqrt(
            a.Value * a.Value + b.Value * b.Value
            - 2.0 * a.Value * b.Value * Math.Cos(includedAngleRadians.Value));
        return new(a, b, new(c));
    }

    /// <summary>Creates a triangle from two sides and the included angle (SAS) in degrees.</summary>
    public static Triangle FromTwoSidesAndAngleDegrees(Scalar a, Scalar b, Scalar includedAngleDegrees)
        => FromTwoSidesAndAngle(a, b, ToRadians(includedAngleDegrees));

    /// <summary>
    /// Creates a triangle from one side and two adjacent angles (AAS).
    /// b = a·sin(B)/sin(A),  c = a·sin(C)/sin(A)
    /// </summary>
    public static Triangle FromOneSideAndTwoAngles(Scalar a, Scalar angleARadians, Scalar angleBRadians)
    {
        ValidateAngle(angleARadians);
        ValidateAngle(angleBRadians);

        double angleC = NumerinusConstants.Pi - angleARadians.Value - angleBRadians.Value;
        if (angleC <= 0)
            throw new ArgumentException("Angles A and B together must be less than 180°.");

        double sinA = Math.Sin(angleARadians.Value);
        if (Math.Abs(sinA) < 1e-15)
            throw new ArgumentException("Angle A cannot be 0° or 180°.");

        double b = a.Value * Math.Sin(angleBRadians.Value) / sinA;
        double c = a.Value * Math.Sin(angleC) / sinA;
        return new(a, new(b), new(c));
    }

    /// <summary>Creates a triangle from one side and two adjacent angles (AAS) in degrees.</summary>
    public static Triangle FromOneSideAndTwoAnglesDegrees(Scalar a, Scalar angleADegrees, Scalar angleBDegrees)
        => FromOneSideAndTwoAngles(a, ToRadians(angleADegrees), ToRadians(angleBDegrees));

    /// <summary>Creates an equilateral triangle from a single side length.</summary>
    public static Triangle Equilateral(Scalar side) => new(side, side, side);

    /// <summary>Creates an isoceles triangle from the base and equal leg length.</summary>
    public static Triangle Isoceles(Scalar base_, Scalar leg) => new(leg, leg, base_);

    /// <summary>Creates a right triangle from two legs — hypotenuse is computed.</summary>
    public static Triangle RightAngle(Scalar legA, Scalar legB)
        => new(legA, legB, new(Math.Sqrt(legA.Value * legA.Value + legB.Value * legB.Value)));

    // -------------------------------------------------------------------------
    // Basic Properties
    // -------------------------------------------------------------------------

    /// <summary>Perimeter = A + B + C</summary>
    public Scalar Perimeter => SideA + SideB + SideC;

    /// <summary>Semi-perimeter = (A + B + C) / 2</summary>
    public Scalar SemiPerimeter => Perimeter / 2.0;

    /// <summary>Area using Heron's formula: √(s(s-a)(s-b)(s-c))</summary>
    public Scalar Area
    {
        get
        {
            Scalar s = SemiPerimeter;
            double value = s.Value * (s - SideA).Value * (s - SideB).Value * (s - SideC).Value;
            return new(Math.Sqrt(value));
        }
    }

    // -------------------------------------------------------------------------
    // Angles (cosine rule)
    // -------------------------------------------------------------------------

    /// <summary>Angle A in radians. cos(A) = (b²+c²-a²) / 2bc</summary>
    public Scalar AngleARadians => new(Math.Acos(
        (SideB.Value * SideB.Value + SideC.Value * SideC.Value - SideA.Value * SideA.Value)
        / (2.0 * SideB.Value * SideC.Value)));

    /// <summary>Angle B in radians. cos(B) = (a²+c²-b²) / 2ac</summary>
    public Scalar AngleBRadians => new(Math.Acos(
        (SideA.Value * SideA.Value + SideC.Value * SideC.Value - SideB.Value * SideB.Value)
        / (2.0 * SideA.Value * SideC.Value)));

    /// <summary>Angle C in radians. Derived as π - A - B.</summary>
    public Scalar AngleCRadians => new(NumerinusConstants.Pi - AngleARadians.Value - AngleBRadians.Value);

    /// <summary>Angle A in degrees.</summary>
    public Scalar AngleADegrees => ToDegrees(AngleARadians);

    /// <summary>Angle B in degrees.</summary>
    public Scalar AngleBDegrees => ToDegrees(AngleBRadians);

    /// <summary>Angle C in degrees.</summary>
    public Scalar AngleCDegrees => ToDegrees(AngleCRadians);

    // -------------------------------------------------------------------------
    // Heights (Altitudes)
    // -------------------------------------------------------------------------

    /// <summary>Height to side A: h = 2·Area / a</summary>
    public Scalar HeightA => new(2.0 * Area.Value / SideA.Value);

    /// <summary>Height to side B: h = 2·Area / b</summary>
    public Scalar HeightB => new(2.0 * Area.Value / SideB.Value);

    /// <summary>Height to side C: h = 2·Area / c</summary>
    public Scalar HeightC => new(2.0 * Area.Value / SideC.Value);

    // -------------------------------------------------------------------------
    // Special Radii
    // -------------------------------------------------------------------------

    /// <summary>
    /// Circumradius — radius of circumscribed circle (passes through all 3 vertices).
    /// R = (a × b × c) / (4 × Area)
    /// </summary>
    public Scalar Circumradius => new(
        (SideA.Value * SideB.Value * SideC.Value) / (4.0 * Area.Value));

    /// <summary>
    /// Inradius — radius of inscribed circle (touches all 3 sides).
    /// r = Area / s
    /// </summary>
    public Scalar Inradius => new(Area.Value / SemiPerimeter.Value);

    // -------------------------------------------------------------------------
    // Centroid — requires vertices
    // -------------------------------------------------------------------------

    /// <summary>
    /// Centroid — intersection of the three medians.
    /// G = ((Ax+Bx+Cx)/3, (Ay+By+Cy)/3)
    /// Requires the triangle to be constructed from Point2D vertices.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if no vertex positions are available.</exception>
    public Point2D Centroid
    {
        get
        {
            RequireVertices(nameof(Centroid));
            Point2D a = A!.Value, b = B!.Value, c = C!.Value;
            return new((a.X + b.X + c.X) / 3.0, (a.Y + b.Y + c.Y) / 3.0);
        }
    }

    // -------------------------------------------------------------------------
    // Incircle — requires vertices for centre
    // -------------------------------------------------------------------------

    /// <summary>
    /// Incircle centre (incentre) — weighted average of vertices by opposite side.
    /// I = (a·A + b·B + c·C) / (a + b + c)
    /// Requires the triangle to be constructed from Point2D vertices.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if no vertex positions are available.</exception>
    public Point2D IncircleCentre
    {
        get
        {
            RequireVertices(nameof(IncircleCentre));
            Point2D va = A!.Value, vb = B!.Value, vc = C!.Value;
            double a = SideA.Value, b = SideB.Value, c = SideC.Value;
            double p = a + b + c;
            return new(
                new((a * va.X.Value + b * vb.X.Value + c * vc.X.Value) / p),
                new((a * va.Y.Value + b * vb.Y.Value + c * vc.Y.Value) / p));
        }
    }

    /// <summary>Incircle radius = Area / semi-perimeter. Available without vertices.</summary>
    public Scalar IncircleRadius => Inradius;

    /// <summary>
    /// Returns the incircle as a <see cref="Circle"/>.
    /// Available without vertices — only the centre requires them.
    /// </summary>
    public Circle Incircle => new(IncircleRadius);

    // -------------------------------------------------------------------------
    // Circumcircle — requires vertices for centre
    // -------------------------------------------------------------------------

    /// <summary>
    /// Circumcircle centre (circumcentre) — equidistant from all three vertices.
    /// Computed via perpendicular bisector intersection.
    /// Requires the triangle to be constructed from Point2D vertices.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if no vertex positions are available.</exception>
    public Point2D CircumcircleCentre
    {
        get
        {
            RequireVertices(nameof(CircumcircleCentre));
            double ax = A!.Value.X.Value, ay = A!.Value.Y.Value;
            double bx = B!.Value.X.Value, by = B!.Value.Y.Value;
            double cx = C!.Value.X.Value, cy = C!.Value.Y.Value;

            double d = 2.0 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));
            if (Math.Abs(d) < 1e-15)
                throw new InvalidOperationException("Triangle is degenerate — vertices are collinear.");

            double a2 = ax * ax + ay * ay;
            double b2 = bx * bx + by * by;
            double c2 = cx * cx + cy * cy;

            double ux = (a2 * (by - cy) + b2 * (cy - ay) + c2 * (ay - by)) / d;
            double uy = (a2 * (cx - bx) + b2 * (ax - cx) + c2 * (bx - ax)) / d;
            return new(new(ux), new(uy));
        }
    }

    /// <summary>Circumcircle radius = (a×b×c) / (4×Area). Available without vertices.</summary>
    public Scalar CircumcircleRadius => Circumradius;

    /// <summary>
    /// Returns the circumcircle as a <see cref="Circle"/>.
    /// Available without vertices — only the centre requires them.
    /// </summary>
    public Circle Circumcircle => new(CircumcircleRadius);

    // -------------------------------------------------------------------------
    // Classification
    // -------------------------------------------------------------------------

    /// <summary>True if all three sides are equal.</summary>
    public bool IsEquilateral =>
        Math.Abs(SideA.Value - SideB.Value) < 1e-15 &&
        Math.Abs(SideB.Value - SideC.Value) < 1e-15;

    /// <summary>True if at least two sides are equal.</summary>
    public bool IsIsoceles =>
        Math.Abs(SideA.Value - SideB.Value) < 1e-15 ||
        Math.Abs(SideB.Value - SideC.Value) < 1e-15 ||
        Math.Abs(SideA.Value - SideC.Value) < 1e-15;

    /// <summary>True if all three sides are different.</summary>
    public bool IsScalene => !IsIsoceles;

    /// <summary>True if one angle is exactly 90°.</summary>
    public bool IsRightAngle =>
        Math.Abs(SideA.Value * SideA.Value - (SideB.Value * SideB.Value + SideC.Value * SideC.Value)) < 1e-10 ||
        Math.Abs(SideB.Value * SideB.Value - (SideA.Value * SideA.Value + SideC.Value * SideC.Value)) < 1e-10 ||
        Math.Abs(SideC.Value * SideC.Value - (SideA.Value * SideA.Value + SideB.Value * SideB.Value)) < 1e-10;

    /// <summary>True if all angles are less than 90°.</summary>
    public bool IsAcute =>
        AngleARadians.Value < NumerinusConstants.Pi / 2.0 - 1e-15 &&
        AngleBRadians.Value < NumerinusConstants.Pi / 2.0 - 1e-15 &&
        AngleCRadians.Value < NumerinusConstants.Pi / 2.0 - 1e-15;

    /// <summary>True if one angle is greater than 90°.</summary>
    public bool IsObtuse => !IsRightAngle && !IsAcute;

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static Scalar ToRadians(Scalar degrees)
        => new(degrees.Value * NumerinusConstants.Pi / 180.0);

    private static Scalar ToDegrees(Scalar radians)
        => new(radians.Value * 180.0 / NumerinusConstants.Pi);

    private static void ValidateAngle(Scalar angleRadians)
    {
        if (angleRadians.Value <= 0 || angleRadians.Value >= NumerinusConstants.Pi)
            throw new ArgumentException("Angle must be between 0 and π radians (exclusive).");
    }

    private void RequireVertices(string memberName)
    {
        if (!HasVertices)
            throw new InvalidOperationException(
                $"{memberName} requires the triangle to be constructed from Point2D vertices.");
    }

    public override string ToString()
    {
        string sides = $"Triangle(a={SideA}, b={SideB}, c={SideC}) | " +
                       $"Area={Area}, Perimeter={Perimeter}, " +
                       $"Angles=({AngleADegrees}°, {AngleBDegrees}°, {AngleCDegrees}°)";

        return HasVertices
            ? sides + $" | Centroid={Centroid}, IncircleCentre={IncircleCentre}, CircumcircleCentre={CircumcircleCentre}"
            : sides;
    }
}