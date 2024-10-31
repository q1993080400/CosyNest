using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个类型是<see cref="ISize{Num}"/>的实现，
/// 可以视为一个二维平面的大小
/// </summary>
/// <inheritdoc cref="ISize{Num}"/>
sealed class Size<Num> : ISize<Num>
    where Num : INumber<Num>
{
    #region 宽度
    public required Num Width { get; init; }
    #endregion
    #region 高度
    public required Num Height { get; init; }
    #endregion
    #region IEquatable的实现
    public bool Equals(ISize<Num>? other)
        => other is { } &&
        other.Width == Width &&
        other.Height == Height;
    #endregion
}
