using System.Office.Excel.Realize;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="IExcelSheetCollection"/>的实现，
/// 可以视为一个工作表容器
/// </summary>
class ExcelSheetCollectionMS : ExcelSheetCollection
{
    #region 封装的对象
    /// <summary>
    /// 获取这个工作表集合所在的工作簿
    /// </summary>
    private new ExcelBookMicrosoft Book
        => (ExcelBookMicrosoft)base.Book;
    #endregion
    #region 关于工作表
    #region 关于枚举与返回工作表
    #region 枚举所有工作表
    public override IEnumerator<IExcelSheet> GetEnumerator()
        => Book.PackBook.Worksheets.OfType<Worksheet>().
            Select(x => new ExcelSheetMicrosoft(Book, x)).GetEnumerator();
    #endregion
    #region 根据索引返回工作表
    public override IExcelSheet this[int index]
    {
        get => new ExcelSheetMicrosoft(Book, Book.PackBook.Worksheets[index + 1]);
        set => base[index] = value;
    }
    #endregion
    #endregion
    #region 返回工作表数量
    public override int Count
        => Book.PackBook.Worksheets.Count;
    #endregion
    #region 对工作表的操作
    #region 添加工作表
    public override IExcelSheet Add(string? name)
    {
        var sheets = Book.PackBook.Sheets;
        var sheet = (Worksheet)sheets.Add(After: sheets.Last(), Type: XlSheetType.xlWorksheet);
        if (name is { })
            sheet.Name = ExcelRealizeHelp.SheetRepeat(this, name);
        return new ExcelSheetMicrosoft(Book, sheet);
    }
    #endregion
    #region 插入工作表
    public override void Insert(int index, IExcelSheet item)
    {
        var sheet = item.To<ExcelSheetMicrosoft>().PackSheet;
        var array = Book.PackBook.Sheets.OfType<_Worksheet>().ToArray();
        if (index < array.Length)
            sheet.Copy(array[index]);
        else sheet.Copy(After: array[^1]);     //如果插入索引大于等于工作表的数量，则将其放置到最后一个位置

    }
    #endregion
    #region 移除全部工作表
    public override void Clear()
    {                                               //此处实现遵循接口文档要求的规范，详情请到IExcelSheetCollection源文件查看
        var sheets = this.ToArray();
        foreach (var (e, i, _) in sheets.PackIndex())
        {
            e.Name = i.ToString();
        }
        Add("Sheet1");
        sheets.ForEach(x => x.Delete());
    }
    #endregion
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelSheetCollection(IExcelBook)"/>
    public ExcelSheetCollectionMS(IExcelBook book)
        : base(book)
    {

    }
    #endregion
}
