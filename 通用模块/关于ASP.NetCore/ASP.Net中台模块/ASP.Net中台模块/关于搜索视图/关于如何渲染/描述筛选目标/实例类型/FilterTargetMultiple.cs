namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录表示一个复合的筛选目标
/// </summary>
public sealed record FilterTargetMultiple : FilterTarget
{
    #region 开始属性访问表达式
    /// <summary>
    /// 获取作为区间开始的属性访问表达式
    /// </summary>
    public required FilterEntity PropertyAccessStart { get; init; }
    #endregion
    #region 结束属性访问表达式
    /// <summary>
    /// 获取作为区间结束的属性访问表达式
    /// </summary>
    public required FilterEntity PropertyAccessEnd { get; init; }
    #endregion
    #region 是否存在自定义默认值
    public override bool HasDefaultValue
        => PropertyAccessStart.HasDefaultValue || PropertyAccessEnd.HasDefaultValue;
    #endregion
    #region 筛选标识
    public override string Identification
        => PropertyAccessStart.Name;
    #endregion
}
