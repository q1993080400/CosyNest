using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="ExcelSheet"/>的实现，
/// 是一个通过EPPlus实现的Excel工作表
/// </summary>
sealed class ExcelSheetEPPlus : ExcelSheet
{
    #region 封装的对象
    #region 工作表
    /// <summary>
    /// 获取封装的工作表，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ExcelWorksheet Sheet { get; }
    #endregion
    #region 工作表集合
    /// <summary>
    /// 获取封装的工作表集合，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private ExcelWorksheets Sheets
        => Sheet.Workbook.Worksheets;
    #endregion
    #endregion
    #region 关于工作表
    #region 名称
    public override string Name
    {
        get => Sheet.Name;
        set => Sheet.Name = value;
    }
    #endregion
    #region 工作表的索引
    public override int Index
    {
        get => Sheet.Index;
        set => Sheet.Workbook.Worksheets.MoveBefore(Index, value);
    }
    #endregion
    #region 删除工作表
    public override void Delete()
        => Sheets.Delete(Sheet);
    #endregion
    #region 复制工作表
    public override IExcelSheet Copy(IExcelSheetManage? collection = null, Func<string, int, string>? renamed = null)
    {
        collection ??= Book.SheetManage;
        var sheets = collection.To<ExcelSheetManageEPPlus>().EPPlusSheets;
        var newName = ExcelRealizeHelp.SheetRepeat(sheets.Select(x => x.Name), Name, renamed);
        var newSheet = sheets.Add(newName, Sheet);
        return new ExcelSheetEPPlus(collection, newSheet);
    }
    #endregion
    #endregion
    #region 关于单元格
    #region 根据索引获取子单元格
    public override IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1]
        => Cell[beginRow, beginColumn, endRow, endColumn];
    #endregion
    #region 返回所有单元格
    public override IExcelCells Cell { get; }
    #endregion
    #region 返回行或列
    public override IExcelRC GetRC(bool isRow, int begin, int? end)
    {
        begin++;
        var e = end is { } ev ? ev + 1 : begin;
        var range = CreateCollection.RangeBE(begin, e);
        return isRow ?
            new ExcelRowEPPlus(this, range.Select(Sheet.Row), begin, e) :
            new ExcelColumnEPPlus(this, range.Select(Sheet.Column), begin, e);
    }
    #endregion
    #endregion
    #region 关于打印和Excel对象
    #region 获取页面对象
    public override ISheetPage Page => throw new NotImplementedException();
    #endregion
    #region 返回图表管理对象
    public override IOfficeObjectManageCommon<IOfficeChart> ChartManage { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelSheet(IExcelSheetManage)"/>
    /// <param name="sheet">封装的工作表，本对象的功能就是通过它实现的</param>
    public ExcelSheetEPPlus(IExcelSheetManage sheetManage, ExcelWorksheet sheet)
        : base(sheetManage)
    {
        Sheet = sheet;
        Cell = new ExcelCellsEPPlus(this, Sheet.Cells);
        ChartManage = new ExcelChartManageEPPlus(sheet.Drawings);
    }
    #endregion
}
