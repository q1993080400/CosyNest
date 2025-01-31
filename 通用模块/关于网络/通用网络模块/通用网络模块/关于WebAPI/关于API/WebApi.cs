using Microsoft.Extensions.DependencyInjection;

namespace System.NetFrancis.Api;

/// <summary>
/// 本类型是所有WebApi的基类
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="serviceProvider">服务提供者对象</param>
public abstract class WebApi(IServiceProvider serviceProvider)
{
    #region Http客户端
    /// <summary>
    /// 获取一个Http客户端，它可用于发起请求
    /// </summary>
    protected IHttpClient HttpClient
        => ServiceProvider.GetRequiredService<IHttpClient>();
    #endregion
    #region 服务提供者对象
    /// <summary>
    /// 获取服务提供者对象，
    /// 它可以用于请求服务
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    #endregion
}
