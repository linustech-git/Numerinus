using Numerinus.Core.Interfaces;

namespace Numerinus.Core.Numerics;

public class ComplexNumber : IArithmetic<ComplexNumber>
{
    public double Real { get; }
    public double Imaginary { get; }

    public ComplexNumber(double real, double imaginary) => (Real, Imaginary) = (real, imaginary);

    // Implementation of IArithmetic Rules
    public static ComplexNumber Add(ComplexNumber l, ComplexNumber r) => new(l.Real + r.Real, l.Imaginary + r.Imaginary);
    public static ComplexNumber Subtract(ComplexNumber l, ComplexNumber r) => new(l.Real - r.Real, l.Imaginary - r.Imaginary);
    public static ComplexNumber Multiply(ComplexNumber l, ComplexNumber r) =>
        new(l.Real * r.Real - l.Imaginary * r.Imaginary, l.Real * r.Imaginary + l.Imaginary * r.Real);

    public static ComplexNumber Divide(ComplexNumber l, ComplexNumber r)
    {
        double div = r.Real * r.Real + r.Imaginary * r.Imaginary;
        if (Math.Abs(div) < 1e-15) throw new DivideByZeroException("Cannot divide by a complex zero.");
        return new((l.Real * r.Real + l.Imaginary * r.Imaginary) / div, (l.Imaginary * r.Real - l.Real * r.Imaginary) / div);
    }

    public static ComplexNumber Zero => new(0, 0);
    public static ComplexNumber One => new(1, 0);

    public bool IsZero(double epsilon = 1e-15) => Math.Abs(Real) < epsilon && Math.Abs(Imaginary) < epsilon;

    public override string ToString() => $"{Real} + {Imaginary}i";
}
