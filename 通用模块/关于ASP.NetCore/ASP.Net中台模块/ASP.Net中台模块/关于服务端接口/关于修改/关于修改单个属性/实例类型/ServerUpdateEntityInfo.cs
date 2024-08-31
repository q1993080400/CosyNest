namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录封装了对实体部分属性的更改
/// </summary>
public sealed record ServerUpdateEntityInfo
{
    #region 实体的ID
    /// <summary>
    /// 获取要修改的实体的ID
    /// </summary>
    public required Guid ID { get; init; }
    #endregion
    #region 属性更改信息
    /// <summary>
    /// 获取被更改的属性的信息
    /// </summary>
    public required IReadOnlyCollection<ServerUpdatePropertyInfo> UpdatePropertyInfo { get; init; }
    #endregion
}
