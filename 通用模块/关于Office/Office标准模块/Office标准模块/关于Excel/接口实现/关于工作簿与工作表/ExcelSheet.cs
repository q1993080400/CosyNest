namespace System.Office.Excel.Realize;

/// <summary>
///  在实现<see cref="IExcelSheet"/>时，可以继承自本类型，
///  以减少重复的操作
/// </summary>
/// <param name="sheetManage">工作表管理对象</param>
public abstract class ExcelSheet(IExcelSheetManage sheetManage) : IExcelSheet
{
    #region 关于单元格
    #region 返回工作表的所有单元格
    public abstract IExcelCells Cell { get; }
    #endregion
    #region 返回行或者列
    public abstract IExcelRC GetRC(bool isRow, int begin, int? end);
    #endregion
    #region 根据行列号获取单元格
    public abstract IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1] { get; }
    #endregion
    #region 搜索单元格
    public IEnumerable<IExcelCells> Find(string content, bool findValue = true)
        => Cell.Find(content, findValue);
    #endregion
    #region 替换单元格
    public void Replace(string content, string replace)
        => Cell.Replace(content, replace);
    #endregion
    #endregion
    #region 关于工作簿与工作表
    #region 返回工作表管理对象
    public IExcelSheetManage SheetManage { get; } = sheetManage;
    #endregion
    #region 返回工作表所在的工作薄
    public IExcelBook Book => SheetManage.Book;
    #endregion
    #region 获取或设置工作表的名称
    public abstract string Name { get; set; }
    #endregion
    #region 工作表的索引
    public abstract int Index { get; set; }
    #endregion
    #region 对工作表的操作
    #region 删除工作表
    public abstract void Delete();
    #endregion
    #region 复制工作表
    public abstract IExcelSheet Copy(IExcelSheetManage? collection = null, Func<string, int, string>? renamed = null);
    #endregion
    #endregion
    #region 返回页面
    public abstract ISheetPage Page { get; }
    #endregion
    #endregion
    #region 关于Excel对象
    #region 返回用来管理图表的对象
    public abstract IOfficeObjectManageCommon<IOfficeChart> ChartManage { get; }
    #endregion
    #endregion
    #region 重写的方法
    #region 重写GetHashCode
    public override int GetHashCode()
        => ToolEqual.CreateHash(Name, Book);
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is IExcelSheet { Book: var book, Name: var name } &&
        book == Book && name == Name;
    #endregion
    #region 重写ToString方法
    public override string ToString()
        => Name;
    #endregion
    #endregion
}
