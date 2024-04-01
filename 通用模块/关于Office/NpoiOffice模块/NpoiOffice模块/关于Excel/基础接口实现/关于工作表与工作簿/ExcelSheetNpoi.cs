using System.Office.Excel.Realize;

using NPOI.SS.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型代表使用Npoi实现的Excel工作表
/// </summary>
sealed class ExcelSheetNpoi : ExcelSheet
{
    #region 封装的对象
    #region 工作簿
    /// <summary>
    /// 获取工作表所在的工作簿
    /// </summary>
    private IWorkbook BookNpoi => Sheet.Workbook;
    #endregion
    #region 工作表
    /// <summary>
    /// 获取封装的Excel工作表，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    public ISheet Sheet { get; }
    #endregion
    #endregion
    #region 关于工作表
    #region 返回工作表索引
    /// <summary>
    /// 返回工作表在工作簿中的索引
    /// </summary>
    private int Index
        => BookNpoi.GetSheetIndex(Sheet);
    #endregion
    #region 工作表名称
    public override string Name
    {
        get => Sheet.SheetName;
        set => BookNpoi.SetSheetName(Index, value);
    }
    #endregion
    #region 删除工作表
    public override void Delete()
        => BookNpoi.RemoveSheetAt(Index);
    #endregion
    #region 复制工作表
    public override IExcelSheet Copy(IExcelSheetManage? collection = null, Func<string, int, string>? renamed = null)
    {
        collection ??= Book.Sheets;
        if (collection is ExcelSheetCollectionNpoi sheets)
        {
            Sheet.CopyTo(sheets.ExcelBookNpoi, ExcelRealizeHelp.SheetRepeat(collection, Name, renamed), true, true);
            return collection[^1];
        }
        throw new Exception($"{collection}不是Npoi实现的工作表集合");
    }
    #endregion
    #endregion
    #region 关于单元格
    #region 用户区域
    public override IExcelCells Cell { get; }
    #endregion
    #region 根据行列号返回单元格
    public override IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1]
        => Cell[beginRow, beginColumn, endRow, endColumn];
    #endregion
    #region 获取行或列
    public override IExcelRC GetRC(int begin, int? end, bool isRow)
    {
        var e = end ?? begin;
        var rc = CreateCollection.RangeBE(begin, e);
        #region 用来枚举行的本地函数
        IEnumerable<IRow> Row()
            => rc.Select(x => Sheet.GetRow(x) ?? Sheet.CreateRow(x));
        #endregion
        return isRow ?
            new ExcelRowNpoi(this, begin, e, Row()) :
            new ExcelColumnNpoi(this, begin, e);
    }
    #endregion
    #endregion
    #region 返回页面对象
    public override IPageSheet Page => throw CreateException.NotSupported();
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="book">工作表所在的工作簿</param>
    /// <param name="sheet">封装的Npoi工作表</param>
    public ExcelSheetNpoi(IExcelBook book, ISheet sheet)
        : base(book)
    {
        Sheet = sheet;
        Cell = new ExcelCellUserNpoi(this);
    }
    #endregion
}
