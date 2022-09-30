namespace System.Media.Drawing.PDF;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为PDF文档中的一页
/// </summary>
public interface IPDFPage
{
    #region 获取PDF文档
    /// <summary>
    /// 获取页所在的PDF文档
    /// </summary>
    IPDFDocument PDF { get; }
    #endregion
}
