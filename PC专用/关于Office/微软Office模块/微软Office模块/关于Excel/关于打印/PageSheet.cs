using System.IOFrancis.FileSystem;
using System.Maths.Plane;
using System.Office.Excel.Realize;
using System.Underlying;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个对象是<see cref="IPageSheet"/>的实现，
/// 可以视为一个Excel页面对象
/// </summary>
class PageSheet : IPageSheet
{
    #region 封装的对象
    #region 封装的工作表
    /// <summary>
    /// 获取封装的工作表，本对象的功能就是通过它实现的
    /// </summary>
    private Worksheet PackSheet { get; }
    #endregion
    #region 封装的APP对象
    /// <summary>
    /// 获取封装的工作表的APP对象
    /// </summary>
    private Application Application
        => PackSheet.Application;
    #endregion
    #endregion
    #region 获取或设置打印区域
    public ISizePosPixel? PrintRegional
    {
        get
        {
            var add = PackSheet.PageSetup.PrintArea;
            return add.IsVoid() ? null : ExcelRealizeHelp.AddressToSizePos(add);
        }
        set => PackSheet.PageSetup.PrintArea =
            value is null ? "" : ExcelRealizeHelp.GetAddress(value);
    }
    #endregion
    #region 返回页数
    public int PageCount
        => PackSheet.PageSetup.Pages.Count;
    #endregion
    #region 打印工作表
    #region 辅助方法
    #region 执行打印
    /// <summary>
    /// 执行打印操作
    /// </summary>
    /// <param name="from">打印的起始页数，从0开始，如果为<see langword="null"/>，代表通过打印区域确定打印范围</param>
    /// <param name="to">打印的结束页数，从0开始，代表通过打印区域确定打印范围</param>
    /// <param name="number">打印的份数</param>
    /// <param name="printer">指定的打印机名称，如果为<see langword="null"/>，代表不打印到文件</param>
    /// <param name="filePath">指定的打印到文件的路径，如果为<see langword="null"/>，代表不打印到纸张</param>
    private void Print(int? from = null, int? to = null, int number = 1, IPrinter? printer = null, PathText? filePath = null)
        => PackSheet.PrintOut
            (from is null ? 1 : from.Value + 1,
                to is null ? PageCount : to.Value + 1, number,
                ActivePrinter: MSOfficeRealize.PrinterName(Application.ActivePrinter, printer, filePath),
                PrintToFile: filePath == null ? null : true,
                PrToFileName: filePath?.Path);
    #endregion
    #region 按照页数打印的基础方法
    /// <summary>
    /// 按照页数打印的基本方法
    /// </summary>
    /// <param name="page">要打印的页数范围</param>
    /// <param name="number">要打印的份数</param>
    /// <param name="printer">打印机的名称</param>
    /// <param name="filePath">要打印到的文件路径</param>
    /// <returns>一个用于等待打印任务完成的<see cref="Task"/></returns>
    private Task PrintFromPageBase(Range? page = null, int number = 1, IPrinter? printer = null, PathText? filePath = null)
    {
        var activePrinter = Application.ActivePrinter;
        var (beg, end) = (page ?? Range.All).GetOffsetAndEnd(PageCount);
        Print(beg, end, number, printer, filePath);
        Application.ActivePrinter = activePrinter;                      //打印后还原活动打印机，不破坏原有设置
        return ExcelRealizeHelp.EstimatedPrintingTime(beg, end, number, printer is { });
    }
    #endregion
    #region 按照范围打印的基础方法
    /// <summary>
    /// 按照范围打印的基础方法
    /// </summary>
    /// <param name="regional">打印区域，
    /// 如果为<see langword="null"/>，则遵照<see cref="PrintRegional"/>属性设置的打印区域</param>
    /// <param name="filePath">指定打印的目标文件路径，
    /// 函数会根据该路径的扩展名自动判断应使用哪个打印机</param>
    /// <inheritdoc cref="PrintFromPageBase(Range?, int, IPrinter?, PathText?)"/>
    private Task PrintFromRegionalBase(ISizePosPixel? regional = null, int number = 1, IPrinter? printer = null, PathText? filePath = null)
    {
        var activePrinter = Application.ActivePrinter;
        if (regional is { })
        {
            var ps = PackSheet.PageSetup;
            var printArea = ps.PrintArea;
            ps.PrintArea = ExcelRealizeHelp.GetAddress(regional);
            Print(number: number, printer: printer, filePath: filePath);
            ps.PrintArea = printArea;                                          //打印后还原默认打印区域，不破坏原有设置
        }
        else Print(number: number, printer: printer, filePath: filePath);
        Application.ActivePrinter = activePrinter;                  //打印后还原活动打印机，不破坏原有设置
        return ExcelRealizeHelp.EstimatedPrintingTime(number, printer is { });
    }
    #endregion
    #endregion
    #region 按照页数打印
    #region 打印到文件
    public Task PrintFromPageToFile(PathText filePath, Range? page)
        => PrintFromPageBase(page, filePath: filePath);
    #endregion
    #region 打印到纸张
    public Task PrintFromPage(Range? page = null, int number = 1, IPrinter? printer = null)
    {
        ExceptionIntervalOut.Check(1, null, number);
        return PrintFromPageBase(page, number, printer);
    }
    #endregion
    #endregion
    #region 按照范围打印
    #region 打印到文件
    public Task PrintFromRegionalToFile(ISizePosPixel? regional, PathText filePath)
        => PrintFromRegionalBase(regional, filePath: filePath);
    #endregion
    #region 打印到纸张
    public Task PrintFromRegional(ISizePosPixel? regional = null, int number = 1, IPrinter? printer = null)
    {
        ExceptionIntervalOut.Check(1, null, number);
        return PrintFromRegionalBase(regional, number, printer);
    }
    #endregion
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的工作表封装进对象
    /// </summary>
    /// <param name="packSheet">指定的工作表</param>
    public PageSheet(Worksheet packSheet)
    {
        this.PackSheet = packSheet;
    }
    #endregion
}
