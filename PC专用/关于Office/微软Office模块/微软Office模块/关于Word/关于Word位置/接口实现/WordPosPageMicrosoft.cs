using Microsoft.Office.Interop.Word;

namespace System.Office.Word;

/// <summary>
/// 这个类型表示一个指定页数的Word文档位置
/// </summary>
sealed class WordPosPageMicrosoft : IWordPosPage
{
    #region 页的索引
    public required int PageIndex { get; init; }
    #endregion
    #region 转换为精确位置
    public IWordRange PrecisePos(IWordDocument document)
    {
        var msDocument = document.GetMSDocument();
        var msRange = msDocument.GoTo(WdGoToItem.wdGoToPage, WdGoToDirection.wdGoToAbsolute, PageIndex + 1);
        return new WordRangeMicrosoft(msRange);
    }
    #endregion
}
