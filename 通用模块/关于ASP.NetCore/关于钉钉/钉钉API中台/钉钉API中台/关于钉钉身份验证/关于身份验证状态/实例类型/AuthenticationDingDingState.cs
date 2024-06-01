namespace System.DingDing;

/// <summary>
/// 这个记录是钉钉身份验证的状态
/// </summary>
public sealed record AuthenticationDingDingState
{
    #region 是否通过
    /// <summary>
    /// 获取身份验证是否通过
    /// </summary>
    public bool Passed => UserInfo is { };
    #endregion
    #region 对钉钉用户的概括
    /// <summary>
    /// 获取对钉钉用户的概括，
    /// 如果为<see langword="null"/>，表示身份验证未通过
    /// </summary>
    public required DingDingUserInfo? UserInfo { get; init; }
    #endregion
    #region 用于下一次请求的身份验证对象
    /// <summary>
    /// 获取用于下一次请求的身份验证对象，
    /// 必须将它写入Cookie，
    /// 如果为<see langword="null"，请忽略/>
    /// </summary>
    public required AuthenticationDingDingRequest? NextRequest { get; init; }
    #endregion
}
