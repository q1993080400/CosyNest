namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个指定页数的Word文档位置
/// </summary>
public interface IWordPosPage : IWordPos
{
    #region 页索引
    /// <summary>
    /// 获取对应的页索引，从0开始
    /// </summary>
    int PageIndex { get; }
    #endregion
}
