namespace Numerinus.Algebra
{
    public class SimpleArithmetic
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

        // -------------------------
        // Number Theory Operations
        // -------------------------

        /// <summary>
        /// Calculates the factorial of a non-negative integer n (n!).
        /// </summary>
        /// <param name="n">A non-negative integer whose factorial is to be computed.</param>
        /// <returns>The factorial of <paramref name="n"/> as a double to support large values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="n"/> is negative.</exception>
        public double Factorial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "Factorial is not defined for negative numbers.");
            if (n == 0 || n == 1) return 1;
            double result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        /// <summary>
        /// Computes the Greatest Common Divisor (GCD) of two integers using the Euclidean algorithm.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <returns>The GCD of <paramref name="a"/> and <paramref name="b"/>. Always returns a non-negative value.</returns>
        public long GreatestCommonDivisor(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Computes the Least Common Multiple (LCM) of two integers.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <returns>The LCM of <paramref name="a"/> and <paramref name="b"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when both <paramref name="a"/> and <paramref name="b"/> are zero.</exception>
        public long LeastCommonMultiple(long a, long b)
        {
            if (a == 0 && b == 0) throw new ArgumentException("LCM is undefined when both values are zero.");
            return Math.Abs(a / GreatestCommonDivisor(a, b) * b);
        }

        /// <summary>
        /// Determines whether a given integer is a prime number.
        /// A prime number is greater than 1 and divisible only by 1 and itself.
        /// </summary>
        /// <param name="n">The integer to test for primality.</param>
        /// <returns><c>true</c> if <paramref name="n"/> is prime; otherwise, <c>false</c>.</returns>
        public bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            for (long i = 3; i <= Math.Sqrt(n); i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the absolute value of a double-precision floating-point number.
        /// </summary>
        /// <param name="a">The number whose absolute value is required.</param>
        /// <returns>The absolute value of <paramref name="a"/>.</returns>
        public double AbsoluteValue(double a) => Math.Abs(a);
    }
}