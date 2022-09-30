using System.Office.Excel.Realize;

using NPOI.SS.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是使用Npoi实现的Excel工作表集合
/// </summary>
sealed class ExcelSheetCollectionNpoi : ExcelSheetCollection
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
    #region 关于添加和删除工作表
    #region 插入工作表
    public override void Insert(int index, IExcelSheet item)
    {
        if (item is ExcelSheetNpoi)
        {
            var newSheet = (ExcelSheetNpoi)item.Copy(collection: this);
            ExcelBookNpoi.SetSheetOrder(newSheet.Name, index);
        }
        else throw new Exception($"{item}不是通过Npoi实现的工作表");
    }
    #endregion
    #region 添加工作表
    public override IExcelSheet Add(string name = "Sheet")
    {
        var sheet = ExcelBookNpoi.CreateSheet(ExcelRealizeHelp.SheetRepeat(this, name));
        return new ExcelSheetNpoi(Book, sheet);
    }
    #endregion
    #region 移除所有工作表
    public override void Clear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            ExcelBookNpoi.RemoveSheetAt(i);
        }
    }
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelSheetCollection(IExcelBook)"/>
    public ExcelSheetCollectionNpoi(IExcelBook excelBook)
        : base(excelBook)
    {
        if (!Sheets.Any())
            Add();
    }
    #endregion
}
