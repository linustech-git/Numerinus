namespace Numerinus.Core.Interfaces;

/// <summary>
/// Defines the standard operations for any mathematical type in the Numerinus suite.
/// </summary>
public interface IArithmetic<T> where T : IArithmetic<T>
{
    static abstract T Add(T left, T right);
    static abstract T Subtract(T left, T right);
    static abstract T Multiply(T left, T right);
    static abstract T Divide(T left, T right);

    static abstract T Zero { get; }
    static abstract T One { get; }

    // Helper to check if a value is effectively zero based on precision
    bool IsZero(double epsilon = 1e-15);
}
