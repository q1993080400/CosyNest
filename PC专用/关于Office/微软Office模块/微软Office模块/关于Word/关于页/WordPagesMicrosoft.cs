using System.Collections;
using System.Office.Word;

using Microsoft.Office.Interop.Word;

using Task = System.Threading.Tasks.Task;

namespace System.Office;

/// <summary>
/// 这个类型是<see cref="IOfficePage"/>的实现，
/// 可以用来管理Word文档的打印
/// </summary>
/// <param name="document">封装的Word文档对象</param>
sealed class WordPagesMicrosoft(Document document) : IWordPages
{
    #region 公开成员
    #region 打印页数
    public int Count
        => document.ComputeStatistics(WdStatistic.wdStatisticPages);
    #endregion
    #region 打印到文件
    public (int PageCount, Task Wait) PrintFromPageToFile(string filePath, Range? page = null)
    {
        ToolIO.CreateFather(filePath);
        var (start, end) = (page ?? Range.All).GetStartAndEnd(Count);
        var pageRange = $"{start + 1}-{end + 1}";
        var activePrinter = MicrosoftOfficeRealize.GetPrinterName(filePath);
        document.Application.ActivePrinter = activePrinter;
        document.PrintOut(Range: WdPrintOutRange.wdPrintRangeOfPages,
            Pages: pageRange, PrintToFile: true, OutputFileName: filePath);
        var pageCount = end - start + 1;
        return (pageCount, Task.Delay(350 * (pageCount + 1)));
    }
    #endregion
    #region 枚举所有页面
    public IEnumerator<IWordPage> GetEnumerator()
        => Pages.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 按索引获取页面
    public IWordPage this[int index]
        => Pages.ElementAt(index);
    #endregion
    #endregion
    #region 内部成员
    #region 返回所有页面
    /// <summary>
    /// 返回所有页面
    /// </summary>
    private IEnumerable<IWordPage> Pages
    {
        get
        {
            var maxIndex = Count - 1;
            for (var i = 0; i <= maxIndex; i++)
            {
                var start = document.GoTo(WdGoToItem.wdGoToPage, WdGoToDirection.wdGoToAbsolute, i + 1).Start;
                var end = i == maxIndex ?
                    document.Content.End :
                    document.GoTo(WdGoToItem.wdGoToPage, WdGoToDirection.wdGoToAbsolute, i + 2).Start - 1;
                var range = document.Range(start, end);
                yield return new WordPageMicrosoft(i, range);
            }
        }
    }
    #endregion
    #endregion
    #region 未实现的成员
    public (int PageCount, Task Wait) PrintFromPage(Range? page = null, int number = 1, string? printer = null)
    {
        throw new NotImplementedException();
    }
    #endregion
}
