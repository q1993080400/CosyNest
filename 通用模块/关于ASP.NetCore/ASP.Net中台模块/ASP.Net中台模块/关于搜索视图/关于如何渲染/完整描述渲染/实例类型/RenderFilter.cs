namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录封装了筛选目标和动作
/// </summary>
/// <typeparam name="Target">筛选目标的类型</typeparam>
/// <typeparam name="Action">筛选动作的类型</typeparam>
public sealed record RenderFilter<Target, Action>
    where Target : FilterTarget
    where Action : FilterAction
{
    #region 筛选目标
    /// <summary>
    /// 获取筛选目标
    /// </summary>
    public required Target FilterTarget { get; init; }
    #endregion
    #region 筛选动作
    /// <summary>
    /// 获取筛选动作
    /// </summary>
    public required Action FilterAction { get; init; }
    #endregion
}
