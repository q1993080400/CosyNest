using System.Collections;

namespace System.Office.Excel.Realize;

/// <summary>
/// 在实现<see cref="IExcelSheetManage"/>时，
/// 可以继承自本类型，以减少重复的工作
/// </summary>
/// <remarks>
/// 将指定的工作簿封装进对象
/// </remarks>
/// <param name="book">指定的工作簿</param>
public abstract class ExcelSheetManage(IExcelBook book) : IExcelSheetManage
{
    #region 返回工作簿
    public IExcelBook Book { get; } = book;
    #endregion
    #region 枚举所有工作表
    public abstract IEnumerator<IExcelSheet> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 工作表的数量
    public abstract int Count { get; }
    #endregion
    #region 关于返回工作表
    #region 根据名称返回，不可能返回null
    public abstract IExcelSheet GetSheet(string name, bool createTable = false);
    #endregion
    #region 根据名称返回，可能返回null
    public abstract IExcelSheet? GetSheetOrNull(string name);
    #endregion
    #region 根据索引返回
    public abstract IExcelSheet GetSheet(int index);
    #endregion
    #endregion
    #region 关于添加工作表
    #region 添加空白表
    public abstract IExcelSheet Add(string name = "Sheet", Index? pos = null);
    #endregion
    #region 添加非空白表
    public abstract IExcelSheet Add(IExcelSheet sheet, Index? pos = null);
    #endregion
    #endregion 
}
