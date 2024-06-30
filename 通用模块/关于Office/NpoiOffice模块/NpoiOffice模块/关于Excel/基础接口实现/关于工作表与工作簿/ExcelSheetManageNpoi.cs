using System.Office.Excel.Realize;

using NPOI.SS.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是使用Npoi实现的Excel工作表集合
/// </summary>
sealed class ExcelSheetManageNpoi : ExcelSheetManage
{
    #region 返回工作簿
    /// <summary>
    /// 返回本工作表集合所在的工作簿，
    /// 它以Npoi格式呈现
    /// </summary>
    public IWorkbook ExcelBookNpoi
        => ((ExcelBookNpoi)Book).WorkBook;
    #endregion
    #region 枚举所有工作表
    #region Npoi格式
    /// <summary>
    /// 以<see cref="ISheet"/>的形式枚举所有工作表
    /// </summary>
    internal IEnumerable<ISheet> Sheets
    {
        get
        {
            using var enumerator = ExcelBookNpoi.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }

    /*不要吐槽这个函数很傻逼，
      这是因为IWorkbook确实没有实现IEnumerable啊*/
    #endregion
    #region 自定义格式
    public override IEnumerator<IExcelSheet> GetEnumerator()
        => Sheets.Select(x => new ExcelSheetNpoi(Book, x)).GetEnumerator();
    #endregion
    #endregion
    #region 返回工作表的数量
    public override int Count
        => Sheets.Count();
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelSheetManage(IExcelBook)"/>
    public ExcelSheetManageNpoi(IExcelBook excelBook)
        : base(excelBook)
    {
        if (!Sheets.Any())
            Add();
    }
    #endregion
    #region 未实现的成员
    public override IExcelSheet GetSheet(string name, bool createTable = false)
    {
        throw new NotImplementedException();
    }

    public override IExcelSheet? GetSheetOrNull(string name)
    {
        throw new NotImplementedException();
    }

    public override IExcelSheet GetSheet(int index)
    {
        throw new NotImplementedException();
    }

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
