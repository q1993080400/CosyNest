namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录描述了枚举的名称还有值
/// </summary>
public sealed record EnumItem
{
    #region 枚举的值
    /// <summary>
    /// 获取枚举的值
    /// </summary>
    public required string Value { get; init; }
    #endregion
    #region 枚举的描述
    /// <summary>
    /// 获取枚举的描述
    /// </summary>
    public required string Describe { get; init; }
    #endregion
}
