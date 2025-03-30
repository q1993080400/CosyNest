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
    #region 渲染所有筛选条件
    /// <summary>
    /// 返回一个委托，
    /// 它一次性渲染所有筛选条件
    /// </summary>
    public RenderFragment RenderConditionAll
        => RenderCondition.MergeRender();
    #endregion
    #region 用来渲染提交区域的参数
    /// <summary>
    /// 获取用来渲染提交区域的参数，
    /// 它可以用于自定义渲染
    /// </summary>
    public required RenderSearchPanelSubmitInfo RenderSubmitInfo { get; init; }
    #endregion
    #region 渲染提交区域的委托
    /// <summary>
    /// 获取一个渲染提交区域的委托
    /// </summary>
    public required RenderFragment RenderSubmit { get; set; }
    #endregion
}
