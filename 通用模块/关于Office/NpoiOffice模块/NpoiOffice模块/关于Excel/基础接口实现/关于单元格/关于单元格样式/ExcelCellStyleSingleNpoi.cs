using NPOI.SS.UserModel;

using System.Drawing;
using System.Media.Drawing.Text;

using ICell = NPOI.SS.UserModel.ICell;
using IColor = System.Media.Drawing.IColor;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="IRangeStyle"/>的实现，
/// 可以视为一个底层由NPOI实现，适用于单个单元格的样式
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="cell">封装的单元格，
/// 本对象的功能就是通过它实现的</param>
sealed class ExcelCellStyleSingleNpoi(ICell cell) : IRangeStyle
{
    #region 公开成员
    #region 背景颜色
    public IColor? BackColor
    {
        get
        {
            var back = Style.FillBackgroundColor;
            return back is 64 ?
                null :
                MapColor.TryBMapA(back).Value ??
                throw new KeyNotFoundException("由于底层限制，本属性只支持有限的几种颜色，这种颜色不在支持范围内");
        }
        set => SetStyle(style =>
        {
            var isNull = value is null;
            style.FillForegroundColor = isNull ? (short)64 : MapColor.AMapB(value!);
            style.FillPattern = isNull ? FillPattern.NoFill : FillPattern.SolidForeground;
        });
    }
    #endregion
    #region 数字格式
    public string Format
    {
        get => Style.GetDataFormatString();
        set => SetStyle(style =>
        {
            var format = Cell.Sheet.Workbook.CreateDataFormat();
            style.DataFormat = format.GetFormat(value);
        });
    }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的单元格
    /// <summary>
    /// 获取封装的单元格，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private ICell Cell { get; } = cell;
    #endregion
    #region 单元格样式
    /// <summary>
    /// 获取单元格的样式
    /// </summary>
    private ICellStyle Style => Cell.CellStyle;
    #endregion
    #region 颜色双向映射表
    /// <summary>
    /// 映射颜色和颜色索引的双向映射表
    /// </summary>
    private static ITwoWayMap<IColor, short> MapColor { get; }
    = CreateCollection.TwoWayMap<IColor, short>((Color.Red.ToColor(), 10));
    #endregion
    #region 写入单元格样式
    /// <summary>
    /// 写入单元格样式
    /// </summary>
    /// <param name="action">用来写入单元格样式的委托，
    /// 它的参数就是待修改的原始单元格样式</param>
    private void SetStyle(Action<ICellStyle> action)
    {
        var style = Cell.Sheet.Workbook.CreateCellStyle();
        style.CloneStyleFrom(Style);
        action(style);
        Cell.CellStyle = style;
    }

    /*问：为什么需要本方法？
      答：首先问候NPOI作者全家，
      然后需要这个方法的原因在于：
      NPOI的样式写入真的很坑，如果不创建样式的克隆，
      写入样式时有可能会影响其他单元格的样式，
      而且必须要执行以下代码，否则对样式的修改不会生效：
    
      Cell.CellStyle=Style*/
    #endregion
    #endregion
    #region 未实现的成员
    public ITextStyleVar TextStyle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool AutoLineBreaks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public OfficeAlignment AlignmentVertical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public OfficeAlignment AlignmentHorizontal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    #endregion
    #region 构造函数
    #endregion
}
