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
    public bool Passed => AuthenticationResult is { };
    #endregion
    #region 身份验证的结果
    /// <summary>
    /// 获取身份验证的结果，
    /// 如果为<see langword="null"/>，表示身份验证未通过
    /// </summary>
    public required AuthenticationDingDingResult? AuthenticationResult { get; init; }
    #endregion
    #region 钉钉用户的ID
    /// <summary>
    /// 获取钉钉用户的ID，
    /// 如果没有登陆成功，
    /// 则为<see langword="null"/>
    /// </summary>
    public string? UserID
        => AuthenticationResult?.UserInfo.UserID;
    #endregion
}
