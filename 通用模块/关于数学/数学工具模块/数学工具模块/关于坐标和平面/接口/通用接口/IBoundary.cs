namespace System.MathFrancis.Plane;

/// <summary>
/// 凡是实现这个接口的类型，都可以视为一个边界，
/// 它是平面上的一个封闭矩形界限
/// </summary>
public interface IBoundary
{
    #region 命中检测
    #region 传入点
    /// <summary>
    /// 返回一个点是否在平面内部
    /// </summary>
    /// <param name="point">待检查的点</param>
    /// <returns></returns>
    bool Contains(IPoint point)
    {
        var (r, t) = point;
        var ((tr, tt), (br, bt)) = Boundaries;
        return r >= tr && r <= br && t <= tt && t >= bt;
    }
    #endregion
    #region 传入界限
    /// <summary>
    /// 返回一个界限是否完全位于这个界限内部
    /// </summary>
    /// <param name="boundary">待检查的界限</param>
    /// <returns></returns>
    bool Contains(IBoundary boundary)
    {
        var (tl, br) = boundary.Boundaries;
        return Contains(tl) && Contains(br);
    }
    #endregion
    #endregion
    #region 关于顶点
    #region 返回全部四个顶点
    /// <summary>
    /// 返回一个集合，
    /// 它按照左上，右上，右下，左下的顺序枚举二维平面的四个顶点
    /// </summary>
    IReadOnlyList<IPoint> Vertex { get; }
    #endregion
    #region 返回左上角和右下角的顶点
    /// <summary>
    /// 返回这个平面的界限，
    /// 也就是它左上角和右下角的顶点
    /// </summary>
    (IPoint TopLeft, IPoint BottomRight) Boundaries
    {
        get
        {
            var vertex = Vertex;
            return (vertex[0], vertex[2]);
        }
    }
    #endregion
    #endregion
}
