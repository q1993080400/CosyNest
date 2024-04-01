namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是搜索视图单个搜索和排序条件的渲染参数
/// </summary>
public sealed record RenderSearchPanelPropertyInfo
{
    #region 搜索视图的状态
    /// <summary>
    /// 获取搜索视图的状态
    /// </summary>
    public required SearchViewerState SearchViewerState { get; init; }
    #endregion
    #region 渲染条件组
    /// <summary>
    /// 获取渲染条件组，它描述如何渲染单个属性
    /// </summary>
    public required RenderConditionGroup RenderConditionGroup { get; init; }
    #endregion
    #region 提交搜索
    /// <summary>
    /// 这个委托可以用来提交搜索，
    /// 它通常会被用来排序上面
    /// </summary>
    public required Func<Task> Submit { get; init; }
    #endregion
}
