using System.IOFrancis.FileSystem;
using System.Media.Drawing;
using System.Media.Drawing.Graphics;
using System.Office.Chart;
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
    private ExcelWorksheet Sheet { get; }
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
    #region 删除工作表
    public override void Delete()
        => Sheets.Delete(Sheet);
    #endregion
    #region 复制工作表
    public override IExcelSheet Copy(IExcelSheetCollection? collection = null, Func<string, int, string>? renamed = null)
    {
        collection ??= Book.Sheets;
        var sheets = collection.To<ExcelSheetCollectionEPPlus>().Sheets;
        var newSheet = sheets.Add(ExcelRealizeHelp.SheetRepeat(Book.Sheets, Name, renamed), Sheet);
        return new ExcelSheetEPPlus(collection.Book, newSheet);
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
    public override IExcelRC GetRC(int begin, int? end, bool isRow)
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
    public override IPageSheet Page => throw new NotImplementedException();
    #endregion
    #region 获取图表创建器
    public override ICreateExcelChart CreateChart => throw new NotImplementedException();
    #endregion
    #region 获取图表集合
    public override IEnumerable<IExcelObj<IOfficeChart>> Charts => throw new NotImplementedException();
    #endregion
    #region 获取图片集合
    public override IEnumerable<IExcelObj<IImage>> Images => throw new NotImplementedException();
    #endregion
    #region 创建图片
    public override IExcelObj<IImage> CreateImage(IImage image)
    {
        throw new NotImplementedException();
    }

    public override IExcelObj<IImage> CreateImage(PathText path)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region 获取画布
    public override ICanvas Canvas => throw new NotImplementedException();
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelSheet(IExcelBook)"/>
    /// <param name="sheet">封装的工作表，本对象的功能就是通过它实现的</param>
    public ExcelSheetEPPlus(IExcelBook book, ExcelWorksheet sheet)
        : base(book)
    {
        Sheet = sheet;
        Cell = new ExcelCellsEPPlus(this, Sheet.Cells);
    }
    #endregion
}
