﻿using System.MathFrancis;

namespace System.Office.Realize;

/// <summary>
/// 这个静态类为实现Office对象提供帮助
/// </summary>
public static class OfficeRealizeHelp
{
    #region 返回一个坐标的绝对值
    /// <summary>
    /// 将一个二维坐标的X和Y取绝对值，并返回一个新的坐标
    /// </summary>
    /// <param name="point">待取绝对值的坐标</param>
    /// <returns></returns>
    public static IPoint<int> Abs(this IPoint<int> point)
        => CreateMath.Point(int.Abs(point.Right), int.Abs(point.Top));
    #endregion
}
