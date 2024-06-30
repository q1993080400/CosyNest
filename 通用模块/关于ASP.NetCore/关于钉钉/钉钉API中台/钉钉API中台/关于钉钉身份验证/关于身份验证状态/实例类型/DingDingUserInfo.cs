namespace System.DingDing;

/// <summary>
/// 这个记录表示一个钉钉用户的概括
/// </summary>
public sealed record DingDingUserInfo
{
    #region 用户的ID
    /// <summary>
    /// 获取用户的ID，
    /// 它指的是UserID，不是UnionID
    /// </summary>
    public required string UserID { get; init; }
    #endregion
    #region 当前用户的名称
    /// <summary>
    /// 获取当前用户的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
    #region 员工头像
    /// <summary>
    /// 获取员工头像的Uri
    /// </summary>
    public required string? AvatarUri { get; init; }
    #endregion
}
