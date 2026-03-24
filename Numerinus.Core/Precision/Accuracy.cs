namespace Numerinus.Core.Precision;

public static class Accuracy
{
    public const double StandardEpsilon = 1e-15;
    public const double HighPrecisionEpsilon = 1e-18;

    public static bool AreEqual(double a, double b, double epsilon = StandardEpsilon)
        => Math.Abs(a - b) < epsilon;
}
