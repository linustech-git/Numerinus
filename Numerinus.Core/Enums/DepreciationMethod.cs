namespace Numerinus.Core.Enums;

/// <summary>
/// Specifies the accounting method used to allocate the cost of an asset over its useful life.
/// </summary>
public enum DepreciationMethod
{
    /// <summary>Equal depreciation charge each period.</summary>
    StraightLine,

    /// <summary>A fixed percentage is applied to the remaining book value each period.</summary>
    DecliningBalance,

    /// <summary>Depreciation is proportional to the sum of the years' digits.</summary>
    SumOfYearsDigits
}
