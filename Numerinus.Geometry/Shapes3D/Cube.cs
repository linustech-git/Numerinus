// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;

namespace Numerinus.Geometry.Shapes;

/// <summary>
/// Represents an immutable cube — a regular hexahedron where all six faces are equal squares.
/// All edges are equal, all angles are 90°.
/// </summary>
public sealed class Cube
{
    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>The edge (side) length of the cube.</summary>
    public Scalar Edge { get; }

    /// <param name="edge">Edge length. Must be greater than zero.</param>
    /// <exception cref="ArgumentException">Thrown if edge is not positive.</exception>
    public Cube(Scalar edge)
    {
        if (edge.Value <= 0)
            throw new ArgumentException("Edge length must be greater than zero.");
        Edge = edge;
    }

    // -------------------------------------------------------------------------
    // Factory Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates a cube from a known volume.
    /// edge = ?volume
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if volume is not positive.</exception>
    public static Cube FromVolume(Scalar volume)
    {
        if (volume.Value <= 0)
            throw new ArgumentException("Volume must be greater than zero.");
        return new(new(Math.Cbrt(volume.Value)));
    }

    /// <summary>
    /// Creates a cube from a known surface area.
    /// edge = ?(surfaceArea / 6)
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if surface area is not positive.</exception>
    public static Cube FromSurfaceArea(Scalar surfaceArea)
    {
        if (surfaceArea.Value <= 0)
            throw new ArgumentException("Surface area must be greater than zero.");
        return new(new(Math.Sqrt(surfaceArea.Value / 6.0)));
    }

    /// <summary>
    /// Creates a cube from a known space diagonal.
    /// edge = diagonal / ?3
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if diagonal is not positive.</exception>
    public static Cube FromSpaceDiagonal(Scalar diagonal)
    {
        if (diagonal.Value <= 0)
            throw new ArgumentException("Space diagonal must be greater than zero.");
        return new(new(diagonal.Value / Math.Sqrt(3.0)));
    }

    /// <summary>
    /// Creates a cube from a known face diagonal.
    /// edge = faceDiagonal / ?2
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if face diagonal is not positive.</exception>
    public static Cube FromFaceDiagonal(Scalar faceDiagonal)
    {
        if (faceDiagonal.Value <= 0)
            throw new ArgumentException("Face diagonal must be greater than zero.");
        return new(new(faceDiagonal.Value / Math.Sqrt(2.0)));
    }

    /// <summary>
    /// Creates a cube from a known insphere radius.
    /// edge = 2r
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if insphere radius is not positive.</exception>
    public static Cube FromInsphereRadius(Scalar insphereRadius)
    {
        if (insphereRadius.Value <= 0)
            throw new ArgumentException("Insphere radius must be greater than zero.");
        return new(new(insphereRadius.Value * 2.0));
    }

    /// <summary>
    /// Creates a cube from a known circumsphere radius.
    /// edge = 2R / ?3
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if circumsphere radius is not positive.</exception>
    public static Cube FromCircumsphereRadius(Scalar circumsphereRadius)
    {
        if (circumsphereRadius.Value <= 0)
            throw new ArgumentException("Circumsphere radius must be greater than zero.");
        return new(new(2.0 * circumsphereRadius.Value / Math.Sqrt(3.0)));
    }

    // -------------------------------------------------------------------------
    // Volume & Surface
    // -------------------------------------------------------------------------

    /// <summary>
    /// Volume of the cube: V = edge³
    /// </summary>
    public Scalar Volume => new(Math.Pow(Edge.Value, 3));

    /// <summary>
    /// Total surface area: A = 6 × edge²
    /// The cube has 6 equal square faces.
    /// </summary>
    public Scalar SurfaceArea => new(6.0 * Edge.Value * Edge.Value);

    /// <summary>
    /// Lateral surface area (4 side faces): A = 4 × edge²
    /// Excludes the top and bottom faces.
    /// </summary>
    public Scalar LateralSurfaceArea => new(4.0 * Edge.Value * Edge.Value);

    /// <summary>
    /// Area of a single face: edge²
    /// </summary>
    public Scalar FaceArea => new(Edge.Value * Edge.Value);

    // -------------------------------------------------------------------------
    // Diagonals
    // -------------------------------------------------------------------------

    /// <summary>
    /// Face diagonal — diagonal across one square face: d = edge × ?2
    /// </summary>
    public Scalar FaceDiagonal => new(Edge.Value * Math.Sqrt(2.0));

    /// <summary>
    /// Space diagonal — longest diagonal through the interior: d = edge × ?3
    /// Connects two opposite vertices of the cube.
    /// </summary>
    public Scalar SpaceDiagonal => new(Edge.Value * Math.Sqrt(3.0));

    // -------------------------------------------------------------------------
    // Edges & Faces
    // -------------------------------------------------------------------------

    /// <summary>Total number of edges on a cube: 12</summary>
    public int EdgeCount => 12;

    /// <summary>Total number of faces on a cube: 6</summary>
    public int FaceCount => 6;

    /// <summary>Total number of vertices (corners) on a cube: 8</summary>
    public int VertexCount => 8;

    /// <summary>
    /// Total edge length: 12 × edge
    /// Sum of all 12 edges.
    /// </summary>
    public Scalar TotalEdgeLength => new(12.0 * Edge.Value);

    // -------------------------------------------------------------------------
    // Inscribed & Circumscribed Spheres
    // -------------------------------------------------------------------------

    /// <summary>
    /// Insphere radius — radius of the largest sphere that fits inside the cube,
    /// touching all six faces at their centres.
    /// r = edge / 2
    /// </summary>
    public Scalar InsphereRadius => new(Edge.Value / 2.0);

    /// <summary>
    /// Midsphere radius — radius of the sphere that is tangent to every edge
    /// at its midpoint.
    /// ? = edge × ?2 / 2
    /// </summary>
    public Scalar MidsphereRadius => new(Edge.Value * Math.Sqrt(2.0) / 2.0);

    /// <summary>
    /// Circumsphere radius — radius of the smallest sphere that contains the cube,
    /// passing through all eight vertices.
    /// R = edge × ?3 / 2
    /// </summary>
    public Scalar CircumsphereRadius => new(Edge.Value * Math.Sqrt(3.0) / 2.0);

    /// <summary>Returns the insphere as a <see cref="Sphere"/> instance.</summary>
    public Sphere Insphere => new(InsphereRadius);

    /// <summary>Returns the midsphere as a <see cref="Sphere"/> instance.</summary>
    public Sphere Midsphere => new(MidsphereRadius);

    /// <summary>Returns the circumsphere as a <see cref="Sphere"/> instance.</summary>
    public Sphere Circumsphere => new(CircumsphereRadius);

    // -------------------------------------------------------------------------
    // Face Shape
    // -------------------------------------------------------------------------

    /// <summary>Returns any one face of the cube as a <see cref="Square"/> instance.</summary>
    public Square Face => new(Edge);

    // -------------------------------------------------------------------------
    // Angles
    // -------------------------------------------------------------------------

    /// <summary>
    /// Angle between a space diagonal and any face diagonal sharing the same vertex.
    /// ? = arccos(?2 / ?3) ? 35.26°
    /// </summary>
    public Scalar SpaceToFaceDiagonalAngleDegrees =>
        new(Math.Acos(Math.Sqrt(2.0) / Math.Sqrt(3.0)) * 180.0 / NumerinusConstants.Pi);

    /// <summary>
    /// Angle between a space diagonal and any edge sharing the same vertex.
    /// ? = arccos(1 / ?3) ? 54.74°
    /// </summary>
    public Scalar SpaceToEdgeAngleDegrees =>
        new(Math.Acos(1.0 / Math.Sqrt(3.0)) * 180.0 / NumerinusConstants.Pi);

    // -------------------------------------------------------------------------
    // String
    // -------------------------------------------------------------------------

    public override string ToString() =>
        $"Cube(edge={Edge}) | Volume={Volume}, SurfaceArea={SurfaceArea}, SpaceDiagonal={SpaceDiagonal}";
}