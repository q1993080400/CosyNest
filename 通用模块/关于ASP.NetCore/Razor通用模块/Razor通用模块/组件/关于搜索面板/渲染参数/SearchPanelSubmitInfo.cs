namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来提交搜索视图的参数
/// </summary>
public sealed record SearchPanelSubmitInfo
{
    #region 筛选条件
    /// <summary>
    /// 获取新生效的筛选条件
    /// </summary>
    public required DataFilterDescription DataFilterDescription { get; init; }
    #endregion
}
