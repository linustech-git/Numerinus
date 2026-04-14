namespace Numerinus.Core.Enums;

/// <summary>
/// Specifies how electrical components are connected in a circuit.
/// </summary>
public enum CircuitType
{
    /// <summary>Components are connected end-to-end; the same current flows through each.</summary>
    Series,

    /// <summary>Components share the same two nodes; the same voltage is across each.</summary>
    Parallel
}
