namespace System.Maths.Plane.Geometric;

/// <summary>
/// 这个几何模型可以用来绘制线段
/// </summary>
sealed class ModelLine : IGeometricModel<IBessel>
{
    #region 返回线段的终点
    /// <summary>
    /// 返回线段的终点，
    /// 起点统一为(0,0)
    /// </summary>
    private IPoint End { get; }
    #endregion
    #region 绘制几何图形
    public IBessel Draw(IPoint? position)
        => new Line(this, position ??= IPoint.Original, End.ToAbs(position));
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的终点初始化模型
    /// </summary>
    /// <param name="end">线段的终点，起点统一为(0,0)</param>
    public ModelLine(IPoint end)
    {
        this.End = end;
    }
    #endregion
}
