using System.Maths;
using System.Maths.Plane.Geometric;
using System.Media.Drawing.Graphics;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="IGraphicsGeometric{GraphicsGeometric}"/>的实现，
/// 可以视为一个用Excel绘制的几何图形
/// </summary>
/// <typeparam name="GeometricType">几何图形的类型</typeparam>
class ExcelGraphicsGeometric<GeometricType> : ExcelGraphics, IGraphicsGeometric<GeometricType>
    where GeometricType : IGeometric
{
    #region 获取或设置样式
    #region 隐式实现
    public IGraphicsStyleShape Style
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    #endregion
    #region 显式实现
    IGraphicsStyle IGraphicsHasStyle.Style => Style;
    #endregion
    #endregion
    #region 获取或设置几何图形
    public GeometricType Geometric { get; }
    #endregion
    #region 构造函数
    #region 指定图形集合和几何图形
    /// <summary>
    /// 使用指定的图形集合和几何图形初始化对象
    /// </summary>
    /// <param name="packShapes">图形集合，它负责将图形添加或移除至Excel工作表</param>
    /// <param name="geometric">指定的几何图形</param>
    public ExcelGraphicsGeometric(Shapes packShapes, GeometricType geometric)
    {
        var con = geometric.Content;
        if (con.Any(x => x.Properties is BesselProperties.Curve))
            throw new NotSupportedException("用来描绘Excel图形的线段中存在曲线，暂不支持这个操作");
        PackShape = packShapes.AddPolyline(con.AllPoint().ToArray().ToOfficePoint());
        this.Geometric = geometric;
    }
    #endregion
    #endregion
}
