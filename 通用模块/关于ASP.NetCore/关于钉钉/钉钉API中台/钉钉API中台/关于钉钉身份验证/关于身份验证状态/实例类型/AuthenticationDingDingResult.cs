using Microsoft.AspNetCore;
using Microsoft.AspNetCore.DataProtection;

namespace System.DingDing;

/// <summary>
/// 这个记录是钉钉身份验证的结果
/// </summary>
public sealed record AuthenticationDingDingResult : ISecret<AuthenticationDingDingResult>
{
    #region 刷新Token
    /// <summary>
    /// 获取用于刷新的Token
    /// </summary>
    public required string RefreshToken { get; init; }
    #endregion
    #region 访问Token
    /// <summary>
    /// 获取用于访问的Token
    /// </summary>
    public required string AccessToken { get; init; }
    #endregion
    #region 身份验证信息
    /// <summary>
    /// 获取已验证的钉钉身份验证信息
    /// </summary>
    public required DingDingUserInfo UserInfo { get; init; }
    #endregion
    #region 是否加密
    public required bool IsEncryption { get; init; }
    #endregion
    #region 加密请求
    public AuthenticationDingDingResult Encryption(IDataProtector dataProtector)
        => IsEncryption ?
        this :
        new()
        {
            AccessToken = dataProtector.Protect(AccessToken),
            RefreshToken = dataProtector.Protect(RefreshToken),
            IsEncryption = true,
            UserInfo = UserInfo
        };
    #endregion
    #region 解密请求
    public AuthenticationDingDingResult Decryption(IDataProtector dataProtector)
        => IsEncryption ?
        new()
        {
            AccessToken = dataProtector.Unprotect(AccessToken),
            RefreshToken = dataProtector.Unprotect(RefreshToken),
            IsEncryption = false,
            UserInfo = UserInfo
        } :
        this;
    #endregion
    #region 获取身份验证请求
    /// <summary>
    /// 将本对象转换为一个身份验证请求
    /// </summary>
    /// <returns></returns>
    public AuthenticationDingDingRequest GetAuthenticationRequest()
        => new()
        {
            Code = AccessToken,
            IsToken = true,
            RefreshToken = RefreshToken,
            IsEncryption = IsEncryption
        };
    #endregion
}
