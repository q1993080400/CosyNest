using System.MathFrancis.Plane;
using System.Office.Excel;
using System.Office.Excel.Realize;
using System.Underlying;

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
    public void PrintFromPageToFile(string filePath, Range? page = null)
    {
        var (start, end) = (page ?? Range.All).GetStartAndEnd(Count);
        var printer = MicrosoftOfficeRealize.GetPrinterName(filePath);
        worksheet.PrintOutEx(From: start + 1, To: end + 1,
            PrintToFile: true, PrToFileName: filePath, ActivePrinter: printer);
    }
    #endregion
    #region 设置打印区域
    public ISizePosPixel? PrintRegional
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

    public int PrintFromRegional(ISizePosPixel? regional = null, int number = 1, IPrinter? printer = null)
    {
        throw new NotImplementedException();
    }

    public void PrintFromRegionalToFile(ISizePosPixel? regional, string filePath)
    {
        throw new NotImplementedException();
    }


    public int PrintFromPage(Range? page = null, int number = 1, IPrinter? printer = null)
    {
        throw new NotImplementedException();
    }
    #endregion 
}
