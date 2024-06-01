namespace System.DingDing;

/// <summary>
/// 这个记录是钉钉授权状态
/// </summary>
public sealed record AuthorizationDingDingState
{
    #region 身份验证状态
    /// <summary>
    /// 获取身份验证状态
    /// </summary>
    public required AuthenticationDingDingState AuthenticationState { get; init; }
    #endregion
    #region 是否授权通过
    /// <summary>
    /// 获取是否授权通过，允许显示这个页面
    /// </summary>
    public required bool AuthorizationPassed { get; init; }
    #endregion
}
