using System.Maths.Plane;

namespace System.Drawing;

/// <summary>
/// 这个静态类型提供从UI坐标到数学坐标的转换
/// </summary>
public static class ExtenUIPoint
{
    #region 将坐标在数学坐标和UI坐标之间互相转换
    /// <summary>
    /// 将坐标在数学坐标和UI坐标之间互相转换，
    /// 也就是将它的Y轴镜像
    /// </summary>
    /// <param name="point">待转换的坐标</param>
    /// <returns></returns>
    public static IPoint ConvertMU(this IPoint point)
        => point.Mirror(false, true);
    #endregion
}
