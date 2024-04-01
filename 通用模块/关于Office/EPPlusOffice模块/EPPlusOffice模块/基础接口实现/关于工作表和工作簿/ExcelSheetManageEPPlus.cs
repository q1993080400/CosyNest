using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="ExcelSheetManage"/>的实现，
/// 可以视为一个底层使用EPPlus的Excel工作表集合
/// </summary>
/// <inheritdoc cref="ExcelSheetManage(IExcelBook)"/>
sealed class ExcelSheetManageEPPlus(IExcelBook excelBook) : ExcelSheetManage(excelBook)
{
    #region 公开成员
    #region 工作表数量
    public override int Count => EPPlusSheets.Count;
    #endregion
    #region 枚举工作表
    public override IEnumerator<IExcelSheet> GetEnumerator()
       => EPPlusSheets.Select(PackEPPlusSheet).GetEnumerator();
    #endregion
    #region 关于返回工作表
    #region 根据名称返回，不可能返回null
    public override IExcelSheet GetSheet(string name, bool createTable = false)
    {
        var sheet = GetSheetOrNull(name);
        return (sheet, createTable) switch
        {
            ({ } s, _) => s,
            (null, true) => throw new KeyNotFoundException($"没有找到名称为{name}的工作表"),
            (null, false) => Add()
        };
    }
    #endregion
    #region 根据名称返回，可能返回null
    public override IExcelSheet? GetSheetOrNull(string name)
    {
        var sheet = EPPlusSheets[name];
        return sheet is { } ? PackEPPlusSheet(sheet) : null;
    }
    #endregion
    #region 根据索引返回
    public override IExcelSheet GetSheet(int index)
    {
        var sheet = EPPlusSheets[index];
        return PackEPPlusSheet(sheet);
    }
    #endregion
    #endregion
    #region 添加工作表
    #region 插入空白表
    public override IExcelSheet Add(string name = "Sheet", Index? pos = null)
    {
        var newName = ExcelRealizeHelp.SheetRepeat(EPPlusSheets.Select(x => x.Name), name);
        var sheet = EPPlusSheets.Add(newName);
        var newSheet = PackEPPlusSheet(sheet);
        var index = (pos ?? Index.End).GetOffset(Count);
        newSheet.Index = index;
        return newSheet;
    }
    #endregion 
    #region 添加非空白表
    public override IExcelSheet Add(IExcelSheet sheet, Index? pos = null)
    {
        if (sheet is not ExcelSheetEPPlus { Sheet: { } epSheet })
            throw new NotSupportedException($"不能将非EPPlus工作表复制到EPPlus工作表集合中");
        var name = epSheet.Name;
        var newName = ExcelRealizeHelp.SheetRepeat(EPPlusSheets.Select(x => x.Name), name);
        var newEPPlusSheet = EPPlusSheets.Copy(name, newName);
        var newSheet = PackEPPlusSheet(newEPPlusSheet);
        var index = (pos ?? Index.End).GetOffset(Count);
        newSheet.Index = index;
        return newSheet;
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 工作表集合
    /// <summary>
    /// 获取封装的工作表集合，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ExcelWorksheets EPPlusSheets { get; }
    = excelBook.To<ExcelBookEPPlus>().ExcelWorkbook.Worksheets;
    #endregion
    #region 封装EPPlus工作表
    /// <summary>
    /// 将EPPlus工作表封装为工作表接口
    /// </summary>
    /// <param name="sheet">待封装的工作表</param>
    /// <returns></returns>
    private ExcelSheetEPPlus PackEPPlusSheet(ExcelWorksheet sheet)
        => new(Book, sheet);
    #endregion
    #endregion
}
