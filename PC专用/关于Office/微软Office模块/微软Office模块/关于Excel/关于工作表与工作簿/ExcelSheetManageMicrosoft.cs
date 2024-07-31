using System.Office.Excel;
using System.Office.Excel.Realize;

using Microsoft.Office.Interop.Excel;

namespace System.Office;

/// <summary>
/// 这个类型是<see cref="IExcelSheetManage"/>的实现，
/// 可以视为一个Excel工作表管理对象
/// </summary>
/// <inheritdoc cref="ExcelSheetManage(IExcelBook)"/>
sealed class ExcelSheetManageMicrosoft(ExcelBookMicrosoft book) : ExcelSheetManage(book)
{
    #region 公开成员
    #region 工作表数量
    public override int Count
        => Sheets.Count;
    #endregion
    #region 枚举所有工作表
    public override IEnumerator<IExcelSheet> GetEnumerator()
    {
        foreach (Worksheet sheet in Sheets)
        {
            yield return PackSheet(sheet);
        }
    }
    #endregion
    #region 关于获取工作表
    #region 可能为null
    public override IExcelSheet? GetSheetOrNull(string name)
    {
        Worksheet? sheet = Sheets[name];
        return sheet is null ? null : PackSheet(sheet);
    }
    #endregion
    #region 不可为null
    #region 根据索引返回
    public override IExcelSheet this[int index]
    {
        get
        {
            Worksheet? sheet = Sheets[index];
            return sheet is null ?
                throw new IndexOutOfRangeException($"索引越界，不存在索引为{index}的工作表") :
                PackSheet(sheet);
        }
    }
    #endregion
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 工作表集合
    /// <summary>
    /// 获取封装的工作表集合对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Sheets Sheets
        => book.Workbook.Worksheets;
    #endregion
    #region 封装一个工作表
    /// <summary>
    /// 封装一个Excel工作表
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ExcelSheetMicrosoft(ExcelSheetManageMicrosoft, Worksheet)"/>
    private ExcelSheetMicrosoft PackSheet(Worksheet worksheet)
        => new(this, worksheet);
    #endregion
    #endregion
    #region 未实现的成员
    public override IExcelSheet Add(string name = "Sheet", Index? pos = null)
    {
        throw new NotImplementedException();
    }

    public override IExcelSheet Add(IExcelSheet sheet, Index? pos = null)
    {
        throw new NotImplementedException();
    }
    #endregion 
}
