using System.MathFrancis;
using System.Office.Excel;
using System.Office.Excel.Realize;

using Microsoft.Office.Interop.Excel;

namespace System.Office;

/// <summary>
/// 这个类型是<see cref="ISheetPage"/>的实现，
/// 可以用来打印Excel工作表
/// </summary>
/// <param name="sheet">工作表接口对象</param>
/// <param name="worksheet">封装的Excel工作表对象</param>
sealed class ExcelPageSheetMicrosoft(IExcelSheet sheet, Worksheet worksheet) : ISheetPage
{
    #region 公开成员
    #region 打印页数
    public int Count
        => worksheet.PageSetup.Pages.Count;
    #endregion
    #region 打印到文件
    public (int PageCount, Task Wait) PrintFromPageToFile(string filePath, Range? page = null)
    {
        ToolIO.CreateFather(filePath);
        var (start, end) = (page ?? Range.All).GetStartAndEnd(Count);
        var printer = MicrosoftOfficeRealize.GetPrinterName(filePath);
        worksheet.PrintOutEx(From: start + 1, To: end + 1,
            PrintToFile: true, PrToFileName: filePath, ActivePrinter: printer);
        var pageCount = end - start + 1;
        return (pageCount, Task.Delay(350 * (pageCount + 1)));
    }
    #endregion
    #region 设置打印区域
    public ISizePos<int>? PrintRegional
    {
        get
        {
            var address = worksheet.PageSetup.PrintArea;
            return address.IsVoid() ? null : ExcelRealizeHelp.AddressToSizePos(address);
        }
        set => worksheet.PageSetup.PrintArea =
            value is null ? "" : ExcelRealizeHelp.GetAddress(value);
    }
    #endregion
    #region 打印区域单元格
    public IExcelCells? PrintRegionalRange
    {
        get
        {
            var address = worksheet.PageSetup.PrintArea;
            if (address is null)
                return null;
            var a1Address = worksheet.Application.ConvertAddress(address, false);
            return sheet[a1Address];
        }
    }
    #endregion
    #endregion
    #region 未实现的成员
    public (int PageCount, Task Wait) PrintFromRegional(ISizePos<int>? regional = null, int number = 1, string? printer = null)
    {
        throw new NotImplementedException();
    }

    public (int PageCount, Task Wait) PrintFromRegionalToFile(ISizePos<int>? regional, string filePath)
    {
        throw new NotImplementedException();
    }


    public (int PageCount, Task Wait) PrintFromPage(Range? page = null, int number = 1, string? printer = null)
    {
        throw new NotImplementedException();
    }
    #endregion 
}
