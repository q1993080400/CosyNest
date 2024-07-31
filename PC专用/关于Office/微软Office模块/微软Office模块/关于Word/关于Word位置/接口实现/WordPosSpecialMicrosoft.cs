using Microsoft.Office.Interop.Word;

namespace System.Office.Word;

/// <summary>
/// 这个类型是<see cref="IWordPosSpecial"/>的实现，
/// 可以视为一个专用于微软Word文档的特殊位置
/// </summary>
sealed class WordPosSpecialMicrosoft : IWordPosSpecial
{
    #region 获取特殊位置
    public required WordPosSpecialEnum Pos { get; init; }
    #endregion
    #region 转换为精确位置
    public IWordRange PrecisePos(IWordDocument document)
    {
        var msDocument = document.GetMSDocument();
        switch (Pos)
        {
            case WordPosSpecialEnum.NewPage:
                var selection = msDocument.Application.Selection;
                selection.GoTo(WdGoToItem.wdGoToPercent, WdGoToDirection.wdGoToLast);
                selection.InsertNewPage();
                var msRange = msDocument.GoTo(WdGoToItem.wdGoToPage, WdGoToDirection.wdGoToLast);
                return new WordRangeMicrosoft(msRange);
            case var pos:
                throw new NotSupportedException($"无法识别{pos}类型的位置");
        }
    }
    #endregion
}
