namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是搜索视图的渲染参数
/// </summary>
public sealed record RenderSearchViewerInfo
{
    #region 如何渲染查询和排序条件
    /// <summary>
    /// 这个记录描述了如何渲染查询和排序条件
    /// </summary>
    public required IEnumerable<RenderFilterGroup> RenderAllFilterCondition { get; init; }
    #endregion
    #region 筛选视图状态
    /// <summary>
    /// 获取筛选视图的状态
    /// </summary>
    public SearchViewerState SearchViewerState { get; init; } = new();
    #endregion
}
