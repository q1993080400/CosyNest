namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录表示一个钉钉用户的概括
/// </summary>
public sealed record DingDingUserInfo
{
    #region 当前用户的名称
    /// <summary>
    /// 获取当前用户的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
}
