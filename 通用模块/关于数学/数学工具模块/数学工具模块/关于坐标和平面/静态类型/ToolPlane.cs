
using static System.Maths.CreateMath;

namespace System.Maths.Plane;

/// <summary>
/// 关于坐标和平面的工具类
/// </summary>
public static class ToolPlane
{
    #region 关于IPoint
    #region 返回相对位置
    /// <summary>
    /// 以一个点为原点，
    /// 返回另一个点相对于它的位置
    /// </summary>
    /// <param name="original">原点</param>
    /// <param name="relative">另一个点，注意：它是绝对坐标</param>
    /// <returns></returns>
    public static PointPos RelativePos(IPoint original, IPoint relative)
    {
        var (or, ot) = original;
        var (rr, rt) = relative;
        var equalsR = or == rr;
        var equalsT = ot == rt;
        if (equalsR && equalsT)
            return PointPos.Coincidence;
        var comT = rt > ot;
        if (equalsR)
            return comT ? PointPos.Above : PointPos.Below;
        var comR = rr > or;
        if (equalsT)
            return comR ? PointPos.Right : PointPos.Left;
        if (comR)
            return comT ? PointPos.Q1 : PointPos.Q4;
        return comT ? PointPos.Q2 : PointPos.Q3;
    }
    #endregion
    #endregion
    #region 关于ISize及其衍生类
    #region 排列一个矩形
    /// <summary>
    /// 传入一个矩形，将它在一个空间中按照水平或垂直方向排列，
    /// 如有超出则会换行，并返回第N个矩形的界限，
    /// 注意：这个方法假定排列空间的下端或右端是无限的
    /// </summary>
    /// <param name="sizePos">要排列的<see cref="ISize"/></param>
    /// <param name="n">指示排列到第N个矩形，从1开始</param>
    /// <param name="isVertical">如果这个值为<see langword="true"/>，则垂直排列，否则水平排列</param>
    /// <param name="isPositive">如果这个值为<see langword="true"/>，则向上或向右排列，否则向下或向左排列</param>
    /// <returns></returns>
    public static ISizePos Arranged(ISizePos sizePos, int n, bool isVertical = true, bool isPositive = true)
    {
        n--;
        var (w, h) = sizePos;
        var row = isVertical ? n : 0;
        var col = isVertical ? 0 : n;
        var rightTop = Point(col * w, row * h);
        var pos = (isPositive ? rightTop : rightTop.Mirror(!isVertical, isVertical)).ToAbs(sizePos.Position);
        return CreateMath.SizePos(pos, sizePos);
    }
    #endregion
    #region 返回平面的界限
    #region 传入IPoint
    /// <summary>
    /// 传入若干个坐标，
    /// 返回容纳它们所需要的最小平面
    /// </summary>
    /// <param name="points">传入的坐标</param>
    /// <returns></returns>
    public static ISizePos Boundaries(params IPoint[] points)
    {
        switch (points.Length)
        {
            case 0:
                return SizePos(0, 0, 0, 0);
            case 1:
                return SizePos(points[0], 0, 0);
            default:
                var (lx, ly) = points[0];
                var (rx, ry) = (lx, ly);
                foreach (var p in points[1..])
                {
                    var (x, y) = p;
                    if (x < lx)
                        lx = x;
                    else if (x > rx)
                        rx = x;
                    if (y > ly)
                        ly = y;
                    else if (y < ry)
                        ry = y;
                }
                return SizePos(Point(lx, ly), Point(rx, ry));
        }
    }
    #endregion
    #region 传入ISizePos
    /// <summary>
    /// 返回一个平面数组的界限，
    /// 也就是容纳数组中所有平面，所需要的最小矩形
    /// </summary>
    /// <param name="plane">要返回界限的平面数组</param>
    /// <returns></returns>
    public static ISizePos Boundaries(params ISizePos[] plane)
    {
        var point = plane.Select(x =>
        {
            var (left, right) = x.Boundaries;
            return new[] { left, right };
        }).UnionNesting(false);
        return Boundaries(point.ToArray());
    }
    #endregion
    #endregion
    #endregion
}
#region 关于象限的枚举
/// <summary>
/// 以一个点O为原点，
/// 这个枚举指示另一个点P相对于它的位置
/// </summary>
public enum PointPos
{
    /// <summary>
    /// 点P处于第一象限
    /// </summary>
    Q1,
    /// <summary>
    /// 点P处于第二象限
    /// </summary>
    Q2,
    /// <summary>
    /// 点P处于第三象限
    /// </summary>
    Q3,
    /// <summary>
    /// 点P处于第四象限
    /// </summary>
    Q4,
    /// <summary>
    /// 两个点都位于Y轴，而且P在O的上面
    /// </summary>
    Above,
    /// <summary>
    /// 两个点都位于Y轴，而且P在O的下面
    /// </summary>
    Below,
    /// <summary>
    /// 两个点都位于X轴，而且P在O的左边
    /// </summary>
    Left,
    /// <summary>
    /// 两个点都位于X轴，而且P在O的右边
    /// </summary>
    Right,
    /// <summary>
    /// 两个点完全重合
    /// </summary>
    Coincidence
}
#endregion