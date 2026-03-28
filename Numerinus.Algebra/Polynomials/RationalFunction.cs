using System;
using System.Collections.Generic;

namespace Numerinus.Algebra.Polynomials;

/// <summary>
/// Represents a rational function as the ratio of two polynomials: P(x) / Q(x)
/// where P(x) is the numerator and Q(x) is the denominator.
/// Works with any numeric type that supports arithmetic operators (+, -, *, /).
/// </summary>
/// <typeparam name="T">The coefficient type. Supports any type with arithmetic operators (+, -, *, /).</typeparam>
public class RationalFunction<T>
{
    /// <summary>
    /// Gets the numerator polynomial P(x).
    /// </summary>
    public Polynomial<T> Numerator { get; private set; }

    /// <summary>
    /// Gets the denominator polynomial Q(x).
    /// </summary>
    public Polynomial<T> Denominator { get; private set; }

    /// <summary>
    /// Initializes a new rational function with given numerator and denominator polynomials.
    /// </summary>
    /// <param name="numerator">The numerator polynomial P(x).</param>
    /// <param name="denominator">The denominator polynomial Q(x). Must not be the zero polynomial.</param>
    /// <exception cref="ArgumentNullException">Thrown if numerator or denominator is null.</exception>
    /// <exception cref="ArgumentException">Thrown if denominator is the zero polynomial.</exception>
    public RationalFunction(Polynomial<T> numerator, Polynomial<T> denominator)
    {
        if (numerator == null)
            throw new ArgumentNullException(nameof(numerator));
        if (denominator == null)
            throw new ArgumentNullException(nameof(denominator));
        if (denominator.Degree == -1)
            throw new ArgumentException("Denominator cannot be the zero polynomial.", nameof(denominator));

        Numerator = numerator;
        Denominator = denominator;
    }

    /// <summary>
    /// Evaluates the rational function at a given value of x.
    /// Returns the result of P(x) / Q(x).
    /// </summary>
    /// <param name="x">The value at which to evaluate the rational function.</param>
    /// <returns>The result of evaluating P(x) / Q(x) at x.</returns>
    /// <exception cref="DivideByZeroException">Thrown if the denominator evaluates to zero at x (a pole).</exception>
    public T Evaluate(T x)
    {
        dynamic numResult = Numerator.Evaluate(x);
        dynamic denResult = Denominator.Evaluate(x);

        if (denResult == 0)
            throw new DivideByZeroException($"The rational function has a pole at x = {x}. The denominator is zero.");

        return (T)(numResult / denResult);
    }

    /// <summary>
    /// Evaluates the rational function at a given value of x, returning null if a pole is encountered.
    /// </summary>
    /// <param name="x">The value at which to evaluate the rational function.</param>
    /// <param name="tolerance">Tolerance for considering denominator as zero (default: 1e-10).</param>
    /// <returns>The result of P(x) / Q(x), or null if a pole is encountered.</returns>
    public T? TryEvaluate(T x, double tolerance = 1e-10)
    {
        try
        {
            dynamic numResult = Numerator.Evaluate(x);
            dynamic denResult = Denominator.Evaluate(x);

            // Check if denominator is effectively zero
            if (Math.Abs((double)denResult) < tolerance)
                return default(T);

            return (T)(numResult / denResult);
        }
        catch
        {
            return default(T);
        }
    }

    /// <summary>
    /// Adds two rational functions: (P₁/Q₁) + (P₂/Q₂) = (P₁Q₂ + P₂Q₁) / (Q₁Q₂)
    /// </summary>
    /// <param name="other">The rational function to add.</param>
    /// <returns>A new rational function representing the sum.</returns>
    /// <exception cref="ArgumentNullException">Thrown if other is null.</exception>
    public RationalFunction<T> Add(RationalFunction<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        // (P1/Q1) + (P2/Q2) = (P1*Q2 + P2*Q1) / (Q1*Q2)
        var numerator = Numerator.Multiply(other.Denominator)
                                  .Add(other.Numerator.Multiply(Denominator));
        var denominator = Denominator.Multiply(other.Denominator);

        return new RationalFunction<T>(numerator, denominator);
    }

    /// <summary>
    /// Subtracts another rational function: (P₁/Q₁) - (P₂/Q₂) = (P₁Q₂ - P₂Q₁) / (Q₁Q₂)
    /// </summary>
    /// <param name="other">The rational function to subtract.</param>
    /// <returns>A new rational function representing the difference.</returns>
    /// <exception cref="ArgumentNullException">Thrown if other is null.</exception>
    public RationalFunction<T> Subtract(RationalFunction<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        // (P1/Q1) - (P2/Q2) = (P1*Q2 - P2*Q1) / (Q1*Q2)
        var numerator = Numerator.Multiply(other.Denominator)
                                  .Subtract(other.Numerator.Multiply(Denominator));
        var denominator = Denominator.Multiply(other.Denominator);

        return new RationalFunction<T>(numerator, denominator);
    }

    /// <summary>
    /// Multiplies two rational functions: (P₁/Q₁) × (P₂/Q₂) = (P₁P₂) / (Q₁Q₂)
    /// </summary>
    /// <param name="other">The rational function to multiply.</param>
    /// <returns>A new rational function representing the product.</returns>
    /// <exception cref="ArgumentNullException">Thrown if other is null.</exception>
    public RationalFunction<T> Multiply(RationalFunction<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        // (P1/Q1) * (P2/Q2) = (P1*P2) / (Q1*Q2)
        var numerator = Numerator.Multiply(other.Numerator);
        var denominator = Denominator.Multiply(other.Denominator);

        return new RationalFunction<T>(numerator, denominator);
    }

    /// <summary>
    /// Divides by another rational function: (P₁/Q₁) ÷ (P₂/Q₂) = (P₁Q₂) / (Q₁P₂)
    /// </summary>
    /// <param name="other">The rational function to divide by.</param>
    /// <returns>A new rational function representing the quotient.</returns>
    /// <exception cref="ArgumentNullException">Thrown if other is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the numerator of other is the zero polynomial.</exception>
    public RationalFunction<T> Divide(RationalFunction<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        if (other.Numerator.Degree == -1)
            throw new ArgumentException("Cannot divide by a rational function with zero numerator.", nameof(other));

        // (P1/Q1) / (P2/Q2) = (P1*Q2) / (Q1*P2)
        var numerator = Numerator.Multiply(other.Denominator);
        var denominator = Denominator.Multiply(other.Numerator);

        return new RationalFunction<T>(numerator, denominator);
    }

    /// <summary>
    /// Computes the derivative of the rational function using the quotient rule:
    /// d/dx[P(x)/Q(x)] = [P'(x)Q(x) - P(x)Q'(x)] / [Q(x)]²
    /// </summary>
    /// <returns>A new rational function representing the derivative.</returns>
    public RationalFunction<T> Derivative()
    {
        var pPrime = Numerator.Derivative();
        var qPrime = Denominator.Derivative();

        // [P'Q - PQ'] / Q²
        var numerator = pPrime.Multiply(Denominator).Subtract(Numerator.Multiply(qPrime));
        var denominator = Denominator.Multiply(Denominator);

        return new RationalFunction<T>(numerator, denominator);
    }

    /// <summary>
    /// Gets the poles (zeros) of the denominator polynomial.
    /// Poles are x-values where the rational function is undefined.
    /// </summary>
    /// <remarks>
    /// This method only works with Polynomial&lt;double&gt;.
    /// For other types, the denominator can be inspected directly via the Denominator property.
    /// </remarks>
    /// <param name="searchMin">Minimum value of search interval (for double type only).</param>
    /// <param name="searchMax">Maximum value of search interval (for double type only).</param>
    /// <returns>A list of approximate poles, or empty list if T is not double.</returns>
    public List<double> GetPoles(double searchMin = -100, double searchMax = 100)
    {
        if (typeof(T) == typeof(double) && Denominator is Polynomial<double> polyDenom)
        {
            return PolynomialRootFinder.FindRoots(polyDenom, searchMin, searchMax);
        }

        return new List<double>();
    }

    /// <summary>
    /// Gets the zeros (roots) of the numerator polynomial.
    /// Zeros are x-values where the rational function equals zero.
    /// </summary>
    /// <remarks>
    /// This method only works with Polynomial&lt;double&gt;.
    /// For other types, the numerator can be inspected directly via the Numerator property.
    /// </remarks>
    /// <param name="searchMin">Minimum value of search interval (for double type only).</param>
    /// <param name="searchMax">Maximum value of search interval (for double type only).</param>
    /// <returns>A list of approximate zeros, or empty list if T is not double.</returns>
    public List<double> GetZeros(double searchMin = -100, double searchMax = 100)
    {
        if (typeof(T) == typeof(double) && Numerator is Polynomial<double> polyNum)
        {
            return PolynomialRootFinder.FindRoots(polyNum, searchMin, searchMax);
        }

        return new List<double>();
    }

    /// <summary>
    /// Returns a string representation of the rational function.
    /// Example: "[1 + 2x] / [3 + x²]"
    /// </summary>
    public override string ToString() => $"[{Numerator}] / [{Denominator}]";
}
