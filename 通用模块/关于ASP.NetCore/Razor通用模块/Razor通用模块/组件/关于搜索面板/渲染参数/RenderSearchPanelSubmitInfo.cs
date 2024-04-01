namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="SearchPanel"/>提交部分的参数
/// </summary>
public sealed record RenderSearchPanelSubmitInfo
{
    #region 提交搜索
    /// <summary>
    /// 这个委托可以用来提交搜索
    /// </summary>
    public required Func<Task> Submit { get; init; }
    #endregion
    #region 清除搜索
    /// <summary>
    /// 这个委托可以用来清除搜索
    /// </summary>
    public required Func<Task> Clear { get; init; }
    #endregion
}
