using System.IOFrancis.FileSystem;
using System.Office.Excel.Realize;
using System.Underlying;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="IOfficePrint"/>的实现，
/// 可以用来打印Excel工作簿
/// </summary>
class WorkBookPrint : IOfficePrint
{
    #region 封装的工作簿
    /// <summary>
    /// 获取封装的工作簿，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Workbook PackBook { get; }
    #endregion
    #region 返回页面数量
    public int PageCount
        => PackBook.Worksheets.OfType<Worksheet>().
        Sum(x => x.PageSetup.Pages.Count);
    #endregion
    #region 打印工作簿
    #region 基础方法
    /// <summary>
    /// 打印工作簿的基础方法
    /// </summary>
    /// <param name="page">打印的页码范围</param>
    /// <param name="number">打印的份数</param>
    /// <param name="printer">执行打印的打印机</param>
    /// <param name="filePath">打印到文件的路径</param>
    /// <returns></returns>
    private Task PrintBase(Range? page = null, int number = 1, IPrinter? printer = null, PathText? filePath = null)
    {
        var application = PackBook.Application;
        var (beg, end) = (page ?? Range.All).GetOffsetAndEnd(PageCount);
        var activePrinter = application.ActivePrinter;
        PackBook.PrintOut
             (beg + 1, end + 1, number,
             ActivePrinter: MSOfficeRealize.PrinterName(activePrinter, printer, filePath),
             PrintToFile: filePath == null ? null : true,
             PrToFileName: filePath?.Path);
        application.ActivePrinter = activePrinter;      //还原默认打印机，不破坏设置
        return ExcelRealizeHelp.EstimatedPrintingTime(beg, end, number, printer is { });
    }
    #endregion
    #region 打印到纸张
    public Task PrintFromPage(Range? page = null, int number = 1, IPrinter? printer = null)
    {
        ExceptionIntervalOut.Check(1, null, number);
        return PrintBase(page, number, printer, null);
    }
    #endregion
    #region 打印到文件
    public Task PrintFromPageToFile(PathText filePath, Range? page)
        => PrintBase(page, filePath: filePath);
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的工作簿封装进对象
    /// </summary>
    /// <param name="packBook">指定的工作簿</param>
    public WorkBookPrint(Workbook packBook)
    {
        this.PackBook = packBook;
    }
    #endregion
}
