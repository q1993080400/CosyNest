namespace System.Maths.Plane;

/// <summary>
/// 这个类型是<see cref="IVector"/>的实现，
/// 可以用来表示一个向量
/// </summary>
sealed record Vector(Num Length, IUnit<IUTAngle> Direction) : IVector
{
    #region 重写ToString
    public override string ToString()
        => $"极径：{Length}  极角（角度）：{Direction.Convert(IUTAngle.AngleMetric)}";
    #endregion
}
