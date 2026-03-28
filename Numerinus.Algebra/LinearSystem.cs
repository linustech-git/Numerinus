using System;
using System.Collections.Generic;
using System.Linq;

namespace Numerinus.Algebra;

/// <summary>
/// Represents a system of linear equations in the form Ax = b, where A is the coefficient matrix,
/// x is the variable vector, and b is the constant vector.
/// Solves systems using Gaussian elimination with partial pivoting.
/// </summary>
public class LinearSystem
{
    private readonly double[][] _augmentedMatrix; // [A|b]
    private readonly int _numEquations;
    private readonly int _numVariables;

    /// <summary>
    /// Gets the coefficient matrix A.
    /// </summary>
    public double[][] CoefficientMatrix
    {
        get
        {
            var result = new double[_numEquations][];
            for (int i = 0; i < _numEquations; i++)
            {
                result[i] = new double[_numVariables];
                Array.Copy(_augmentedMatrix[i], result[i], _numVariables);
            }
            return result;
        }
    }

    /// <summary>
    /// Gets the constants vector b.
    /// </summary>
    public double[] ConstantsVector
    {
        get
        {
            var result = new double[_numEquations];
            for (int i = 0; i < _numEquations; i++)
            {
                result[i] = _augmentedMatrix[i][_numVariables];
            }
            return result;
        }
    }

    /// <summary>
    /// Initializes a new linear system from a coefficient matrix A and constants vector b.
    /// </summary>
    /// <param name="coefficients">The coefficient matrix A (numEquations × numVariables).</param>
    /// <param name="constants">The constants vector b (numEquations × 1).</param>
    /// <exception cref="ArgumentNullException">Thrown if coefficients or constants is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the dimensions are invalid or inconsistent.</exception>
    public LinearSystem(double[][] coefficients, double[] constants)
    {
        if (coefficients == null)
            throw new ArgumentNullException(nameof(coefficients));
        if (constants == null)
            throw new ArgumentNullException(nameof(constants));
        if (coefficients.Length == 0)
            throw new ArgumentException("Coefficient matrix cannot be empty.", nameof(coefficients));
        if (coefficients.Length != constants.Length)
            throw new ArgumentException("Number of equations must match number of constants.", nameof(constants));

        _numEquations = coefficients.Length;
        _numVariables = coefficients[0].Length;

        // Validate all rows have same length
        for (int i = 0; i < _numEquations; i++)
        {
            if (coefficients[i] == null || coefficients[i].Length != _numVariables)
                throw new ArgumentException("All rows of coefficient matrix must have the same length.", nameof(coefficients));
        }

        // Create augmented matrix [A|b]
        _augmentedMatrix = new double[_numEquations][];
        for (int i = 0; i < _numEquations; i++)
        {
            _augmentedMatrix[i] = new double[_numVariables + 1];
            Array.Copy(coefficients[i], _augmentedMatrix[i], _numVariables);
            _augmentedMatrix[i][_numVariables] = constants[i];
        }
    }

    /// <summary>
    /// Initializes a new linear system from an augmented matrix [A|b].
    /// </summary>
    /// <param name="augmentedMatrix">The augmented matrix [A|b] (numEquations × (numVariables + 1)).</param>
    /// <exception cref="ArgumentNullException">Thrown if augmentedMatrix is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the matrix is invalid.</exception>
    public LinearSystem(double[][] augmentedMatrix)
    {
        if (augmentedMatrix == null)
            throw new ArgumentNullException(nameof(augmentedMatrix));
        if (augmentedMatrix.Length == 0)
            throw new ArgumentException("Augmented matrix cannot be empty.", nameof(augmentedMatrix));

        _numEquations = augmentedMatrix.Length;
        _numVariables = augmentedMatrix[0].Length - 1;

        if (_numVariables <= 0)
            throw new ArgumentException("Augmented matrix must have at least 2 columns (A and b).", nameof(augmentedMatrix));

        // Validate all rows have same length
        for (int i = 0; i < _numEquations; i++)
        {
            if (augmentedMatrix[i] == null || augmentedMatrix[i].Length != _numVariables + 1)
                throw new ArgumentException("All rows of augmented matrix must have the same length.", nameof(augmentedMatrix));
        }

        // Copy the augmented matrix
        _augmentedMatrix = new double[_numEquations][];
        for (int i = 0; i < _numEquations; i++)
        {
            _augmentedMatrix[i] = new double[_numVariables + 1];
            Array.Copy(augmentedMatrix[i], _augmentedMatrix[i], _numVariables + 1);
        }
    }

    /// <summary>
    /// Solves the linear system using Gaussian elimination with partial pivoting.
    /// </summary>
    /// <param name="tolerance">Tolerance for considering values as zero (default: 1e-10).</param>
    /// <returns>A LinearSystemSolution containing the solution information.</returns>
    public LinearSystemSolution Solve(double tolerance = 1e-10)
    {
        // Create a working copy of the augmented matrix
        var matrix = new double[_numEquations][];
        for (int i = 0; i < _numEquations; i++)
        {
            matrix[i] = new double[_numVariables + 1];
            Array.Copy(_augmentedMatrix[i], matrix[i], _numVariables + 1);
        }

        // Forward elimination with partial pivoting
        int rank = 0;
        for (int col = 0; col < _numVariables && rank < _numEquations; col++)
        {
            // Find pivot
            int pivotRow = rank;
            double maxVal = Math.Abs(matrix[rank][col]);

            for (int row = rank + 1; row < _numEquations; row++)
            {
                if (Math.Abs(matrix[row][col]) > maxVal)
                {
                    maxVal = Math.Abs(matrix[row][col]);
                    pivotRow = row;
                }
            }

            // Check if pivot is zero
            if (maxVal < tolerance)
                continue;

            // Swap rows
            var temp = matrix[rank];
            matrix[rank] = matrix[pivotRow];
            matrix[pivotRow] = temp;

            // Eliminate below
            for (int row = rank + 1; row < _numEquations; row++)
            {
                double factor = matrix[row][col] / matrix[rank][col];
                for (int j = col; j <= _numVariables; j++)
                {
                    matrix[row][j] -= factor * matrix[rank][j];
                }
            }

            rank++;
        }

        // Check for inconsistency
        for (int row = rank; row < _numEquations; row++)
        {
            bool allZero = true;
            for (int col = 0; col < _numVariables; col++)
            {
                if (Math.Abs(matrix[row][col]) > tolerance)
                {
                    allZero = false;
                    break;
                }
            }

            if (allZero && Math.Abs(matrix[row][_numVariables]) > tolerance)
            {
                // 0 = non-zero (inconsistent)
                return new LinearSystemSolution(_numVariables, SystemType.Inconsistent, null, rank);
            }
        }

        // Back substitution
        var solution = new double[_numVariables];
        var freeVariables = new List<int>();

        // Identify free variables
        var pivotCols = new bool[_numVariables];
        for (int row = 0; row < rank; row++)
        {
            for (int col = 0; col < _numVariables; col++)
            {
                if (Math.Abs(matrix[row][col]) > tolerance)
                {
                    pivotCols[col] = true;
                    break;
                }
            }
        }

        for (int col = 0; col < _numVariables; col++)
        {
            if (!pivotCols[col])
                freeVariables.Add(col);
        }

        // Back substitution for pivot variables
        for (int row = rank - 1; row >= 0; row--)
        {
            // Find the pivot column
            int pivotCol = -1;
            for (int col = 0; col < _numVariables; col++)
            {
                if (Math.Abs(matrix[row][col]) > tolerance)
                {
                    pivotCol = col;
                    break;
                }
            }

            if (pivotCol == -1)
                continue;

            double value = matrix[row][_numVariables];
            for (int col = pivotCol + 1; col < _numVariables; col++)
            {
                value -= matrix[row][col] * solution[col];
            }

            solution[pivotCol] = value / matrix[row][pivotCol];
        }

        // Determine system type
        SystemType type = freeVariables.Count == 0 ? SystemType.UniqueSolution : SystemType.InfiniteSolutions;

        return new LinearSystemSolution(_numVariables, type, solution, rank, freeVariables);
    }

    /// <summary>
    /// Returns a string representation of the augmented matrix.
    /// </summary>
    public override string ToString()
    {
        var lines = new List<string>();
        for (int i = 0; i < _numEquations; i++)
        {
            var row = string.Join("\t", _augmentedMatrix[i].Select(v => v.ToString("F4")));
            lines.Add(row);
        }
        return string.Join("\n", lines);
    }
}

/// <summary>
/// Represents the type of linear system.
/// </summary>
public enum SystemType
{
    /// <summary>System has exactly one solution.</summary>
    UniqueSolution,

    /// <summary>System has infinitely many solutions.</summary>
    InfiniteSolutions,

    /// <summary>System has no solution.</summary>
    Inconsistent
}

/// <summary>
/// Represents the solution to a linear system.
/// </summary>
public class LinearSystemSolution
{
    /// <summary>
    /// Gets the type of the system (unique solution, infinite solutions, or inconsistent).
    /// </summary>
    public SystemType Type { get; }

    /// <summary>
    /// Gets the solution vector. For infinite solutions, this is one particular solution.
    /// Returns null for inconsistent systems.
    /// </summary>
    public double[] Solution { get; }

    /// <summary>
    /// Gets the rank of the coefficient matrix.
    /// </summary>
    public int Rank { get; }

    /// <summary>
    /// Gets the indices of free variables (for infinite solutions).
    /// </summary>
    public IReadOnlyList<int> FreeVariables { get; }

    /// <summary>
    /// Gets the number of variables in the system.
    /// </summary>
    public int NumVariables { get; }

    internal LinearSystemSolution(int numVariables, SystemType type, double[] solution, int rank, 
        List<int> freeVariables = null)
    {
        NumVariables = numVariables;
        Type = type;
        Solution = solution;
        Rank = rank;
        FreeVariables = freeVariables?.AsReadOnly() ?? new List<int>().AsReadOnly();
    }

    /// <summary>
    /// Returns a string representation of the solution.
    /// </summary>
    public override string ToString()
    {
        return Type switch
        {
            SystemType.UniqueSolution => $"Unique Solution: [{string.Join(", ", Solution.Select(s => s.ToString("F6")))}]",
            SystemType.InfiniteSolutions => $"Infinite Solutions. Free variables: {string.Join(", ", FreeVariables.Select(i => $"x{i + 1}"))}. Particular solution: [{string.Join(", ", Solution.Select(s => s.ToString("F6")))}]",
            SystemType.Inconsistent => "No Solution (Inconsistent System)",
            _ => "Unknown"
        };
    }
}
