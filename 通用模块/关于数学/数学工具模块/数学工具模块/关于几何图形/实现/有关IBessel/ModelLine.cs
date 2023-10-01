namespace System.MathFrancis.Plane.Geometric;

/// <summary>
/// 这个几何模型可以用来绘制线段
/// </summary>
/// <remarks>
/// 使用指定的终点初始化模型
/// </remarks>
/// <param name="end">线段的终点，起点统一为(0,0)</param>
sealed class ModelLine(IPoint end) : IGeometricModel<IBessel>
{
    #region 返回线段的终点
    /// <summary>
    /// 返回线段的终点，
    /// 起点统一为(0,0)
    /// </summary>
    private IPoint End { get; } = end;
    #endregion
    #region 绘制几何图形
    public IBessel Draw(IPoint? position)
        => new Line(this, position ??= IPoint.Original, End.ToAbs(position));

    #endregion
}
