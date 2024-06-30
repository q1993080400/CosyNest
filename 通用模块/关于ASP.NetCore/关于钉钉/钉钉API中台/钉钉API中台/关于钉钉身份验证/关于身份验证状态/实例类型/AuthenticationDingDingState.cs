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
}
