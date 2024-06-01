namespace System.Office.Word.Realize;

/// <summary>
/// 这个类型是对<see cref=" IWordParagraph"/>的实现，
/// 表示一个不可变的Word段落
/// </summary>
public abstract class WordParagraph : IWordParagraph
{
    #region 返回对象的接口形式
    /// <summary>
    /// 返回这个对象的接口形式，
    /// 通过它可以访问一些显式实现的成员
    /// </summary>
    protected IWordParagraph Interface => this;
    #endregion
    #region 段落所属的文档
    public IWordDocument Document { get; }
    #endregion
    #region 关于段落位置，长度，排版
    #region 段落的开始
    public IWordBookmark Begin { get; }
    #endregion
    #region 移动段落
    public abstract void Move(Index? pos);
    #endregion
    #region 段落的对齐样式
    public abstract OfficeAlignment Alignment { get; set; }
    #endregion
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => this is IWordParagraphText t ? t.Text : base.ToString()!;
    #endregion
    #region 删除段落
    #region 正式方法
    public void Delete()
    {
        DeleteRealize();
        var length = Interface.Length;
        Document.To<WordDocument>().CallLengthChange(Begin.Pos..length, -length);
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// 删除段落的实际逻辑在这个方法中实现
    /// </summary>
    protected abstract void DeleteRealize();
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的文档和开始位置初始化对象
    /// </summary>
    /// <param name="document">段落所属的文档</param>
    /// <param name="begin">段落的开始位置</param>
    public WordParagraph(IWordDocument document, int begin)
    {
        Document = document;
        Begin = Document.GetBookmark(begin);
    }
    #endregion
}
