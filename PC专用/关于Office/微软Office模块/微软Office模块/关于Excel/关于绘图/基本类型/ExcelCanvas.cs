
using System.Media.Drawing.Graphics;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="ICanvas"/>的实现，
/// 它可以在Excel上绘制图形
/// </summary>
class ExcelCanvas : ICanvas
{
    #region 封装的图形集合
    /// <summary>
    /// 获取封装的图形集合，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Shapes PackShapes { get; }
    #endregion
    #region 枚举所有Excel图形
    public ICollection<IGraphics> Details => throw new NotImplementedException();
    #endregion
    #region 获取图形创建器
    public ICreateGraphics CreateGraphics { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的图形集合封装进对象
    /// </summary>
    /// <param name="packShapes">指定的图形集合</param>
    public ExcelCanvas(Shapes packShapes)
    {
        this.PackShapes = packShapes;
        CreateGraphics = new ExcelCreateGraphics(this.PackShapes);
    }
    #endregion
}
