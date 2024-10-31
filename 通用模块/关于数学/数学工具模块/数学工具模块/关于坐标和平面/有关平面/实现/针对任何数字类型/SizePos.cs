using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个类型是<see cref="ISizePos{Num}"/>的实现，
/// 可以视为一个具有位置的二维平面
/// </summary>
/// <inheritdoc cref="ISizePos{Num}"/>
abstract class SizePos<Num> : ISizePos<Num>
    where Num : INumber<Num>
{
    #region 二维平面的位置
    public required IPoint<Num> Position { get; init; }
    #endregion
    #region 二维平面的大小
    public required ISize<Num> Size { get; init; }
    #endregion
    #region IEquatable的实现
    public bool Equals(ISizePos<Num>? other)
        => other is { } &&
        other.Position.Equals(Position) &&
        other.Size.Equals(Size);
    #endregion
    #region 抽象成员：返回全部四个顶点
    public abstract IReadOnlyList<IPoint<Num>> Vertex { get; }
    #endregion
}
