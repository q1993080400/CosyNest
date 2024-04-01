using System.MathFrancis.Plane;
using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是使用EPPlus实现的Excel单元格
/// </summary>
sealed class ExcelCellsEPPlus : ExcelCells, IExcelCells
{
    #region 公开成员
    #region 关于单元格本身
    #region 关于值和公式
    #region 单元格的值
    public override RangeValue Value
    {
        get => new() { Content = Range.Value };
        set
        {
            var (r, c) = Interface.RCCount;
            var content = value.Content;
            Range.Value = (r is not 1 || c is not 1) && content is object[] array ?       //EPPlus底层只认识二维数组，需要进行转换
                array.ToArray(r, c) : content;
            if (content is DateTime or DateTimeOffset)
                Range.Style.Numberformat.Format = "yyyy/M/d";
        }
    }
    #endregion
    #region 关于公式
    #region A1格式
    public override string? FormulaA1
    {
        get => Range.Formula switch
        {
            null or "" => null,
            var t => t
        };
        set => SetFormulaAssist(value, value => Range.Formula = value, false);
    }
    #endregion
    #region R1C1格式
    public override string? FormulaR1C1
    {
        get => Range.FormulaR1C1 switch
        {
            null or "" => null,
            var t => t
        };
        set => SetFormulaAssist(value, value => Range.FormulaR1C1 = value, false);
    }
    #endregion
    #endregion
    #endregion 
    #region 单元格地址
    public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; }
    #endregion
    #region 复制单元格
    public override IExcelCells Copy(IExcelCells cells)
        => throw new NotImplementedException();
    #endregion
    #region 单元格样式
    public override IRangeStyle Style
    {
        get => new ExcelCellsStyleEPPlus(this);
        set => throw new NotImplementedException();
    }
    #endregion
    #endregion
    #region 关于子单元格和其他单元格
    #region 枚举非空单元格
    public IEnumerable<IExcelCells> CellsNotNull
    {
        get
        {
            foreach (var item in Range)
            {
                if (item.Value is { })
                    yield return new ExcelCellsEPPlus(Sheet, item);
            }
        }
    }
    #endregion
    #region 按索引获取子单元格
    protected override IExcelCells IndexTemplate(int beginRow, int beginColumn, int endRow, int endColumn)
    {
        switch (Range)
        {
            case OfficeOpenXml.ExcelRange r:
                var (br, bc, _, _) = Address;
                beginRow += br + 1;
                beginColumn += bc + 1;
                endRow = br + 1 + endRow;
                endColumn = bc + 1 + endColumn;
                return new ExcelCellsEPPlus(Sheet, r[beginRow, beginColumn, endRow, endColumn]);
            case ExcelRangeBase when (beginRow, beginColumn, endRow, endColumn) is (0, 0, 0, 0):
                return this;
            default:
                throw new ArgumentException("仅有一个单元格，除非地址为(0,0,0,0)，否则无法获取子单元格");
        }
    }
    #region 说明文档
    /*EPPlus的ExcelRange的索引器设计非常古怪，
      它返回的不是相对于单元格的Range，
      而是相对于整个工作表的Range，因此此处需要手动将相对坐标转换为绝对坐标*/
    #endregion
    #endregion
    #region 是否合并单元格
    public override bool IsMerge
    {
        get => Range.Merge;
        set => Range.Merge = value;
    }
    #endregion
    #region 替换单元格
    public override void Replace(string content, string replace)
         => throw new NotImplementedException();
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 封装的Excel单元格
    private ExcelRangeBase RangeField;

    /// <summary>
    /// 获取封装的Excel单元格，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ExcelRangeBase Range
    {
        get
        {
            var newRange = RangeField.Worksheet.Cells[RangeField.Address];
            RangeField.Dispose();
            return RangeField = newRange;
        }
    }

    /*问：为什么此处需要每次访问该属性，都要返回一个新的ExcelRangeBase对象？
      答：这是因为EPPlus的设计存在非常大的问题，它很可能在多个单元格之间共享状态，
      这导致了在某些情况下，即便是单线程获取某些重叠的单元格，且不修改它们，
      都有可能造成单元格错乱，本设计的目的就是为了解决这个问题*/
    #endregion
    #endregion
    #region 未实现的成员
    public override string? Link { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override ISizePos VisualPosition => throw new NotImplementedException();

    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelCells(IExcelSheet)"/>
    /// <param name="range">封装的Excel单元格，本对象的功能就是通过它实现的</param>
    public ExcelCellsEPPlus(IExcelSheet sheet, ExcelRangeBase range)
        : base(sheet)
    {
        RangeField = range;
        var start = range.Start;
        var row = start.Row - 1;
        var column = start.Column - 1;
        Address = (row, column, row + range.Rows - 1, column + range.Columns - 1);
    }
    #endregion
}
