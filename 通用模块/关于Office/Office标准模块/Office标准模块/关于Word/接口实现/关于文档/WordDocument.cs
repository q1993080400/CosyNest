using System.IOFrancis;

namespace System.Office.Word.Realize;

/// <summary>
/// 在实现<see cref="IWordDocument"/>的时候，
/// 可以继承自本类型，以减少重复的工作
/// </summary>
/// <param name="path">文档所在的文件路径，
/// 如果文档不是通过文件创建的，则为<see langword="null"/></param>
public abstract class WordDocument(string? path) : FromIO(path), IWordDocument
{
    #region 返回页面对象
    public abstract IWordPages Pages { get; }
    #endregion
    #region 总页数
    public int PageCount
        => Pages.Count;
    #endregion
    #region 返回文档的主体部分
    public abstract IWordRange Content { get; }
    #endregion
    #region 从剪贴板粘贴
    public abstract IWordRange Paste(IWordPos pos);
    #endregion
    #region 图片管理对象
    public abstract IWordImageManage ImageManage { get; }
    #endregion
    #region 查找
    public abstract IReadOnlyCollection<IWordRange> Find(string condition);
    #endregion
}
