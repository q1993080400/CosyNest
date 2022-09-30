using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 这个类型代表使用EPPlus实现的Excel工作簿
/// </summary>
sealed class ExcelBookEPPlus : ExcelBook, IExcelBook
{
    #region Excel封装包
    /// <summary>
    /// 获取Excel封装包对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    public ExcelPackage ExcelPackage { get; }
    #endregion
    #region 关于工作簿
    #region 释放工作簿
    protected override ValueTask DisposeAsyncActualRealize()
    {
        ExcelPackage.Dispose();
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 保存工作簿
    protected override async Task SaveRealize(string path, bool isSitu)
    {
        if (!Sheets.Any())
            return;
        if (isSitu)
        {
            await ExcelPackage.SaveAsync();
        }
        else await ExcelPackage.SaveAsAsync(new FileInfo(path));
    }
    #endregion
    #region 返回代表Office文件的流
    public IBitRead Read()
    {
        var stream = new MemoryStream();
        ExcelPackage.SaveAs(stream);
        return stream.ToBitPipe(Path is { } p ? ToolPath.SplitPathFile(p).Extended : "").Read;
    }
    #endregion
    #endregion
    #region 返回打印对象
    public override IOfficePrint Print => throw new NotImplementedException();
    #endregion
    #region 返回工作表集合
    public override IExcelSheetCollection Sheets { get; }
    #endregion
    #region 构造函数
    #region 使用流
    /// <summary>
    /// 使用指定的流初始化Excel工作簿
    /// </summary>
    /// <param name="stream">Excel工作簿的流</param>
    public ExcelBookEPPlus(Stream stream)
        : base(null, CreateOfficeEPPlus.SupportExcel)
    {
        ExcelPackage = new ExcelPackage(stream);
        Sheets = new ExcelSheetCollectionEPPlus(this);
    }
    #endregion
    #region 使用路径
    /// <summary>
    /// 使用指定的路径初始化Excel工作簿
    /// </summary>
    /// <param name="path">Excel工作簿的路径，
    /// 如果为<see langword="null"/>，则不从文件中加载，而是创建一个新的工作簿</param>
    public ExcelBookEPPlus(PathText? path)
        : base(path, CreateOfficeEPPlus.SupportExcel)
    {
        if (path is null || !File.Exists(path))
        {
            ExcelPackage = new();
            ExcelPackage.Workbook.Worksheets.Add("Sheet1");
        }
        else
            ExcelPackage = new(new FileInfo(path));
        Sheets = new ExcelSheetCollectionEPPlus(this);
    }
    #endregion
    #region 静态构造函数
    static ExcelBookEPPlus()
    {
        ExcelPackage.LicenseContext ??= LicenseContext.NonCommercial;
    }
    #endregion
    #endregion
}
