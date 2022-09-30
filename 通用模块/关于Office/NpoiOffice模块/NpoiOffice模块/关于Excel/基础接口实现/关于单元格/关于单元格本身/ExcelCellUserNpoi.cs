
using static System.Office.Excel.Realize.ExcelRealizeHelp;

namespace System.Office.Excel;

/// <summary>
/// 该类型表示一个由Npoi实现的，
/// 专门用于<see cref="IExcelSheet.Cell"/>的单元格
/// </summary>
sealed class ExcelCellUserNpoi : ExcelCellNpoi
{
    #region 辅助方法：抛出因不支持功能产生的异常
    /// <summary>
    /// 直接抛出一个异常，
    /// 表示由于本单元格性质特殊，无法完成指定的功能
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">永远抛出此异常</exception>
    private static dynamic ThrowException()
        => throw new NotImplementedException("该单元格性质特殊，表示工作表中的所有单元格，不能完成此功能");
    #endregion
    #region 关于单元格本身
    #region 单元格的值
    public override RangeValue Value
    {
        get => ThrowException();
        set => ThrowException();
    }
    #endregion
    #region 单元格的公式
    public override string? FormulaA1
    {
        get => ThrowException();
        set => ThrowException();
    }
    #endregion
    #region 单元格地址
    public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; }
    #endregion
    #endregion
    #region 关于子单元格与其他单元格
    #region 枚举所有子单元格
    public override IEnumerable<IExcelCells> Cells
        => throw new NotSupportedException("禁止枚举整个工作表上的所有子单元格，这会导致程序卡死");
    #endregion
    #region 是否合并
    public override bool IsMerge
    {
        get => false;
        set => ThrowException();
    }
    #endregion
    #region 根据索引返回子单元格
    protected override IExcelCells IndexTemplate(int beginRow, int beginColumn, int endRow, int endColumn)
    {
        if (beginRow == endRow && beginColumn == endColumn)
        {
            var row = SheetNpoi.GetRow(beginRow) ?? SheetNpoi.CreateRow(beginRow);
            var cell = row.GetCell(beginColumn) ?? row.CreateCell(beginColumn);
            return new ExcelCellSingleNpoi(Sheet, cell);
        }
        return new ExcelCellMultipleNpoi(Sheet, (beginRow, beginColumn, endRow, endColumn));
    }
    #endregion
    #region 替换单元格
    public override void Replace(string content, string replace)
         => throw new NotImplementedException();
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelCells(IExcelSheet)"/>
    public ExcelCellUserNpoi(IExcelSheet sheet)
        : base(sheet)
    {
        var is2007 = Interface.Book.To<ExcelBookNpoi>().Is2007;
        var (er, ec) = (is2007 ? MaxRow2007 : MaxRow2007, is2007 ? MaxColumn2007 : MaxColumn2003);
        Address = (0, 0, er - 1, ec - 1);
    }
    #endregion
}
