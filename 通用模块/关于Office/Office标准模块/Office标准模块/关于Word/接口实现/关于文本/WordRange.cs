using System.Media.Drawing.Text;

namespace System.Office.Word.Realize;

/// <summary>
/// 在实现<see cref="IWordRange"/>的时候可以继承自本类型，
/// 以减少重复的工作
/// </summary>
/// <remarks>
/// 使用指定的文档，开始位置和结束位置初始化范围
/// </remarks>
/// <param name="document">范围所属的文档</param>
/// <param name="begin">范围的开始位置</param>
/// <param name="end">范围的结束位置</param>
public abstract class WordRange(IWordDocument document, IWordBookmark begin, IWordBookmark end) : IWordRange
{
    #region 范围所属的文档
    public IWordDocument Document { get; } = document;
    #endregion
    #region 范围的开始和结束
    public (IWordBookmark Begin, IWordBookmark End) Range { get; } = (begin, end);
    #endregion
    #region 获取或设置超链接
    public abstract string? Link { get; set; }
    #endregion
    #region 关于文本和格式
    #region 范围的文本
    public abstract string Text { get; set; }
    #endregion
    #region 范围的文本格式
    public abstract ITextStyleVar Style { get; set; }
    #endregion
    #endregion
    #region 重写ToString
    public override string ToString()
        => Text;

    #endregion
    #region 构造函数
    #endregion
}
