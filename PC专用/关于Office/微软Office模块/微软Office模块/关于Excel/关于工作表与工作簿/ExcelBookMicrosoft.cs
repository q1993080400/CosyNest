using System.Diagnostics;
using System.IOFrancis.FileSystem;
using System.Office.Excel.Realize;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个对象代表通过微软COM组件实现的Excel工作簿
/// </summary>
sealed partial class ExcelBookMicrosoft : ExcelBook, IExcelBook, IOfficeUpdate
{
    #region 公开成员
    #region 保存对象
    protected override Task SaveRealize(string path, bool isSitu)
    {
        if (isSitu)
            Workbook.Save();
        else
            Workbook.SaveAs(path);
        return Task.CompletedTask;
    }
    #endregion
    #region 释放对象
    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial int GetWindowThreadProcessId(IntPtr hwnd, out int processid);

    protected override ValueTask DisposeAsyncActualRealize()
    {
        var application = Application;
        GetWindowThreadProcessId(new IntPtr(application.Hwnd), out var pid);
        Workbook.Close(false);
        application.Quit();
        Process.GetProcessById(pid).Kill();
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 返回工作表管理对象
    public override IExcelSheetManage SheetManage { get; }
    #endregion
    #region 默认格式
    protected override string DefaultFormat
        => "xlsx";
    #endregion
    #region 升级Office文件
    public string Update()
    {
        var newPath = ToolPath.RefactoringPath(Path!, newExtension: static _ => "xlsx");
        Workbook.SaveAs(newPath, XlFileFormat.xlWorkbookDefault);
        return newPath;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 检查文件路径的扩展名
    protected override bool CheckExtensionName(string extensionName)
        => extensionName is "xls" or "xlsx";
    #endregion
    #region 封装的工作簿
    /// <summary>
    /// 获取封装的工作簿，本对象的功能就是通过它实现的
    /// </summary>
    internal Workbook Workbook { get; }
    #endregion
    #region 封装的Excel进程
    /// <summary>
    /// 获取封装的Excel进程，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Application Application
        => Workbook.Application;
    #endregion
    #endregion
    #region 未实现的成员
    public override IWorkBookPage Print => throw new NotImplementedException();

    public override bool AutoCalculation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    #endregion
    #region 构造函数：指定路径
    /// <summary>
    /// 通过指定的路径初始化Excel工作簿
    /// </summary>
    /// <param name="path">工作簿所在的路径，
    /// 如果为<see langword="null"/>或不存在，则创建一个新工作簿</param>
    public ExcelBookMicrosoft(string? path)
        : base(path)
    {
        var excel = new Application
        {
            Visible = false,
            DisplayAlerts = false,
            FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip
        };
        var books = excel.Workbooks;
        Workbook = path is null || !File.Exists(path) ?
            books.Add() : books.Open(path);
        SheetManage = new ExcelSheetManageMicrosoft(this);
    }
    #endregion
}
