using System.Maths.Plane.Geometric;
using System.Media.Drawing.Graphics;

using Microsoft.Office.Interop.Excel;

using IPoint = System.Maths.Plane.IPoint;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="ICreateGraphics"/>的实现，
/// 可以用来创建Excel图形
/// </summary>
class ExcelCreateGraphics : ICreateGraphics
{
    #region 封装的图形集合
    /// <summary>
    /// 获取封装的图形集合，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Shapes PackShapes { get; }
    #endregion
    #region 创建图形
    #region 创建文本
    public IGraphicsText CreateText(string text, IPoint? point = null)
        => throw new NotImplementedException();
    #endregion
    #region 创建几何图形
    public IGraphicsGeometric<Geometric> CreateGeometric<Geometric>(IGeometricModel<Geometric> model, IPoint? point = null)
          where Geometric : IGeometric
        => new ExcelGraphicsGeometric<Geometric>(PackShapes, model.Draw(point));
    #endregion
    #endregion
    #region 创建样式
    public Style CreateStyle<Style>() where Style : IGraphicsStyle
        => throw new NotImplementedException();
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的图形集合初始化对象
    /// </summary>
    /// <param name="packShapes">指定的图形集合，
    /// 本对象的功能就是通过它实现的</param>
    public ExcelCreateGraphics(Shapes packShapes)
    {
        this.PackShapes = packShapes;
    }
    #endregion
}
