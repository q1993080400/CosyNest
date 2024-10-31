using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个类型是<see cref="IPoint{Num}"/>的实现，
/// 可以用来表示一个二维坐标
/// </summary>
/// <inheritdoc cref="IPoint{Num}"/>
sealed class Point<Num> : IPoint<Num>
    where Num : INumber<Num>
{
    #region 水平位置
    public required Num Right { get; init; }
    #endregion
    #region 垂直位置
    public required Num Top { get; init; }
    #endregion
    #region 重写ToString
    public override string ToString()
        => $"Right:{Right}，Top:{Top}";
    #endregion
    #region IEquatable的实现
    public bool Equals(IPoint<Num>? other)
        => other is { } && Right == other.Right && Top == other.Top;
    #endregion
}
