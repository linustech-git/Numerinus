using Numerinus.Core.Interfaces;

namespace Numerinus.Algebra.Matrices;

public class Matrix<T> where T : IArithmetic<T>
{
    private readonly T[,] _data;
    public int Rows { get; }
    public int Columns { get; }

    public Matrix(int rows, int cols)
    {
        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("Dimensions must be greater than zero.");

        Rows = rows;
        Columns = cols;
        _data = new T[rows, cols];
    }

    // Indexer to allow: myMatrix[0, 0] = value;
    public T this[int r, int c]
    {
        get => _data[r, c];
        set => _data[r, c] = value;
    }

    // 1. ADDITION OPERATOR (+)
    public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
    {
        ValidateDimensions(left, right);
        var result = new Matrix<T>(left.Rows, left.Columns);

        for (int i = 0; i < left.Rows; i++)
            for (int j = 0; j < left.Columns; j++)
                result[i, j] = T.Add(left[i, j], right[i, j]);

        return result;
    }

    // 2. SUBTRACTION OPERATOR (-)
    public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
    {
        ValidateDimensions(left, right);
        var result = new Matrix<T>(left.Rows, left.Columns);

        for (int i = 0; i < left.Rows; i++)
            for (int j = 0; j < left.Columns; j++)
                result[i, j] = T.Subtract(left[i, j], right[i, j]);

        return result;
    }

    // 3. MULTIPLICATION OPERATOR (*)
    public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
    {
        if (left.Columns != right.Rows)
            throw new ArgumentException("Inner dimensions must match for multiplication.");

        var result = new Matrix<T>(left.Rows, right.Columns);

        for (int i = 0; i < left.Rows; i++)
        {
            for (int j = 0; j < right.Columns; j++)
            {
                T sum = T.Zero;
                for (int k = 0; k < left.Columns; k++)
                {
                    T product = T.Multiply(left[i, k], right[k, j]);
                    sum = T.Add(sum, product);
                }
                result[i, j] = sum;
            }
        }
        return result;
    }

    // Helper to check math rules
    private static void ValidateDimensions(Matrix<T> a, Matrix<T> b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
            throw new ArgumentException("Matrix dimensions must be identical for this operation.");
    }

    public override string ToString()
    {
        return $"Matrix ({Rows}x{Columns})";
    }
}
