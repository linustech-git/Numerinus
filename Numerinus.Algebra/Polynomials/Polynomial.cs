using System;
using System.Collections.Generic;
using System.Linq;

namespace Numerinus.Algebra.Polynomials;    

/// <summary>
/// Represents a polynomial with coefficients of type T.
/// Coefficients are stored in ascending order of degree: [a₀, a₁, a₂, ...] represents a₀ + a₁x + a₂x² + ...
/// Works with any numeric type: int, double, decimal, Scalar, ComplexNumber, etc.
/// </summary>
/// <typeparam name="T">The coefficient type. Supports any type with arithmetic operators (+, -, *, /).</typeparam>
public class Polynomial<T>
{
    private List<T> _coefficients;

    /// <summary>
    /// Gets the coefficients of the polynomial in ascending order of degree.
    /// </summary>
    public IReadOnlyList<T> Coefficients => _coefficients.AsReadOnly();

    /// <summary>
    /// Gets the degree of the polynomial (highest power with non-zero coefficient).
    /// Returns -1 for the zero polynomial.
    /// </summary>
    public int Degree
    {
        get
        {
            for (int i = _coefficients.Count - 1; i >= 0; i--)
            {
                if (!IsZero(_coefficients[i]))
                    return i;
            }
            return -1; // Zero polynomial
        }
    }

    /// <summary>
    /// Checks if a coefficient is effectively zero.
    /// </summary>
    private static bool IsZero(T value)
    {
        try
        {
            dynamic d = value;
            return d == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the zero value for type T.
    /// </summary>
    private static T Zero
    {
        get
        {
            try
            {
                dynamic zero = 0;
                return (T)zero;
            }
            catch
            {
                // Fallback for types without implicit conversion from 0
                return default(T);
            }
        }
    }

    /// <summary>
    /// Initializes a new polynomial with the given coefficients in ascending order of degree.
    /// </summary>
    /// <param name="coefficients">The coefficients [a₀, a₁, a₂, ...] for a₀ + a₁x + a₂x² + ...</param>
    /// <exception cref="ArgumentException">Thrown if coefficients array is empty.</exception>
    public Polynomial(params T[] coefficients)
    {
        if (coefficients == null || coefficients.Length == 0)
            throw new ArgumentException("Polynomial must have at least one coefficient.", nameof(coefficients));

        _coefficients = new List<T>(coefficients);
    }

    /// <summary>
    /// Initializes a new polynomial from a list of coefficients in ascending order of degree.
    /// </summary>
    /// <param name="coefficients">The coefficients [a₀, a₁, a₂, ...] for a₀ + a₁x + a₂x² + ...</param>
    /// <exception cref="ArgumentException">Thrown if coefficients list is empty.</exception>
    public Polynomial(IEnumerable<T> coefficients)
    {
        var list = coefficients?.ToList() ?? new List<T>();
        if (list.Count == 0)
            throw new ArgumentException("Polynomial must have at least one coefficient.", nameof(coefficients));

        _coefficients = list;
    }

    /// <summary>
    /// Evaluates the polynomial at a given value of x.
    /// Uses Horner's method for numerical stability: p(x) = a₀ + x(a₁ + x(a₂ + ...))
    /// </summary>
    /// <param name="x">The value at which to evaluate the polynomial.</param>
    /// <returns>The result of evaluating the polynomial at x.</returns>
    public T Evaluate(T x)
    {
        if (_coefficients.Count == 0)
            return Zero;

        // Horner's method: start from highest degree and work backwards
        dynamic result = _coefficients[_coefficients.Count - 1];
        for (int i = _coefficients.Count - 2; i >= 0; i--)
        {
            result = result * x + _coefficients[i];
        }
        return (T)result;
    }

    /// <summary>
    /// Adds two polynomials.
    /// </summary>
    /// <param name="other">The polynomial to add.</param>
    /// <returns>A new polynomial representing the sum.</returns>
    /// <exception cref="ArgumentNullException">Thrown if other is null.</exception>
    public Polynomial<T> Add(Polynomial<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        int maxDegree = Math.Max(_coefficients.Count, other._coefficients.Count);
        var result = new List<T>(maxDegree);

        for (int i = 0; i < maxDegree; i++)
        {
            dynamic coeff1 = i < _coefficients.Count ? _coefficients[i] : Zero;
            dynamic coeff2 = i < other._coefficients.Count ? other._coefficients[i] : Zero;
            result.Add((T)(coeff1 + coeff2));
        }

        return new Polynomial<T>(result);
    }

    /// <summary>
    /// Subtracts another polynomial from this one.
    /// </summary>
    /// <param name="other">The polynomial to subtract.</param>
    /// <returns>A new polynomial representing the difference.</returns>
    /// <exception cref="ArgumentNullException">Thrown if other is null.</exception>
    public Polynomial<T> Subtract(Polynomial<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        int maxDegree = Math.Max(_coefficients.Count, other._coefficients.Count);
        var result = new List<T>(maxDegree);

        for (int i = 0; i < maxDegree; i++)
        {
            dynamic coeff1 = i < _coefficients.Count ? _coefficients[i] : Zero;
            dynamic coeff2 = i < other._coefficients.Count ? other._coefficients[i] : Zero;
            result.Add((T)(coeff1 - coeff2));
        }

        return new Polynomial<T>(result);
    }

    /// <summary>
    /// Multiplies two polynomials using the convolution method.
    /// </summary>
    /// <param name="other">The polynomial to multiply.</param>
    /// <returns>A new polynomial representing the product.</returns>
    /// <exception cref="ArgumentNullException">Thrown if other is null.</exception>
    public Polynomial<T> Multiply(Polynomial<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        int resultDegree = _coefficients.Count + other._coefficients.Count - 1;
        var result = new List<T>(resultDegree);

        for (int i = 0; i < resultDegree; i++)
        {
            result.Add(Zero);
        }

        for (int i = 0; i < _coefficients.Count; i++)
        {
            for (int j = 0; j < other._coefficients.Count; j++)
            {
                dynamic product = (dynamic)_coefficients[i] * (dynamic)other._coefficients[j];
                dynamic sum = (dynamic)result[i + j] + product;
                result[i + j] = (T)sum;
            }
        }

        return new Polynomial<T>(result);
    }

    /// <summary>
    /// Computes the derivative of the polynomial.
    /// The derivative of a₀ + a₁x + a₂x² + ... is a₁ + 2a₂x + 3a₃x² + ...
    /// </summary>
    /// <returns>A new polynomial representing the derivative.</returns>
    public Polynomial<T> Derivative()
    {
        if (_coefficients.Count <= 1)
            return new Polynomial<T>(Zero);

        var result = new List<T>(_coefficients.Count - 1);

        for (int i = 1; i < _coefficients.Count; i++)
        {
            // Multiply coefficient by its degree
            dynamic scaled = (dynamic)_coefficients[i] * i;
            result.Add((T)scaled);
        }

        return new Polynomial<T>(result);
    }

    /// <summary>
    /// Computes the integral of the polynomial (indefinite integral, constant of integration is zero).
    /// The integral of a₀ + a₁x + a₂x² + ... is a₀x + (a₁/2)x² + (a₂/3)x³ + ...
    /// </summary>
    /// <returns>A new polynomial representing the integral.</returns>
    public Polynomial<T> Integral()
    {
        var result = new List<T>(_coefficients.Count + 1)
        {
            Zero // Constant of integration is zero
        };

        for (int i = 0; i < _coefficients.Count; i++)
        {
            // Divide coefficient by (i + 1)
            dynamic divided = (dynamic)_coefficients[i] / (i + 1);
            result.Add((T)divided);
        }

        return new Polynomial<T>(result);
    }

    /// <summary>
    /// Returns a string representation of the polynomial.
    /// Example: "1 + 2x + 3x² + 4x³"
    /// </summary>
    public override string ToString()
    {
        if (_coefficients.Count == 0 || Degree == -1)
            return "0";

        var terms = new List<string>();
        for (int i = 0; i < _coefficients.Count; i++)
        {
            if (IsZero(_coefficients[i]))
                continue;

            string term = _coefficients[i].ToString();

            if (i == 0)
                terms.Add(term);
            else if (i == 1)
                terms.Add($"{term}x");
            else
                terms.Add($"{term}x{GetSuperscript(i)}");
        }

        return string.Join(" + ", terms);
    }

    private static string GetSuperscript(int number) =>
        number switch
        {
            2 => "²",
            3 => "³",
            4 => "⁴",
            5 => "⁵",
            _ => $"^{number}"
        };
}
