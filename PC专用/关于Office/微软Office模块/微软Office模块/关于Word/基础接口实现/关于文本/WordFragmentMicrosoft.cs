using WordRange = Microsoft.Office.Interop.Word.Range;

namespace System.Office.Word;

/// <summary>
/// 这个类型代表由微软COM组件实现的Word片段
/// </summary>
class WordFragmentMicrosoft : WordRangeMicrosoft, IWordFragment
{
    #region 片段所属的段落
    public IWordParagraphText Paragraph { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的段落，Word范围和开始位置初始化对象
    /// </summary>
    /// <param name="paragraph">这个片段所在的段落</param>
    /// <param name="range">这个片段所封装的Word范围</param>
    public WordFragmentMicrosoft(IWordParagraphText paragraph, WordRange range)
        : base((WordDocumentMicrosoft)paragraph.Document, range, false)
    {
        this.Paragraph = paragraph;
    }
    #endregion
}
