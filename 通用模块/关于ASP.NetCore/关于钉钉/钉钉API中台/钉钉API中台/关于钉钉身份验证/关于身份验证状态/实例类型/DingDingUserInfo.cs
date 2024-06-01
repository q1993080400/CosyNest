namespace System.DingDing;

/// <summary>
/// 这个记录表示一个钉钉用户的概括
/// </summary>
public sealed record DingDingUserInfo
{
    #region 用户的ID
    /// <summary>
    /// 获取用户的ID
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region 是否为UnionID
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示<see cref="ID"/>为UnionID，否则为UserID
    /// </summary>
    public required bool IsUnionID { get; init; }
    #endregion
    #region 当前用户的名称
    /// <summary>
    /// 获取当前用户的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
}
