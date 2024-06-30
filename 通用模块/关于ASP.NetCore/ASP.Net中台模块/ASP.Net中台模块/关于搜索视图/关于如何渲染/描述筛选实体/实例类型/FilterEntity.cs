namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录描述要筛选的实体
/// </summary>
public sealed record FilterEntity : IHasFilterDefaultValue
{
    #region 实体名称
    /// <summary>
    /// 描述要筛选的实体名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
    #region 默认值字面量
    /// <summary>
    /// 获取默认值的字面量
    /// </summary>
    public required string? DefaultValue { get; init; }
    #endregion
    #region 获取默认值
    public Obj? GetDefaultValue<Obj>()
        => DefaultValue is { } defaultValue ?
        defaultValue.To<Obj>() : default;
    #endregion
    #region 获取是否具有默认值
    /// <summary>
    /// 获取是否含有默认值
    /// </summary>
    public bool HasDefaultValue => DefaultValue is { };
    #endregion
}
