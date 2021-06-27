namespace System.Office.Word
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视为一个Word片段，
    /// 处于同一片段的文本共享相同的格式
    /// </summary>
    public interface IWordFragment : IWordRange
    {
        #region 片段所属的段落
        /// <summary>
        /// 获取这个片段所属的段落
        /// </summary>
        IWordParagraphText Paragraph { get; }
        #endregion
    }
}
