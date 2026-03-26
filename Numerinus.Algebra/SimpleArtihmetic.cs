using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numerinus.Algebra
{
    public class SimpleArtihmetic
    {
        /// <summary>
        /// Addition of 2 numbers. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public double Add(double a, double b) => a + b;

        /// <summary>
        /// Subtraction of 2 numbers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public double Subtract(double a, double b) => a - b;

        /// <summary>
        /// Calculates the product of two double-precision floating-point numbers.
        /// </summary>
        /// <param name="a">The first number to multiply.</param>
        /// <param name="b">The second number to multiply.</param>
        /// <returns>The product of the two specified numbers.</returns>
        public double Multiply(double a, double b) => a * b;

        /// <summary>
        /// Divides one double-precision floating-point number by another and returns the result.
        /// </summary>
        /// <param name="a">The dividend. This is the number to be divided.</param>
        /// <param name="b">The divisor. This is the number by which to divide. Must not be zero.</param>
        /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown when <paramref name="b"/> is zero.</exception>
        public double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }

        /// <summary>
        /// Calculates the remainder resulting from dividing one double-precision floating-point number by another.
        /// </summary>
        /// <param name="a">The dividend in the division operation.</param>
        /// <param name="b">The divisor in the division operation. Must not be zero.</param>
        /// <returns>The remainder after dividing a by b.</returns>
        /// <exception cref="DivideByZeroException">Thrown when b is zero.</exception>
        public double Modulo(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Cannot modulo by zero.");
            return a % b;
        }

        /// <summary>
        /// Calculates a specified number raised to the specified power.
        /// </summary>
        /// <param name="a">The base number to be raised to a power.</param>
        /// <param name="b">The exponent to which the base number is raised.</param>
        /// <returns>The result of raising a to the power of b.</returns>
        public double Power(double a, double b) => Math.Pow(a, b);

        /// <summary>
        /// Calculate square root of a number. If the number is negative, an exception will be thrown.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public double SquareRoot(double a)
        {
            if (a < 0) throw new ArgumentOutOfRangeException(nameof(a), "Cannot take square root of a negative number.");
            return Math.Sqrt(a);
        }
    }
}
