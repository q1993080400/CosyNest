namespace System.NetFrancis.Api;

/// <summary>
/// 这个记录可以用来作为创建百度云盘API的参数
/// </summary>
public sealed record BaidupanAPIInfo
{
    #region 访问令牌
    /// <summary>
    /// 访问令牌，它用于验证身份
    /// </summary>
    public required string AccessToken { get; init; }
    #endregion
    #region 刷新令牌
    /// <summary>
    /// 刷新令牌，当访问令牌失效后，通过它刷新访问令牌
    /// </summary>
    public required string RefreshToken { get; init; }
    #endregion
    #region 应用ID
    /// <summary>
    /// 应用ID，它对应AppKey
    /// </summary>
    public required string ClientId { get; init; }
    #endregion
    #region 应用密钥
    /// <summary>
    /// 应用密钥，它对应SecretKey
    /// </summary>
    public required string ClientSecret { get; init; }
    #endregion
    #region 用于保存刷新令牌的委托
    /// <summary>
    /// 用于保存刷新令牌的委托，
    /// 它的参数就是保存了新令牌的<see cref="BaidupanAPIInfo"/>对象
    /// </summary>
    public required Func<BaidupanAPIInfo, Task> SaveToken { get; init; }
    #endregion
    #region 服务提供对象
    /// <summary>
    /// 获取服务提供者对象，
    /// 它可以用于请求服务
    /// </summary>
    public required IServiceProvider ServiceProvider { get; init; }
    #endregion
}
