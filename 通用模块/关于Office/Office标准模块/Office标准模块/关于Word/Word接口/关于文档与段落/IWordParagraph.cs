namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视作一个Word段落，
/// 处于同一段落的文本或其他对象共享对齐方式
/// </summary>
public interface IWordParagraph
{
    #region 说明文档
    /*段落可以包含文本，也可以包含图片等其他对象*/
    #endregion
    #region 段落所属的文档
    /// <summary>
    /// 返回段落所属的文档
    /// </summary>
    IWordDocument Document { get; }
    #endregion
    #region 关于段落位置，长度，排版
    #region 段落的开始
    /// <summary>
    /// 获取段落开始位置的书签
    /// </summary>
    IWordBookmark Begin { get; }
    #endregion
    #region 移动段落
    /// <summary>
    /// 将段落移动到新位置
    /// </summary>
    /// <param name="pos">段落的新位置，如果为<see langword="null"/>，
    /// 代表应该移动到文档的末尾</param>
    void Move(Index? pos);
    #endregion
    #region 段落的长度
    /// <summary>
    /// 返回段落的长度，也就是段落文本的字数
    /// </summary>
    int Length => this is IWordParagraphText t ? t.Text.Length : 1;

    /*实现本API请遵循以下规范：
      #如果这个段落不是文本，而是图片等其他对象，
      则无论它的大小，默认为占用1个位置，
      这是为了能够更方便的确定非文本段落的位置*/
    #endregion
    #region 段落的对齐样式
    /// <summary>
    /// 获取或设置段落的对齐样式
    /// </summary>
    OfficeAlignment Alignment { get; set; }
    #endregion
    #endregion
    #region 删除段落
    /// <summary>
    /// 删除这个段落
    /// </summary>
    void Delete();
    #endregion
}
