namespace System.Office.Word;

/// <summary>
/// 这个接口代表包含文本的Word段落
/// </summary>
public interface IWordParagraphText : IWordParagraph
{
    #region 段落的文本
    /// <summary>
    /// 返回段落的无格式文本
    /// </summary>
    string Text { get; }
    #endregion
    #region 返回文本范围
    /// <summary>
    /// 返回这个段落的文本范围
    /// </summary>
    IWordRange Range
    {
        get
        {
            var beg = Begin.Pos;
            return Document[beg..(beg + Length - 1)];
        }
    }
    #endregion
    #region 枚举所有片段
    /// <summary>
    /// 获取一个枚举所有片段的枚举器
    /// </summary>
    IEnumerable<IWordFragment> Fragments { get; }

    /*实现本API请遵循以下规范：
      虽然换行符等不可见字符应该被计算位置，
      但是它们不应该出现在片段中，
      因为对它们而言文本格式毫无意义*/
    #endregion
}
