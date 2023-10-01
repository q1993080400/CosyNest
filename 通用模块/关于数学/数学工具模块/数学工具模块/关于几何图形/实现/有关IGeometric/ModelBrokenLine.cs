namespace System.MathFrancis.Plane.Geometric;

/// <summary>
/// 这个几何模型可以用来创建由多条折线组合而成的几何图形
/// </summary>
sealed class ModelBrokenLine : IGeometricModel<IGeometric>
{
    #region 枚举折线的端点
    /// <summary>
    /// 枚举所有折线的端点，
    /// 将它们连接起来可以组成几何图形
    /// </summary>
    private IEnumerable<IPoint> Points { get; }
    #endregion
    #region 描绘几何图形
    public IGeometric Draw(IPoint? position = null)
    {
        var arry = Points.Select(x => x.ToAbs(position ??= IPoint.Original)).
            Polymerization(CreateMath.GeometricLine).ToArray();
        return new BrokenLine(this, arry);
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="close">如果这个值为<see langword="true"/>，则自动闭合这个几何图形</param>
    /// <param name="points">组成几何图形的折线的端点，
    /// 第一个点被固定为(0,0)，不需要输入</param>
    public ModelBrokenLine(bool close, IEnumerable<IPoint> points)
    {
        points = points.Prepend(IPoint.Original).Distinct();
        Points = (close ? points.Append(IPoint.Original) : points).ToArray();
    }
    #endregion
}
