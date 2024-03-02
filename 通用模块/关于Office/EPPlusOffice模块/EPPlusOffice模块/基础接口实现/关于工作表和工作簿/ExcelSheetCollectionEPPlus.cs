using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="ExcelSheetCollection"/>的实现，
/// 可以视为一个底层使用EPPlus的Excel工作表集合
/// </summary>
/// <inheritdoc cref="ExcelSheetCollection(IExcelBook)"/>
sealed class ExcelSheetCollectionEPPlus(IExcelBook excelBook) : ExcelSheetCollection(excelBook)
{
    #region 公开成员
    #region 工作表数量
    public override int Count => Sheets.Count;
    #endregion
    #region 插入工作表
    public override void Insert(int index, IExcelSheet item)
    {
        item.Copy(this);
        Sheets.MoveBefore(Count - 1, index);
    }
    #endregion
    #region 枚举工作表
    public override IEnumerator<IExcelSheet> GetEnumerator()
        => Sheets.Select(x => new ExcelSheetEPPlus(Book, x)).ToArray().
        To<IEnumerable<IExcelSheet>>().GetEnumerator();
    #endregion
    #region 添加工作表
    public override IExcelSheet Add(string name = "Sheet")
    {
        var sheet = Sheets.Add(ExcelRealizeHelp.SheetRepeat(this, name));
        return new ExcelSheetEPPlus(Book, sheet);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的工作表集合
    /// <summary>
    /// 获取封装的工作表集合，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ExcelWorksheets Sheets
        => Book.To<ExcelBookEPPlus>().ExcelPackage.Workbook.Worksheets;
    #endregion
    #endregion
}
