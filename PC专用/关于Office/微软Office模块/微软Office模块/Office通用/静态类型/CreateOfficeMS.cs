using System.Diagnostics;
using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Office.Excel;
using System.Office.Word;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

using ExcelApplication = Microsoft.Office.Interop.Excel.Application;

namespace System.Office;

/// <summary>
/// 这个静态类为创建通过COM组件实现的Office对象提供帮助
/// </summary>
public static class CreateOfficeMS
{
    #region 通用成员
    #region 获取正在运行的COM对象
#if DEBUG
    #region 说明文档
    /*问：为什么要有LoadFromActive这个方法？
      答：这是为了调试方便，有了这个方法以后，
      就可以一边打开Office客户端一边调试，直接看到程序的运行结果，
      但在生产环境下，需要用户打开Office反而会比较麻烦，
      因此在此时请勿使用本方法

      问：这个方法调用了大量Win32API，这是为什么？
      答：因为GetActiveObject本来是一个类型Marshal中的方法，
      但是在.net core中，这个方法没有被实现，
      因此作者复制了.net framework中该方法的源码作为替代，
      如果.net core的后续版本实现了该方法，请将其删除，
      如果没有实现，而且在其他项目中有调用这个方法的需求，
      请将其移动到专门操作COM的模块中并予以公开，以提高复用性*/
    #endregion
    #region 辅助方法
    [DllImport("ole32.dll", PreserveSig = false)]
    [ResourceExposure(ResourceScope.None)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);
    [DllImport("ole32.dll", PreserveSig = false)]
    [ResourceExposure(ResourceScope.None)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    private static extern void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);
    [DllImport("oleaut32.dll", PreserveSig = false)]
    [ResourceExposure(ResourceScope.None)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);
    #endregion
    #region 正式方法
    /// <summary>
    /// 通过COM对象的名称，获取正在运行的COM组件，
    /// 如果没有找到，则返回<see langword="null"/>
    /// </summary>
    /// <param name="progID">COM对象的名称</param>
    /// <returns></returns>
    [SecurityCritical]
    private static object? GetActiveObject(string progID)
    {
        Guid clsid;
        try
        {
            CLSIDFromProgIDEx(progID, out clsid);
        }
        catch (Exception)
        {
            CLSIDFromProgID(progID, out clsid);
        }
        try
        {
            GetActiveObject(ref clsid, IntPtr.Zero, out var obj);
            return obj;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /*说明文档
      #经测试，以管理员权限运行的应用无法获取普通权限的Office进程，反之亦然*/
    #endregion
#endif
    #endregion
    #endregion
    #region 杀死所有Excel和Word进程
    /// <summary>
    /// 杀死所有Excel和Word进程
    /// </summary>
    public static void KillOffice()
        => Process.GetProcesses().
        Where(x => x.ProcessName is "EXCEL" or "WINWORD").
        ForEach(x => x.Kill());
    #endregion
    #region 关于Excel
    #region 返回受支持的文件类型
    /// <summary>
    /// 返回受本模块支持的Excel文件类型
    /// </summary>
    public static IFileType SupportExcel { get; }
    = CreateIO.FileType("受微软Office模块支持的Excel文件类型",
        OfficeFileCom.Excel2003,
        OfficeFileCom.Excel2007,
        OfficeFileCom.Excel2007Macro);
    #endregion
    #region 创建Excel工作簿
    #region 基础方法
    /// <summary>
    /// 创建工作簿的基础方法
    /// </summary>
    /// <inheritdoc cref="ExcelBookMicrosoft(PathText?, ExcelApplication?)"/>
    private static IExcelBook ExcelBookBase(PathText? path, ExcelApplication? application)
         => path is null ?
        new ExcelBookMicrosoft(null, application) :
        Excel.Realize.ExcelBook.GetExcelsBook(path, x => new ExcelBookMicrosoft(x, application));
    #endregion
    #region 共用一个Excel进程
    /// <summary>
    /// 返回一个创建工作簿的工厂，通过它创建的所有工作簿共用一个Excel进程，
    /// 某些操作需要两个工作簿位于同一进程，
    /// 当通过该工厂创建的工作簿全部被释放时，Excel进程会被关闭
    /// </summary>
    /// <returns></returns>
    public static Func<PathText?, IExcelBook> ExcelBookFactory()
    {
        var app = new ExcelApplication();
        return x => ExcelBookBase(x, app);
    }
    #endregion
    #region 通过路径创建
    /// <summary>
    /// 通过指定的文件路径，创建一个Excel工作簿
    /// </summary>
    /// <inheritdoc cref="ExcelBookMicrosoft(PathText?, ExcelApplication?)"/>
    public static IExcelBook ExcelBook(PathText? path = null)
        => ExcelBookBase(path, null);
    #endregion
    #region 获取正在运行的工作簿
#if DEBUG
    #region 获取打开的所有工作簿
    /// <summary>
    /// 从已经打开的Excel进程中加载所有Excel对象，
    /// 这个方法主要的目的在于方便调试，请勿用于生产用途
    /// </summary>
    /// <returns></returns>
    public static IExcelBook[] ExcelBookActive()
        => GetActiveObject("Excel.Application") is ExcelApplication excel ?
            excel.Workbooks.OfType<Workbook>().Select(x => (IExcelBook)new ExcelBookMicrosoft(x)).ToArray() :
            Array.Empty<IExcelBook>();
    #endregion
    #region 获取打开的第一个工作簿
    /// <summary>
    /// 从已经打开的Excel进程中加载第一个Excel对象，
    /// 这个方法主要的目的在于方便调试，请勿用于生产用途
    /// </summary>
    /// <returns></returns>
    public static IExcelBook? ExcelBookActiveFirst()
        => ExcelBookActive().FirstOrDefault();
    #endregion
#endif
    #endregion
    #endregion
    #endregion
    #region 关于Word
    #region 返回受支持的文件类型
    /// <summary>
    /// 返回受本模块支持的Word文件类型
    /// </summary>
    public static IFileType SupportWord { get; }
    = CreateIO.FileType("受微软Office模块支持的Word文件类型",
        OfficeFileCom.Word2003,
        OfficeFileCom.Word2007,
        OfficeFileCom.Word2007Macro);
    #endregion
    #region 创建Word文档
    #region 通过路径创建
    /// <summary>
    /// 通过指定的文件路径，创建一个Word文档
    /// </summary>
    /// <param name="path">文档的文件路径，
    /// 如果为<see langword="null"/>，代表新建一个空白文档</param>
    /// <returns></returns>
    public static IWordDocument WordDocument(PathText? path = null)
        => path is null ?
        new WordDocumentMicrosoft() :
        Word.Realize.WordDocument.GetDocument(path, x => new WordDocumentMicrosoft(x));
    #endregion
    #region 获取正在运行的文档
#if DEBUG
    #region 获取打开的所有文档
    /// <summary>
    /// 从已经打开的Word进程中加载所有Word对象，
    /// 这个方法主要的目的在于方便调试，请勿用于生产用途
    /// </summary>
    /// <returns></returns>
    public static IWordDocument[] WordDocumentActive()
        => GetActiveObject("Word.Application") is Microsoft.Office.Interop.Word.Application word ?
            word.Documents.OfType<Document>().Select(x => (IWordDocument)new WordDocumentMicrosoft(x)).ToArray() :
            Array.Empty<IWordDocument>();
    #endregion
    #region 获取打开的第一个文档
    /// <summary>
    /// 从已经打开的Word进程中加载第一个Word对象，
    /// 这个方法主要的目的在于方便调试，请勿用于生产用途
    /// </summary>
    /// <returns></returns>
    public static IWordDocument? WordDocumentActiveFirst()
        => WordDocumentActive().FirstOrDefault();
    #endregion
#endif
    #endregion
    #endregion
    #endregion
}
