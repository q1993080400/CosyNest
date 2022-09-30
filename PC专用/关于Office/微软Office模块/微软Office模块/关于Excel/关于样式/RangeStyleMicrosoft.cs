using Microsoft.Office.Interop.Excel;
using EXRange = Microsoft.Office.Interop.Excel.Range;
using ColorTranslator = System.Drawing.ColorTranslator;
using System.Media.Drawing;
using System.Media.Drawing.Text;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是IRangStyle的实现，可以作为一个单元格样式
/// </summary>
class RangeStyleMicrosoft : IRangeStyle
{
    #region 封装的对象
    #region 封装的单元格
    /// <summary>
    /// 获取封装的单元格，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private EXRange PackRange { get; }
    #endregion
    #region 封装单元格的内部
    /// <summary>
    /// 获取代表单元格内部的对象
    /// </summary>
    private Interior PackInterior
        => PackRange.Interior;
    #endregion
    #region 代表没有颜色的颜色索引
    /// <summary>
    /// 如果<see cref="Interior.ColorIndex"/>等于这个值，
    /// 则代表这个单元格没有颜色
    /// </summary>
    private const int NotColor = -4142;
    #endregion
    #endregion
    #region 背景颜色
    public IColor? BackColor
    {
        get => PackInterior.ColorIndex is NotColor ?
                null : ColorTranslator.FromOle((int)PackInterior.Color).ToColor();
        set
        {
            if (value is null)
                PackInterior.ColorIndex = NotColor;
            else PackInterior.Color = value.ToColor();
        }
    }
    #endregion
    #region 关于格式
    #region 数字格式
    public string Format
    {
        get => PackRange.NumberFormat;
        set => PackRange.NumberFormat = value;
    }
    #endregion
    #region 文本格式
    private readonly ITextStyleVar TextStyleField;

    public ITextStyleVar TextStyle
    {
        get => TextStyleField;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            OfficeTextStyleCom.CacheStylePro.ForEach(x => x.Copy(value, TextStyleField));
        }
    }
    #endregion
    #endregion
    #region 关于对齐
    #region 获取或设置自动换行
    public bool AutoLineBreaks
    {
        get => PackRange.WrapText is true;
        set => PackRange.WrapText = value;
    }
    #endregion
    #region 垂直对齐
    #region 双向映射表
    /// <summary>
    /// 获取垂直对齐的双向映射表
    /// </summary>
    private static ITwoWayMap<XlVAlign, OfficeAlignment> MapVertical { get; }
    = CreateCollection.TwoWayMap(
        (XlVAlign.xlVAlignCenter, OfficeAlignment.Center),
        (XlVAlign.xlVAlignTop, OfficeAlignment.LeftOrTop),
        (XlVAlign.xlVAlignBottom, OfficeAlignment.RightOrBottom),
        (XlVAlign.xlVAlignJustify, OfficeAlignment.Ends));
    #endregion
    #region 正式属性
    public OfficeAlignment AlignmentVertical
    {
        get => MapVertical.TryAMapB(
            (XlVAlign)PackRange.VerticalAlignment, OfficeAlignment.Unknown).Value;
        set => PackRange.VerticalAlignment = MapVertical.BMapA(value);
    }
    #endregion
    #endregion
    #region 水平对齐
    #region 双向映射表
    /// <summary>
    /// 获取水平对齐的双向映射表
    /// </summary>
    private static ITwoWayMap<XlHAlign, OfficeAlignment> MapHorizontal { get; }
    = CreateCollection.TwoWayMap(
        (XlHAlign.xlHAlignCenter, OfficeAlignment.Center),
        (XlHAlign.xlHAlignLeft, OfficeAlignment.LeftOrTop),
        (XlHAlign.xlHAlignRight, OfficeAlignment.RightOrBottom),
        (XlHAlign.xlHAlignJustify, OfficeAlignment.Ends));
    #endregion
    #region 正式属性
    public OfficeAlignment AlignmentHorizontal
    {
        get => MapHorizontal.TryAMapB(
            (XlHAlign)PackRange.HorizontalAlignment, OfficeAlignment.Unknown).Value;
        set => PackRange.HorizontalAlignment = MapHorizontal.TryBMapA(value);
    }
    #endregion
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的单元格封装进对象
    /// </summary>
    /// <param name="packRange">被封装的单元格</param>
    public RangeStyleMicrosoft(EXRange packRange)
    {
        this.PackRange = packRange;
        TextStyleField = new RangeTextStyleMicrosoft(packRange.Font);
    }
    #endregion
}
