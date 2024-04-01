namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="SearchPanel"/>的参数
/// </summary>
public sealed record RenderSearchPanelInfo
{
    #region 搜索视图的状态
    /// <summary>
    /// 获取搜索视图的状态
    /// </summary>
    public required SearchViewerState SearchViewerState { get; init; }
    #endregion
    #region 渲染每个筛选条件的委托
    /// <summary>
    /// 用来渲染每个筛选条件的委托
    /// </summary>
    public required IEnumerable<RenderFragment> RenderCondition { get; init; }
    #endregion
    #region 渲染提交区域的委托
    /// <summary>
    /// 获取一个渲染提交区域的委托
    /// </summary>
    public required RenderFragment RenderSubmit { get; set; }
    #endregion
}
