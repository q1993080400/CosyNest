using System.Office.Excel.Realize;

using ExcelRange = Microsoft.Office.Interop.Excel.Range;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是由微软COM组件实现的Excel行列
/// </summary>
class ExcelRCMicrosoft : ExcelRC
{
    #region 封装的单元格
    /// <summary>
    /// 获取封装的单元格，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ExcelRange PackRange { get; }
    #endregion
    #region 关于行与列的样式
    #region 隐藏和取消隐藏
    public override bool? IsHide
    {
        get
        {
            var (first, other, _) = PackRange.OfType<ExcelRange>().Select(x => (object)x.Hidden).First(true);
            foreach (var item in other)
            {
                if (!Equals(first, item))
                    return null;
            }
            return (bool)first;
        }
        set => PackRange.Hidden = value is null ?
            throw new NotSupportedException($"{nameof(IsHide)}禁止写入null值") :
            value.Value;
    }
    #endregion
    #region 获取或设置高度或宽度
    public override double? HeightOrWidth
    {
        get => (double?)(IsRow ? PackRange.RowHeight : PackRange.ColumnWidth);
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (IsRow)
                PackRange.RowHeight = value.Value;
            else PackRange.ColumnWidth = value.Value;
        }
    }
    #endregion
    #region 设置或获取样式
    private IRangeStyle? StyleField;

    public override IRangeStyle Style
    {
        get => StyleField ??= new RangeStyleMicrosoft(PackRange);
        set => ExcelRealizeHelp.CopyStyle(value, Style);
    }
    #endregion
    #region 自动调整行高与列宽
    public override void AutoFit()
    {
        foreach (var item in (IsRow ? PackRange.Rows : PackRange.Columns).OfType<ExcelRange>())
        {
            item.AutoFit();
        }
    }
    #endregion
    #endregion
    #region 删除行或者列
    public override void Delete()
        => PackRange.Delete();
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelRC(IExcelSheet, bool, int, int)"/>
    /// <param name="packRange">被封装的范围</param>
    public ExcelRCMicrosoft(IExcelSheet sheet, ExcelRange packRange, bool isRow, int begin, int end)
        : base(sheet, isRow, begin, end)
    {
        this.PackRange = isRow ? packRange.Rows : packRange.Columns;
    }
    #endregion
}
