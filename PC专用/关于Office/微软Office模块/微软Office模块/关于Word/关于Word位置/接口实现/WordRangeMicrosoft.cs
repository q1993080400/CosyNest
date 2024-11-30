using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

using Shape = Microsoft.Office.Interop.Word.Shape;

namespace System.Office.Word;

/// <summary>
/// 这个类型是<see cref="IWordRange"/>的实现，
/// 可以视为一个Word文档的范围
/// </summary>
/// <param name="wordRange">封装的Word文档范围</param>
sealed class WordRangeMicrosoft(MSWordRange wordRange) : IWordRange
{
    #region 公开成员
    #region 封装的Word范围
    /// <summary>
    /// 获取封装的Word范围对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    public MSWordRange WordRange { get; } = wordRange;
    #endregion
    #region 范围开始
    public int Start => WordRange.Start;
    #endregion
    #region 范围结束
    public int End => WordRange.End;
    #endregion
    #region 获取范围内所有图片
    public IReadOnlyCollection<IWordImage> Images
    {
        get
        {
            var inlineShapes = WordRange.InlineShapes.OfType<InlineShape>().
                    Where(static x => x.Type is WdInlineShapeType.wdInlineShapePicture).
                    Select(static x => new WordInlineImageMicrosoft()).ToArray();
            var shape = WordRange.ShapeRange.OfType<Shape>().
                Where(static x => x.Type is MsoShapeType.msoPicture).
                Select(static x => new WordImageMicrosoft(x)).ToArray();
            var array = inlineShapes.Concat<IWordImage>(shape).ToArray();
            return array;
        }
    }
    #endregion
    #region 范围的长度
    public int Length
        => WordRange.End - WordRange.Start;
    #endregion
    #region 范围的文本
    public string Text
        => WordRange.Text;
    #endregion
    #region 返回子范围
    public IWordRange this[Range range]
    {
        get
        {
            var rangeStart = WordRange.Start;
            var (start, end) = range.GetStartAndEnd(Length);
            var newRange = Document.Range(rangeStart + start, rangeStart + end);
            return new WordRangeMicrosoft(newRange);
        }
    }
    #endregion
    #region 转换为精确位置
    public IWordRange PrecisePos(IWordDocument document)
    {
        var msDocument = document.GetMSDocument();
        return msDocument == Document ?
            this :
            throw new NotSupportedException($"这个范围和{nameof(document)}属于两个不同的Word文档，无法返回精确位置");
    }
    #endregion
    #endregion
    #region 内部成员
    #region 文档对象
    /// <summary>
    /// 获取这个Word范围所隶属的文档
    /// </summary>
    private Document Document
        => WordRange.Document;
    #endregion
    #endregion
}
