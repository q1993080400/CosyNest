namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录可以用来执行钉钉身份验证的请求
/// </summary>
public sealed record AuthenticationDingDingRequest
{
    #region 身份验证代码
    /// <summary>
    /// 获取身份验证代码，
    /// 它可以是一个AuthCode，
    /// 也可以是一个Token
    /// </summary>
    public required string Code { get; init; }
    #endregion
    #region 刷新Token
    /// <summary>
    /// 获取用于刷新的Token，
    /// 如果为<see langword="null"/>，表示没有刷新Token
    /// </summary>
    public required string? RefreshToken { get; init; }
    #endregion
    #region 是否为Token
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示<see cref="Code"/>是一个Token，否则表示它是一个AuthCode
    /// </summary>
    public required bool IsToken { get; init; }
    #endregion
}
