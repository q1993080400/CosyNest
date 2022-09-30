namespace System.SafetyFrancis.Authentication;

/// <summary>
/// 这个类型是一个不安全的Web凭据，
/// 它直接封装了用户名，密码和主机名
/// </summary>
/// <param name="Host">凭据的主机名</param>
/// <param name="IsEncrypt">如果这个值为<see langword="true"/>，则使用加密连接</param>
/// <inheritdoc cref="UnsafeCredentials(string, string)"/>
public sealed record UnsafeWebCredentials(string ID, string Password, string Host, bool IsEncrypt) : UnsafeCredentials(ID, Password)
{
    #region 主机名
    /// <summary>
    /// 获取凭据的主机名
    /// </summary>
    public string Host { get; init; } = Host;
    #endregion
    #region 是否加密
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则使用加密连接
    /// </summary>
    public bool IsEncrypt { get; init; } = IsEncrypt;
    #endregion
}
