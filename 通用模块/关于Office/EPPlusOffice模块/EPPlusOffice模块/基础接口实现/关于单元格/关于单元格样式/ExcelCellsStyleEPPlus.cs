using OfficeOpenXml.Style;

using System.Media.Drawing;
using System.Media.Drawing.Text;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是底层使用EPPlus实现的单元格样式
/// </summary>
/// <remarks>
/// 使用指定的单元格初始化对象
/// </remarks>
/// <param name="cell">指定的单元格</param>
sealed class ExcelCellsStyleEPPlus(ExcelCellsEPPlus cell) : IRangeStyle
{
    #region 内部成员
    #region 封装的单元格
    /// <summary>
    /// 获取封装的单元格，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private ExcelCellsEPPlus Cell { get; } = cell;
    #endregion
    #region 单元格样式
    /// <summary>
    /// 获取单元格的样式
    /// </summary>
    private ExcelStyle Style => Cell.Range.Style;
    #endregion
    #endregion
    #region 公开成员
    #region 背景颜色
    public IColor? BackColor
    {
        get
        {
            var fill = Style.Fill;
            return fill.PatternType is ExcelFillStyle.Solid ?
                CreateDrawingObj.Color(fill.BackgroundColor.Rgb) : null;
        }
        set
        {
            var fill = Style.Fill;
            if (value is null)
            {
                fill.PatternType = ExcelFillStyle.None;
                return;
            }
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(value.ToColor());
        }
    }
    #endregion
    #endregion
    #region 未实现的成员
    public string Format { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ITextStyleVar TextStyle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool AutoLineBreaks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public OfficeAlignment AlignmentVertical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public OfficeAlignment AlignmentHorizontal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    #endregion
    #region 构造函数
    #endregion
}
