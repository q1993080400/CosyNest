namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录封装了被更改的实体属性的信息
/// </summary>
public sealed record ServerUpdatePropertyInfo
{
    #region 属性的名称
    /// <summary>
    /// 获取要修改的属性的名称
    /// </summary>
    public required string PropertyName { get; init; }
    #endregion
    #region 属性的新值
    /// <summary>
    /// 获取要修改的属性的新值
    /// </summary>
    public required object? Value { get; init; }
    #endregion
}
