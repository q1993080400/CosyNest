﻿namespace System.MathFrancis.Plane.Geometric;

/// <summary>
/// 这个类型代表由一条或多条折线组合而成的几何图形
/// </summary>
/// <remarks>
/// 使用指定的几何模型和折线初始化对象
/// </remarks>
/// <param name="model">创建这个几何图形的模型</param>
/// <param name="content">用来枚举几何图形每条折线的枚举器</param>
sealed class BrokenLine(IGeometricModel<IGeometric> model, IBessel[] content) : IGeometric
{
    #region 返回几何模型
    public IGeometricModel<IGeometric> Model { get; } = model;
    #endregion
    #region 返回几何图形的每个部分
    public IEnumerable<IBessel> Content { get; } = content;
    #endregion
    #region 返回几何图形的界限
    private ISizePos? BoundariesField;

    public ISizePos Boundaries
        => BoundariesField ??= ToolPlane.Boundaries(Content.AllPoint(true).ToArray());

    #endregion
}
