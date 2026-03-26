// Copyright (c) 2026 Sunil Chaware. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Numerinus.Algebra.Matrices;
using Numerinus.Core.Constants;
using Numerinus.Core.Numerics;
using Numerinus.Geometry.Points;
using Numerinus.Geometry.Vectors;

namespace Numerinus.Geometry.Transforms;

/// <summary>
/// Represents an immutable 2D affine transformation using a 3x3 homogeneous Matrix&lt;Scalar&gt;.
/// Supports translation, rotation, scaling and composition via matrix multiplication.
/// </summary>
/// <remarks>
/// Homogeneous coordinates extend 2D points to 3-element column vectors [x, y, 1],
/// allowing translation to be expressed as a matrix multiplication like rotation and scaling.
///
///  [ m00  m01  m02 ]   [ x ]   [ x' ]
///  [ m10  m11  m12 ] × [ y ] = [ y' ]
///  [  0    0    1  ]   [ 1 ]   [  1 ]
/// </remarks>
public sealed class Transform2D
{
    private readonly Matrix<Scalar> _matrix;

    private Transform2D(Matrix<Scalar> matrix) => _matrix = matrix;

    // --- Identity ---

    /// <summary>The identity transform — applies no change to any point or vector.</summary>
    public static Transform2D Identity
    {
        get
        {
            var m = new Matrix<Scalar>(3, 3);
            m[0, 0] = 1; m[0, 1] = 0; m[0, 2] = 0;
            m[1, 0] = 0; m[1, 1] = 1; m[1, 2] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1;
            return new(m);
        }
    }

    // --- Factory Methods ---

    /// <summary>
    /// Creates a translation transform that moves points by (tx, ty).
    /// [ 1  0  tx ]
    /// [ 0  1  ty ]
    /// [ 0  0   1 ]
    /// </summary>
    public static Transform2D Translation(Scalar tx, Scalar ty)
    {
        var m = new Matrix<Scalar>(3, 3);
        m[0, 0] = 1; m[0, 1] = 0; m[0, 2] = tx;
        m[1, 0] = 0; m[1, 1] = 1; m[1, 2] = ty;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1;
        return new(m);
    }

    /// <summary>
    /// Creates a rotation transform for the given angle in radians (counter-clockwise).
    /// [ cos θ  -sin θ  0 ]
    /// [ sin θ   cos θ  0 ]
    /// [  0       0     1 ]
    /// </summary>
    public static Transform2D Rotation(Scalar angleRadians)
    {
        double cos = Math.Cos(angleRadians.Value);
        double sin = Math.Sin(angleRadians.Value);

        var m = new Matrix<Scalar>(3, 3);
        m[0, 0] = cos; m[0, 1] = -sin; m[0, 2] = 0;
        m[1, 0] = sin; m[1, 1] = cos; m[1, 2] = 0;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1;
        return new(m);
    }

    /// <summary>
    /// Creates a rotation transform for the given angle in degrees (counter-clockwise).
    /// </summary>
    public static Transform2D RotationDegrees(Scalar angleDegrees)
        => Rotation(new(angleDegrees.Value * NumerinusConstants.Pi / 180.0));

    /// <summary>
    /// Creates a uniform scale transform that scales X and Y by the same factor.
    /// [ s  0  0 ]
    /// [ 0  s  0 ]
    /// [ 0  0  1 ]
    /// </summary>
    public static Transform2D Scale(Scalar factor)
        => Scale(factor, factor);

    /// <summary>
    /// Creates a non-uniform scale transform that scales X and Y independently.
    /// [ sx  0  0 ]
    /// [  0 sy  0 ]
    /// [  0  0  1 ]
    /// </summary>
    public static Transform2D Scale(Scalar sx, Scalar sy)
    {
        var m = new Matrix<Scalar>(3, 3);
        m[0, 0] = sx; m[0, 1] = 0; m[0, 2] = 0;
        m[1, 0] = 0; m[1, 1] = sy; m[1, 2] = 0;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1;
        return new(m);
    }

    // --- Composition ---

    /// <summary>
    /// Combines two transforms — applies <paramref name="right"/> first, then <paramref name="left"/>.
    /// Equivalent to matrix multiplication: left × right.
    /// </summary>
    public static Transform2D operator *(Transform2D left, Transform2D right)
        => new(left._matrix * right._matrix);

    // --- Apply ---

    /// <summary>
    /// Applies this transform to a 2D point.
    /// Translates, rotates and scales the position.
    /// </summary>
    public Point2D Apply(Point2D point)
    {
        Scalar x = _matrix[0, 0] * point.X + _matrix[0, 1] * point.Y + _matrix[0, 2];
        Scalar y = _matrix[1, 0] * point.X + _matrix[1, 1] * point.Y + _matrix[1, 2];
        return new(x, y);
    }

    /// <summary>
    /// Applies this transform to a 2D vector.
    /// Rotates and scales the direction — translation has NO effect on vectors.
    /// </summary>
    public Vector2 Apply(Vector2 vector)
    {
        Scalar x = _matrix[0, 0] * vector.X + _matrix[0, 1] * vector.Y;
        Scalar y = _matrix[1, 0] * vector.X + _matrix[1, 1] * vector.Y;
        return new(x, y);
    }

    public override string ToString() => _matrix.ToString();
}