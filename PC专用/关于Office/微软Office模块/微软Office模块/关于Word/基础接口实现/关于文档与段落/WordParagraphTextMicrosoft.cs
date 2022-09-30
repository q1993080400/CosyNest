using System.Office.Word.Realize;

using Microsoft.Office.Interop.Word;

namespace System.Office.Word;

/// <summary>
/// 这个类型代表由微软COM组件实现的Word段落
/// </summary>
class WordParagraphTextMicrosoft : WordParagraph, IWordParagraphText
{
    #region 被封装的段落
    /// <summary>
    /// 获取被封装的Word段落，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal Paragraph PackParagraph { get; }
    #endregion
    #region 关于段落的内容
    #region 枚举所有片段
    public IEnumerable<IWordFragment> Fragments
        => PackParagraph.Range.SplitFromStyle().
        Select(x => new WordFragmentMicrosoft(this, x));
    #endregion
    #region 段落的文本
    public string Text
        => PackParagraph.Range.Text;
    #endregion
    #endregion
    #region 关于段落位置，长度，排版
    #region 段落的对齐样式
    public override OfficeAlignment Alignment
    {
        get => MicrosoftWordRealize.MapAlignment.
            TryBMapA(PackParagraph.Alignment, OfficeAlignment.Unknown).Value;
        set => PackParagraph.Alignment = MicrosoftWordRealize.MapAlignment.AMapB(value);
    }
    #endregion
    #region 移动段落
    public override void Move(Index? pos)
        => throw new NotImplementedException("本API尚未实现");
    #endregion
    #endregion
    #region 删除段落
    protected override void DeleteRealize()
        => PackParagraph.Range.Text = null;
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的对象初始化Word段落
    /// </summary>
    /// <param name="document">这个段落所隶属的文档</param>
    /// <param name="packParagraph">被封装的段落</param>
    public WordParagraphTextMicrosoft(WordDocument document, Paragraph packParagraph)
        : base(document, document.FromUnderlying(packParagraph.Range.Start, true))
    {
        this.PackParagraph = packParagraph;
    }
    #endregion
}
