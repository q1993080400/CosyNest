using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个接口代表一个具有位置的二维平面
/// </summary>
/// <typeparam name="Num">用来描述平面的数字类型</typeparam>
public interface ISizePos<Num> : IEquatable<ISizePos<Num>>
    where Num : INumber<Num>
{
    #region 二维平面的位置
    /// <summary>
    /// 返回二维平面左上角的坐标
    /// </summary>
    IPoint<Num> Position { get; }
    #endregion
    #region 二维平面的大小
    /// <summary>
    /// 返回二维平面的大小
    /// </summary>
    ISize<Num> Size { get; }
    #endregion
    #region 命中检测
    #region 传入点
    /// <summary>
    /// 返回一个点是否在平面内部
    /// </summary>
    /// <param name="point">待检查的点</param>
    /// <returns></returns>
    bool Contains(IPoint<Num> point)
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
    bool Contains(ISizePos<Num> boundary)
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
    IReadOnlyList<IPoint<Num>> Vertex { get; }
    #endregion
    #region 返回左上角和右下角的顶点
    /// <summary>
    /// 返回这个平面的界限，
    /// 也就是它左上角和右下角的顶点
    /// </summary>
    (IPoint<Num> TopLeft, IPoint<Num> BottomRight) Boundaries
    {
        get
        {
            var vertex = Vertex;
            return (vertex[0], vertex[2]);
        }
    }
    #endregion
    #endregion
    #region 解构ISizePos
    /// <summary>
    /// 将本对象解构为位置和大小
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="size">大小</param>
    void Deconstruct(out IPoint<Num> pos, out ISize<Num> size)
    {
        pos = Position;
        size = Size;
    }
    #endregion
}
