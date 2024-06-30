namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录表示一个单一的筛选目标
/// </summary>
public sealed record FilterTargetSingle : FilterTarget, IHasFilterDefaultValue
{
    #region 访问表达式
    /// <summary>
    /// 属性访问表达式，
    /// 通过它可以访问要查询或排序的属性
    /// </summary>
    public required FilterEntity PropertyAccess { get; init; }
    #endregion
    #region 是否存在自定义默认值
    public override bool HasDefaultValue
        => PropertyAccess.HasDefaultValue;
    #endregion
    #region 筛选标识
    public override string Identification
        => PropertyAccess.Name;
    #endregion
    #region 获取默认值
    public Obj? GetDefaultValue<Obj>()
        => PropertyAccess.DefaultValue is { } defaultValue ?
        defaultValue.To<Obj>() : default;
    #endregion
}
