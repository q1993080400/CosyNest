namespace System.Maths.Plane;

/// <summary>
/// 这个类型是<see cref="IPoint"/>的实现，
/// 可以用来表示一个二维坐标
/// </summary>
sealed record Point(Num Right, Num Top) : IPoint
{
    #region 重写ToString
    public override string ToString()
        => $"Right:{Right}，Top:{Top}";
    #endregion
    #region IEquatable的实现
    public bool Equals(IPoint? other)
        => other is { } && Right == other.Right && Top == other.Top;
    #endregion
}
