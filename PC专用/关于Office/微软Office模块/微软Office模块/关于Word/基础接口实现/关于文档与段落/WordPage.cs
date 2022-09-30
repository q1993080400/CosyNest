using System.IOFrancis.FileSystem;
using System.Office.Excel.Realize;
using System.Underlying;

using Microsoft.Office.Interop.Word;

using Task = System.Threading.Tasks.Task;

namespace System.Office.Word;

/// <summary>
/// 这个类型是<see cref="IPage"/>的实现，
/// 可以管理Word文档的页面设置和打印
/// </summary>
class WordPage : IPage
{
    #region 封装的Word文档
    /// <summary>
    /// 获取封装的Word文档对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Document PackDocument { get; }
    #endregion
    #region 返回页数
    public int PageCount
        => PackDocument.ComputeStatistics(WdStatistic.wdStatisticPages);
    #endregion
    #region 打印文档
    #region 基础方法
    /// <summary>
    /// 打印工作簿的基本方法
    /// </summary>
    /// <param name="page">要打印的页码范围</param>
    /// <param name="number">要打印的份数</param>
    /// <param name="printer">执行打印的打印机</param>
    /// <param name="filePath">如果要打印到文件，则指定这个参数以指定输出路径</param>
    /// <returns></returns>
    private Task PrintBase(Range? page, int number = 1, IPrinter? printer = null, PathText? filePath = null)
    {
        var (beg, end) = (page ?? Range.All).GetOffsetAndEnd(PageCount);
        var application = PackDocument.Application;
        var activePrinter = application.ActivePrinter;
        application.ActivePrinter = MSOfficeRealize.PrinterName(activePrinter, printer, filePath);
        var pageRange = $"{beg + 1}-{end + 1}";
        if (filePath is null)
            PackDocument.PrintOut
                (Range: WdPrintOutRange.wdPrintRangeOfPages,
                Pages: pageRange, Copies: number);
        else PackDocument.PrintOut
            (Range: WdPrintOutRange.wdPrintRangeOfPages,
            Pages: pageRange, Copies: number,
            PrintToFile: true,
            OutputFileName: filePath.Path);
        application.ActivePrinter = activePrinter;      //还原默认打印机设置，不破坏原有设置
        return ExcelRealizeHelp.EstimatedPrintingTime(beg, end, number, printer is { });
    }
    #endregion
    #region 打印到纸张
    public Task PrintFromPage(Range? page = null, int number = 1, IPrinter? printer = null)
    {
        ExceptionIntervalOut.Check(1, null, number);
        return PrintBase(page, number, printer);
    }
    #endregion
    #region 打印到文件
    public Task PrintFromPageToFile(PathText filePath, Range? page)
        => PrintBase(page, filePath: filePath);
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的Word文档封装进对象
    /// </summary>
    /// <param name="packDocument">被封装的Word文档</param>
    public WordPage(Document packDocument)
    {
        this.PackDocument = packDocument;
    }
    #endregion
}
