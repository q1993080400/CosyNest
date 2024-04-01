using System.NetFrancis.Http;

namespace System.SafetyFrancis.Authentication;

/// <summary>
/// 这个类型是一个不安全的Web凭据，
/// 它直接封装了用户名，密码和主机名
/// </summary>
public sealed record UnsafeWebCredentials
{
    #region 主机名
    /// <summary>
    /// 获取凭据的主机名
    /// </summary>
    public required UriHost Host { get; init; }
    #endregion
    #region 凭据部分
    /// <summary>
    /// 获取凭据部分，
    /// 如果为<see langword="null"/>，
    /// 表示不需要身份验证
    /// </summary>
    public required UnsafeCredentials? Credentials { get; init; }
    #endregion
}
