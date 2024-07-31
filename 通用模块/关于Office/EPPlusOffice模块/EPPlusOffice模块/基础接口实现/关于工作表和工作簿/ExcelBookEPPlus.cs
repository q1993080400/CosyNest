using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 这个类型代表使用EPPlus实现的Excel工作簿
/// </summary>
sealed class ExcelBookEPPlus : ExcelBook, IExcelBook
{
    #region 公开成员
    #region 返回打印对象
    public override IWorkBookPage Print => throw new NotImplementedException();
    #endregion
    #region 返回工作表的容器
    public override IExcelSheetManage SheetManage { get; }
    #endregion
    #region 是否开启自动计算
    public override bool AutoCalculation
    {
        get => ExcelWorkbook.CalcMode is ExcelCalcMode.Automatic;
        set => ExcelWorkbook.CalcMode = value ? ExcelCalcMode.Automatic : ExcelCalcMode.Manual;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 检查文件路径的扩展名
    protected override bool CheckExtensionName(string extensionName)
        => extensionName is "xlsx";
    #endregion
    #region Excel封装包
    /// <summary>
    /// 获取Excel封装包对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ExcelPackage ExcelPackage { get; }
    #endregion
    #region Excel工作簿
    /// <summary>
    /// 获取Excel工作簿对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ExcelWorkbook ExcelWorkbook => ExcelPackage.Workbook;
    #endregion
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
        if (SheetManage.Count == 0)
            return;
        if (isSitu)
        {
            await ExcelPackage.SaveAsync();
        }
        else await ExcelPackage.SaveAsAsync(new FileInfo(path));
    }
    #endregion
    #region 默认格式
    protected override string DefaultFormat => "xlsx";
    #endregion
    #endregion
    #region 构造函数
    #region 使用流
    /// <summary>
    /// 使用指定的流初始化Excel工作簿
    /// </summary>
    /// <param name="stream">Excel工作簿的流</param>
    public ExcelBookEPPlus(Stream stream)
        : base(null)
    {
        ExcelPackage = new ExcelPackage(stream);
        SheetManage = new ExcelSheetManageEPPlus(this);
    }
    #endregion
    #region 使用路径
    /// <summary>
    /// 使用指定的路径初始化Excel工作簿
    /// </summary>
    /// <param name="path">Excel工作簿的路径，
    /// 如果为<see langword="null"/>，则不从文件中加载，而是创建一个新的工作簿</param>
    public ExcelBookEPPlus(string? path)
        : base(path)
    {
        ExcelPackage = path is null || !File.Exists(path) ?
            new() : new(new FileInfo(path));
        SheetManage = new ExcelSheetManageEPPlus(this);
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
