namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录可以用来创建钉钉WebAPI
/// </summary>
public sealed record DingDingWebApiInfo
{
    #region 用来请求服务的对象
    /// <summary>
    /// 获取用来请求服务的对象
    /// </summary>
    public required IServiceProvider ServiceProvider { get; init; }
    #endregion
}
