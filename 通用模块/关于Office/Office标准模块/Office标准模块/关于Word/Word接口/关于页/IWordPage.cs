namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Word页面
/// </summary>
public interface IWordPage
{
    #region 页面的范围
    /// <summary>
    /// 获取页面的范围
    /// </summary>
    IWordRange Range { get; }
    #endregion
    #region 页面索引
    /// <summary>
    /// 获取页面的索引，从0开始
    /// </summary>
    int PageIndex { get; }
    #endregion
}
