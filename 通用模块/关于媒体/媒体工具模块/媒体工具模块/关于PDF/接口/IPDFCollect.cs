namespace System.Media.Drawing.PDF;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个PDF页面的集合
/// </summary>
public interface IPDFCollect : IList<IPDFPage>
{
    #region 获取PDF文档
    /// <summary>
    /// 获取集合所在的PDF文档
    /// </summary>
    IPDFDocument PDF { get; }
    #endregion
    #region 复制到另一个集合
    /// <summary>
    /// 将PDF集合中的页复制到另一个PDF集合
    /// </summary>
    /// <param name="target">复制的目标集合</param>
    /// <param name="range">该对象指示复制的范围，
    /// 如果为<see langword="null"/>，默认为全部复制</param>
    /// <param name="pos">指示将复制后的页面放在哪个位置，
    /// 如果为<see langword="null"/>，默认为放在最末尾</param>
    void CopyTo(IPDFCollect target, Range? range = null, Index? pos = null);
    #endregion
    #region 添加元素
    /// <summary>
    /// 添加一个空白的PDF页，并返回
    /// </summary>
    /// <returns></returns>
    IPDFPage Add();
    #endregion
}
