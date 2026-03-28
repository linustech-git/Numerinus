using System;
using System.Collections.Generic;

namespace Numerinus.Algebra.Polynomials;

/// <summary>
/// Provides root-finding algorithms for polynomials with double coefficients.
/// </summary>
public static class PolynomialRootFinder
{
    /// <summary>
    /// Finds a root of the polynomial using Newton-Raphson method.
    /// Requires an initial guess and iterates until convergence.
    /// </summary>
    /// <param name="polynomial">The polynomial to find a root of.</param>
    /// <param name="initialGuess">Initial approximation of the root.</param>
    /// <param name="maxIterations">Maximum number of iterations (default: 100).</param>
    /// <param name="tolerance">Convergence tolerance (default: 1e-10).</param>
    /// <returns>An approximation of a root, or null if convergence fails.</returns>
    /// <exception cref="ArgumentNullException">Thrown if polynomial is null.</exception>
    public static double? NewtonRaphson(Polynomial<double> polynomial, double initialGuess, 
        int maxIterations = 100, double tolerance = 1e-10)
    {
        if (polynomial == null)
            throw new ArgumentNullException(nameof(polynomial));

        var derivative = polynomial.Derivative();
        double x = initialGuess;

        for (int i = 0; i < maxIterations; i++)
        {
            double fx = polynomial.Evaluate(x);
            double fxPrime = derivative.Evaluate(x);

            // Avoid division by zero
            if (Math.Abs(fxPrime) < 1e-15)
                return null;

            double xNext = x - fx / fxPrime;
            
            if (Math.Abs(xNext - x) < tolerance)
                return xNext;

            x = xNext;
        }

        return null; // Did not converge
    }

    /// <summary>
    /// Finds a root of the polynomial using the bisection method.
    /// Requires an interval [a, b] where the polynomial has opposite signs at the endpoints.
    /// </summary>
    /// <param name="polynomial">The polynomial to find a root of.</param>
    /// <param name="a">Left endpoint of the interval.</param>
    /// <param name="b">Right endpoint of the interval.</param>
    /// <param name="maxIterations">Maximum number of iterations (default: 100).</param>
    /// <param name="tolerance">Convergence tolerance (default: 1e-10).</param>
    /// <returns>An approximation of a root, or null if the method fails.</returns>
    /// <exception cref="ArgumentNullException">Thrown if polynomial is null.</exception>
    /// <exception cref="ArgumentException">Thrown if f(a) and f(b) have the same sign.</exception>
    public static double? Bisection(Polynomial<double> polynomial, double a, double b, 
        int maxIterations = 100, double tolerance = 1e-10)
    {
        if (polynomial == null)
            throw new ArgumentNullException(nameof(polynomial));

        double fa = polynomial.Evaluate(a);
        double fb = polynomial.Evaluate(b);

        if (fa * fb > 0)
            throw new ArgumentException("f(a) and f(b) must have opposite signs.", nameof(polynomial));

        for (int i = 0; i < maxIterations; i++)
        {
            double c = (a + b) / 2;
            double fc = polynomial.Evaluate(c);

            if (Math.Abs(fc) < tolerance || Math.Abs(b - a) / 2 < tolerance)
                return c;

            if (fa * fc < 0)
            {
                b = c;
                fb = fc;
            }
            else
            {
                a = c;
                fa = fc;
            }
        }

        return (a + b) / 2;
    }

    /// <summary>
    /// Finds multiple roots of the polynomial by applying the bisection method 
    /// to several intervals determined by sampling the polynomial.
    /// </summary>
    /// <param name="polynomial">The polynomial to find roots of.</param>
    /// <param name="searchMin">Minimum value of the search interval.</param>
    /// <param name="searchMax">Maximum value of the search interval.</param>
    /// <param name="samplePoints">Number of points to sample for sign changes (default: 100).</param>
    /// <param name="tolerance">Convergence tolerance (default: 1e-10).</param>
    /// <returns>A list of approximate roots found.</returns>
    /// <exception cref="ArgumentNullException">Thrown if polynomial is null.</exception>
    public static List<double> FindRoots(Polynomial<double> polynomial, double searchMin, double searchMax,
        int samplePoints = 100, double tolerance = 1e-10)
    {
        if (polynomial == null)
            throw new ArgumentNullException(nameof(polynomial));

        var roots = new List<double>();
        double step = (searchMax - searchMin) / (samplePoints - 1);

        for (int i = 0; i < samplePoints - 1; i++)
        {
            double x1 = searchMin + i * step;
            double x2 = x1 + step;
            double f1 = polynomial.Evaluate(x1);
            double f2 = polynomial.Evaluate(x2);

            // Sign change detected
            if (f1 * f2 < 0)
            {
                var root = Bisection(polynomial, x1, x2, 100, tolerance);
                if (root.HasValue)
                {
                    // Avoid duplicates
                    if (roots.Count == 0 || Math.Abs(root.Value - roots[roots.Count - 1]) > tolerance * 10)
                        roots.Add(root.Value);
                }
            }
        }

        return roots;
    }
}
