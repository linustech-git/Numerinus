# Numerinus.Geometry

Geometry module for the **Numerinus** mathematical suite. Built on Numerinus.Core and Numerinus.Algebra, this package provides immutable 2D and 3D geometric types, shape calculations, and affine transformations.

[![NuGet](https://img.shields.io/nuget/v/Numerinus.Geometry)](https://www.nuget.org/packages/Numerinus.Geometry)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## Installation

dotnet add package Numerinus.Geometry

---

## Project Structure

Numerinus.Geometry/
    Vectors/
        Vector2.cs      2D vector with X, Y components
        Vector3.cs      3D vector with X, Y, Z components
    Points/
        Point2D.cs      2D point with X, Y coordinates
        Point3D.cs      3D point with X, Y, Z coordinates
    Shapes/
        Circle.cs       Circle with area, circumference, arc calculations
        Sphere.cs       Sphere with volume, surface area, great circle
        Triangle.cs     Triangle with full geometric analysis
        Rectangle.cs    Rectangle with area, perimeter, diagonal, incircle, circumcircle
        Square.cs       Square extending Rectangle with side-specific formulas
        Cube.cs         Cube with volume, surface area, diagonals, insphere, circumsphere
        Cylinder.cs     Cylinder with volume, surface area, diagonal, insphere, circumsphere
    Transforms/
        Transform2D.cs  2D affine transformation using 3x3 homogeneous matrix

---

## Dependencies

This module depends on:
    Numerinus.Core      for Scalar, IArithmetic, NumerinusConstants
    Numerinus.Algebra   for Matrix(Scalar) used in Transform2D

---

## Quick Start

### Vector2

    using Numerinus.Geometry.Vectors;

    var v1 = new Vector2(3, 4);
    var v2 = new Vector2(1, 2);

    var sum     = v1 + v2;
    var scaled  = v1 * 2.0;
    var length  = v1.Magnitude;
    var unit    = v1.Normalise();
    var dot     = Vector2.Dot(v1, v2);
    var angle   = Vector2.AngleBetween(Vector2.UnitX, Vector2.UnitY);
    var dist    = Vector2.Distance(v1, v2);

### Vector3

    using Numerinus.Geometry.Vectors;

    var a = new Vector3(1, 0, 0);
    var b = new Vector3(0, 1, 0);

    var cross   = Vector3.Cross(a, b);
    var dot     = Vector3.Dot(a, b);
    var unit    = a.Normalise();

### Point2D

    using Numerinus.Geometry.Points;

    var p1 = new Point2D(0, 0);
    var p2 = new Point2D(3, 4);

    var dist    = p1.DistanceTo(p2);
    var mid     = p1.MidpointTo(p2);
    var distSq  = Point2D.DistanceSquared(p1, p2);
    var moved   = p1 + new Vector2(1, 1);
    var dir     = p2 - p1;

### Point3D

    using Numerinus.Geometry.Points;

    var p1 = new Point3D(0, 0, 0);
    var p2 = new Point3D(1, 2, 3);

    var dist = p1.DistanceTo(p2);
    var mid  = p1.MidpointTo(p2);

### Circle

    using Numerinus.Geometry.Shapes;

    var c = new Circle(5);

    var area            = c.Area;
    var circumference   = c.Circumference;
    var diameter        = c.Diameter;
    var arcLen          = c.ArcLengthDegrees(90);
    var arcArea         = c.ArcAreaDegrees(90);
    var arcBoundary     = c.ArcPlusRadiusLengthDegrees(90);
    var angle           = c.AngleDegreesFromArcLength(arcLen);

    var fromCirc    = Circle.FromCircumference(31.415);
    var fromArea    = Circle.FromArea(78.539);
    var fromDiam    = Circle.FromDiameter(10);

### Sphere

    using Numerinus.Geometry.Shapes;

    var s = new Sphere(5);

    var volume          = s.Volume;
    var surfaceArea     = s.SurfaceArea;
    var diameter        = s.Diameter;
    var greatCircle     = s.GreatCircle;
    var gcCircumference = s.GreatCircleCircumference;
    var gcArea          = s.GreatCircleArea;

    var fromDiam        = Sphere.FromDiameter(10);
    var fromVolume      = Sphere.FromVolume(523.6);
    var fromSurface     = Sphere.FromSurfaceArea(314.16);

### Rectangle

    using Numerinus.Geometry.Shapes;

    var r = new Rectangle(6, 4);

    var area                = r.Area;
    var perimeter           = r.Perimeter;
    var diagonal            = r.Diagonal;
    var halfDiagonal        = r.HalfDiagonal;
    var diagAngle           = r.DiagonalAngleDegrees;
    var longerSide          = r.LongerSide;
    var shorterSide         = r.ShorterSide;
    var centreToLonger      = r.CentreToLongerSide;
    var centreToShorter     = r.CentreToShorterSide;
    var incircleRadius      = r.IncircleRadius;
    var circumcircleRadius  = r.CircumcircleRadius;
    var incircle            = r.Incircle;
    var circumcircle        = r.Circumcircle;
    var isSquare            = r.IsSquare;

    var fromArea        = Rectangle.FromAreaAndWidth(24, 6);
    var fromPerimeter   = Rectangle.FromPerimeterAndWidth(20, 6);
    var fromDiagonal    = Rectangle.FromDiagonalAndWidth(10, 6);

### Square

    using Numerinus.Geometry.Shapes;

    var s = new Square(5);

    var side                = s.Side;
    var area                = s.Area;
    var perimeter           = s.Perimeter;
    var diagonal            = s.Diagonal;
    var halfDiagonal        = s.HalfDiagonal;
    var diagAngle           = s.DiagonalAngleDegrees;
    var incircleRadius      = s.IncircleRadius;
    var circumcircleRadius  = s.CircumcircleRadius;
    var incircle            = s.Incircle;
    var circumcircle        = s.Circumcircle;

    var fromArea        = Square.FromArea(25);
    var fromPerimeter   = Square.FromPerimeter(20);
    var fromDiagonal    = Square.FromDiagonal(7.071);
    var fromCircumR     = Square.FromCircumradius(3.535);
    var fromInradius    = Square.FromInradius(2.5);

### Cube

    using Numerinus.Geometry.Shapes;

    var c = new Cube(4);

    var volume              = c.Volume;
    var surfaceArea         = c.SurfaceArea;
    var lateralSurfaceArea  = c.LateralSurfaceArea;
    var faceArea            = c.FaceArea;
    var faceDiagonal        = c.FaceDiagonal;
    var spaceDiagonal       = c.SpaceDiagonal;
    var totalEdgeLength     = c.TotalEdgeLength;
    var edgeCount           = c.EdgeCount;
    var faceCount           = c.FaceCount;
    var vertexCount         = c.VertexCount;
    var insphereRadius      = c.InsphereRadius;
    var midsphereRadius     = c.MidsphereRadius;
    var circumsphereRadius  = c.CircumsphereRadius;
    var insphere            = c.Insphere;
    var midsphere           = c.Midsphere;
    var circumsphere        = c.Circumsphere;
    var face                = c.Face;

    var fromVolume      = Cube.FromVolume(64);
    var fromSurface     = Cube.FromSurfaceArea(96);
    var fromSpaceDiag   = Cube.FromSpaceDiagonal(6.928);
    var fromFaceDiag    = Cube.FromFaceDiagonal(5.657);
    var fromInsphere    = Cube.FromInsphereRadius(2);
    var fromCircumsphere = Cube.FromCircumsphereRadius(3.464);

### Cylinder

    using Numerinus.Geometry.Shapes;

    var c = new Cylinder(3, 10);

    var volume              = c.Volume;
    var surfaceArea         = c.SurfaceArea;
    var lateralSurfaceArea  = c.LateralSurfaceArea;
    var baseArea            = c.BaseArea;
    var diameter            = c.Diameter;
    var baseCircumference   = c.BaseCircumference;
    var diagonal            = c.Diagonal;
    var slantHeight         = c.SlantHeight;
    var insphereRadius      = c.InsphereRadius;
    var circumsphereRadius  = c.CircumsphereRadius;
    var circumsphere        = c.Circumsphere;
    var baseCircle          = c.Base;

    var fromDiameter        = Cylinder.FromDiameter(6, 10);
    var fromVolAndHeight    = Cylinder.FromVolumeAndHeight(282.74, 10);
    var fromVolAndRadius    = Cylinder.FromVolumeAndRadius(282.74, 3);
    var fromLatAndHeight    = Cylinder.FromLateralSurfaceAreaAndHeight(188.5, 10);

### Triangle

    using Numerinus.Geometry.Shapes;

    var t = new Triangle(3, 4, 5);

    var area        = t.Area;
    var perimeter   = t.Perimeter;
    var angleA      = t.AngleADegrees;
    var angleB      = t.AngleBDegrees;
    var angleC      = t.AngleCDegrees;
    var heightA     = t.HeightA;
    var inradius    = t.IncircleRadius;
    var circumR     = t.CircumcircleRadius;
    var incircle    = t.Incircle;
    var circumcircle = t.Circumcircle;

    var isRight     = t.IsRightAngle;
    var isAcute     = t.IsAcute;
    var isObtuse    = t.IsObtuse;
    var isEquil     = t.IsEquilateral;

### Triangle — from vertices (centroid, centres available)

    using Numerinus.Geometry.Points;
    using Numerinus.Geometry.Shapes;

    var t = new Triangle(
        new Point2D(0, 0),
        new Point2D(4, 0),
        new Point2D(0, 3));

    var centroid            = t.Centroid;
    var incircleCentre      = t.IncircleCentre;
    var circumcircleCentre  = t.CircumcircleCentre;

### Triangle — factory constructors

    var sas   = Triangle.FromTwoSidesAndAngleDegrees(4, 5, 60);
    var aas   = Triangle.FromOneSideAndTwoAnglesDegrees(5, 60, 80);
    var eq    = Triangle.Equilateral(6);
    var iso   = Triangle.Isoceles(4, 7);
    var right = Triangle.RightAngle(3, 4);

### Transform2D

    using Numerinus.Geometry.Transforms;
    using Numerinus.Geometry.Points;

    var point = new Point2D(1, 0);

    var moved   = Transform2D.Translation(5, 3).Apply(point);
    var rotated = Transform2D.RotationDegrees(90).Apply(point);
    var scaled  = Transform2D.Scale(2).Apply(point);

    var combined = Transform2D.Translation(5, 0)
                 * Transform2D.RotationDegrees(45)
                 * Transform2D.Scale(2);

    var result = combined.Apply(point);

---

## API Reference

### Vector2 — Numerinus.Geometry.Vectors

Immutable readonly struct. Represents a 2D direction or displacement.

Properties
    X, Y               Scalar components
    Magnitude          Length: sqrt(x2 + y2)
    MagnitudeSquared   Length squared, no sqrt cost
    Zero               (0, 0)
    UnitX              (1, 0)
    UnitY              (0, 1)

Methods
    Normalise()                Returns unit vector in same direction
    Dot(a, b)                  Scalar dot product: Ax*Bx + Ay*By
    AngleBetween(a, b)         Angle in radians between two vectors
    Distance(a, b)             Euclidean distance between tip points

Operators
    v1 + v2, v1 - v2           Component-wise add / subtract
    v * scalar, scalar * v     Scale vector
    v / scalar                 Divide vector
    -v                         Negate

---

### Vector3 — Numerinus.Geometry.Vectors

Immutable readonly struct. Represents a 3D direction or displacement.

Properties
    X, Y, Z
    Magnitude, MagnitudeSquared
    Zero, UnitX, UnitY, UnitZ

Methods
    Normalise()
    Dot(a, b)          Ax*Bx + Ay*By + Az*Bz
    Cross(a, b)        Perpendicular vector (3D only)
    AngleBetween(a, b)
    Distance(a, b)

Operators
    v1 + v2, v1 - v2, v * s, v / s, -v

---

### Point2D — Numerinus.Geometry.Points

Immutable readonly struct. Represents a fixed position in 2D space.

Properties
    X, Y
    Origin             (0, 0)

Methods
    DistanceTo(other)
    Distance(a, b)
    DistanceSquared(a, b)
    MidpointTo(other)
    Midpoint(a, b)
    ToVector2()
    FromVector2(v)

Operators
    point + vector     Translate point
    point - vector     Translate in reverse
    point - point      Returns Vector2 direction

---

### Point3D — Numerinus.Geometry.Points

Immutable readonly struct. Represents a fixed position in 3D space.

Properties
    X, Y, Z
    Origin             (0, 0, 0)

Methods
    DistanceTo(other), Distance(a, b), DistanceSquared(a, b)
    MidpointTo(other), Midpoint(a, b)
    ToVector3(), FromVector3(v)

Operators
    point + vector, point - vector, point - point (returns Vector3)

---

### Circle — Numerinus.Geometry.Shapes

Immutable sealed class. Defined by radius. All arc methods accept radians or degrees.

Construction
    new Circle(radius)
    Circle.FromDiameter(d)              r = d / 2
    Circle.FromCircumference(c)         r = C / (2 * Pi)
    Circle.FromArea(a)                  r = sqrt(A / Pi)

Properties
    Radius, Diameter, Circumference, Area

Arc Methods (radians)
    ArcLength(theta)                    r * theta
    ArcArea(theta)                      0.5 * r2 * theta
    ArcPlusRadiusLength(theta)          Arc + 2r

Arc Methods (degrees)
    ArcLengthDegrees(theta)
    ArcAreaDegrees(theta)
    ArcPlusRadiusLengthDegrees(theta)

Reverse
    AngleFromArcLength(arcLen)          theta = arcLen / r
    AngleDegreesFromArcLength(arcLen)

---

### Sphere — Numerinus.Geometry.Shapes

Immutable sealed class. Defined by radius.

Construction
    new Sphere(radius)
    Sphere.FromDiameter(d)              r = d / 2
    Sphere.FromVolume(V)                r = cbrt(3V / 4Pi)
    Sphere.FromSurfaceArea(A)           r = sqrt(A / 4Pi)

Properties
    Radius, Diameter
    SurfaceArea                         4 * Pi * r2
    Volume                              (4/3) * Pi * r3
    GreatCircleCircumference            2 * Pi * r
    GreatCircleArea                     Pi * r2
    GreatCircle                         Returns Circle at equator

---

### Rectangle — Numerinus.Geometry.Shapes

Immutable class. Defined by width and height. All four angles are 90 degrees.
Square is a subclass of Rectangle.

Construction
    new Rectangle(width, height)
    Rectangle.FromAreaAndWidth(area, width)           h = area / width
    Rectangle.FromPerimeterAndWidth(perimeter, width) h = (perimeter / 2) - width
    Rectangle.FromDiagonalAndWidth(diagonal, width)   h = sqrt(diagonal2 - width2)

Properties
    Width, Height
    LongerSide, ShorterSide
    Perimeter                           2 * (width + height)
    Area                                width * height
    Diagonal                            sqrt(width2 + height2)
    HalfDiagonal                        diagonal / 2
    DiagonalAngleDegrees                atan(height / width) in degrees
    DiagonalAngleRadians                atan(height / width) in radians
    CentreToLongerSide                  shorter side / 2
    CentreToShorterSide                 longer side / 2
    IncircleRadius                      shorter side / 2
    CircumcircleRadius                  diagonal / 2
    Incircle                            Returns Circle touching both longer sides
    Circumcircle                        Returns Circle passing through all 4 corners
    IsSquare                            True if width equals height

---

### Square — Numerinus.Geometry.Shapes

Immutable sealed class. Extends Rectangle. All four sides are equal.
Inherits all Rectangle properties. Square-specific members override with exact formulas.

Construction
    new Square(side)
    Square.FromArea(area)               side = sqrt(area)
    Square.FromPerimeter(perimeter)     side = perimeter / 4
    Square.FromDiagonal(diagonal)       side = diagonal / sqrt(2)
    Square.FromCircumradius(R)          side = R * sqrt(2)
    Square.FromInradius(r)              side = 2 * r

Properties (all inherited from Rectangle plus)
    Side                                Same as Width
    Diagonal                            side * sqrt(2)
    HalfDiagonal                        side * sqrt(2) / 2
    DiagonalAngleDegrees                Always 45
    DiagonalAngleRadians                Always Pi / 4
    IncircleRadius                      side / 2
    CircumcircleRadius                  side * sqrt(2) / 2
    Incircle                            Returns Circle touching all 4 sides
    Circumcircle                        Returns Circle passing through all 4 corners

---

### Cube — Numerinus.Geometry.Shapes

Immutable sealed class. A regular hexahedron with 6 equal square faces, 12 edges, 8 vertices.

Construction
    new Cube(edge)
    Cube.FromVolume(V)                  edge = cbrt(V)
    Cube.FromSurfaceArea(A)             edge = sqrt(A / 6)
    Cube.FromSpaceDiagonal(d)           edge = d / sqrt(3)
    Cube.FromFaceDiagonal(d)            edge = d / sqrt(2)
    Cube.FromInsphereRadius(r)          edge = 2r
    Cube.FromCircumsphereRadius(R)      edge = 2R / sqrt(3)

Properties
    Edge
    Volume                              edge3
    SurfaceArea                         6 * edge2
    LateralSurfaceArea                  4 * edge2
    FaceArea                            edge2
    FaceDiagonal                        edge * sqrt(2)
    SpaceDiagonal                       edge * sqrt(3)
    TotalEdgeLength                     12 * edge
    EdgeCount                           12
    FaceCount                           6
    VertexCount                         8
    InsphereRadius                      edge / 2
    MidsphereRadius                     edge * sqrt(2) / 2
    CircumsphereRadius                  edge * sqrt(3) / 2
    Insphere                            Returns Sphere touching all 6 faces
    Midsphere                           Returns Sphere tangent to all 12 edges
    Circumsphere                        Returns Sphere through all 8 vertices
    Face                                Returns Square face
    SpaceToFaceDiagonalAngleDegrees     ~35.26 degrees
    SpaceToEdgeAngleDegrees             ~54.74 degrees

---

### Cylinder — Numerinus.Geometry.Shapes

Immutable sealed class. A right circular cylinder defined by radius and height.

Construction
    new Cylinder(radius, height)
    Cylinder.FromDiameter(d, height)                    r = d / 2
    Cylinder.FromVolumeAndHeight(V, height)             r = sqrt(V / (Pi * h))
    Cylinder.FromVolumeAndRadius(V, radius)             h = V / (Pi * r2)
    Cylinder.FromLateralSurfaceAreaAndHeight(A, height) r = A / (2 * Pi * h)

Properties
    Radius, Height
    Diameter                            2 * r
    BaseCircumference                   2 * Pi * r
    BaseArea                            Pi * r2
    LateralSurfaceArea                  2 * Pi * r * h
    SurfaceArea                         2 * Pi * r * (r + h)
    Volume                              Pi * r2 * h
    Diagonal                            sqrt(4r2 + h2)
    SlantHeight                         sqrt(r2 + h2)
    InsphereRadius                      min(r, h/2)
    CircumsphereRadius                  sqrt(r2 + (h/2)2)
    Circumsphere                        Returns Sphere through all base-circle points
    Base                                Returns Circle at the base
    DiagonalToAxisAngleDegrees          atan(2r / h) in degrees

---

### Triangle

Immutable class. Two construction modes: sides only, or vertex positions.

Construction — sides
    new Triangle(a, b, c)                                   Three side lengths (SSS)
    Triangle.FromTwoSidesAndAngle(a, b, angleRad)           SAS
    Triangle.FromTwoSidesAndAngleDegrees(a, b, angleDeg)
    Triangle.FromOneSideAndTwoAngles(a, angleA, angleB)     AAS
    Triangle.FromOneSideAndTwoAnglesDegrees(a, aA, aB)
    Triangle.Equilateral(side)
    Triangle.Isoceles(base, leg)
    Triangle.RightAngle(legA, legB)

Construction — vertices
    new Triangle(Point2D a, Point2D b, Point2D c)

Side-based properties (always available)
    SideA, SideB, SideC
    Perimeter, SemiPerimeter
    Area                            Heron's formula
    AngleADegrees/Radians, AngleBDegrees/Radians, AngleCDegrees/Radians
    HeightA, HeightB, HeightC
    Inradius, Circumradius
    IncircleRadius, CircumcircleRadius
    Incircle, Circumcircle          Returns Circle

Vertex-based properties (requires Point2D constructor)
    A, B, C                         Vertex positions
    HasVertices
    Centroid                        Intersection of medians
    IncircleCentre                  Weighted vertex average by opposite side
    CircumcircleCentre              Perpendicular bisector intersection

Classification
    IsEquilateral, IsIsoceles, IsScalene
    IsRightAngle, IsAcute, IsObtuse

---

### Transform2D — Numerinus.Geometry.Transforms

Immutable class. Wraps a 3x3 homogeneous Matrix(Scalar) for 2D affine transformations.
Uses homogeneous coordinates [x, y, 1] so translation, rotation and scaling are
all expressed as matrix multiplication.

Construction
    Transform2D.Identity
    Transform2D.Translation(tx, ty)
    Transform2D.Rotation(angleRadians)
    Transform2D.RotationDegrees(angleDegrees)
    Transform2D.Scale(factor)
    Transform2D.Scale(sx, sy)

Composition
    t1 * t2        Applies t2 first then t1 (standard matrix multiplication order)

Apply
    Apply(Point2D)     Translation + rotation + scale affect position
    Apply(Vector2)     Only rotation + scale applied, translation ignored

---

## Key Design Notes

All types are immutable. Every operation returns a new instance.

Vector vs Point distinction. Use Vector2 or Vector3 for directions and
displacements. Use Point2D or Point3D for fixed positions in space. Applying
Transform2D.Translation to a vector has no effect, which is mathematically correct.

Transform composition order. When combining transforms with *, the rightmost
is applied first. To scale then rotate then translate, write:
    Transform2D.Translation(...) * Transform2D.Rotation(...) * Transform2D.Scale(...)

Triangle vertex requirement. Centroid, IncircleCentre and CircumcircleCentre
require the triangle to be constructed from Point2D vertices. Accessing them
on a sides-only triangle throws InvalidOperationException. IncircleRadius and
CircumcircleRadius are always available since they only need side lengths.

Square extends Rectangle. Square inherits every Rectangle property. Members that
have exact closed-form formulas for a square (Diagonal, IncircleRadius,
DiagonalAngleDegrees, etc.) are overridden with the precise square formula.
Rectangle.IsSquare returns true when width equals height within floating-point
tolerance, so a Rectangle can also be checked without casting.

Cube insphere, midsphere and circumsphere. Three distinct spheres are defined for
a cube. The insphere touches all six faces (r = edge/2). The midsphere is tangent
to all twelve edges (rho = edge*sqrt(2)/2). The circumsphere passes through all
eight vertices (R = edge*sqrt(3)/2). All three are returned as Sphere instances.

---

## Related Modules

Numerinus.Core       IArithmetic(T), Scalar, ComplexNumber, Accuracy, constants
Numerinus.Algebra    Matrix(T), linear algebra
Numerinus.Statistics Statistical functions and distributions

---
## Support & Maintenance
Numerinus is an actively maintained suite of .NET libraries. To ensure that updates, bug fixes, and new performance optimizations continue to flow, consider supporting the project:

[Sponsor on GitHub](https://github.com/sponsors/linustech-git): Your support helps me dedicate more time to research, benchmarking, and shipping regular updates to the Numerinus ecosystem.

Feature Requests: Sponsors get priority visibility when suggesting new mathematical utilities or architectural improvements.

Keep it Alive: By sponsoring, you are investing in the long-term stability of these tools for the entire .NET community.

Stay Connected: For updates on the roadmap or to discuss the project, connect with me on [LinkedIn](https://www.linkedin.com/in/sunil-chaware-9035a646/).

---

## License

Copyright (c) 2026 Sunil Chaware. Licensed under the MIT License.
https://opensource.org/licenses/MIT

