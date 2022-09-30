using System.Maths;
using System.Media.Drawing.Graphics;

using Microsoft.Office.Interop.Excel;

using IPoint = System.Maths.Plane.IPoint;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是所有Excel图形的基类
/// </summary>
abstract class ExcelGraphics : IGraphicsVar
{
    #region 封装的对象
    #region 图形
#pragma warning disable CS8618
    /// <summary>
    /// 获取封装的图形，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected Shape PackShape { get; init; }
#pragma warning restore
    #endregion
    #endregion
    #region 获取或设置位置
    public IPoint Position
    {
        get => CreateMath.Point(PackShape.Left, -PackShape.Top);
        set
        {
            PackShape.Left = value.Right;
            PackShape.Top = -value.Top;
        }
    }
    #endregion
    #region 获取或设置图层
    public int Layer
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    #endregion
}
