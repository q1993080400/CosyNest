
using System.Net;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System.NetFrancis;

/// <summary>
/// 这个类型是<see cref="IConfigurationSignalRFactory"/>的实现，
/// 可以用来配置SignalR工厂
/// </summary>
/// <param name="serviceProvider">一个用于请求服务的对象</param>
public class ConfigurationSignalRFactory(IServiceProvider serviceProvider) : IConfigurationSignalRFactory
{
    #region 公开成员
    #region 配置SignalR工厂
    public async Task<IHubConnectionBuilder> Builder(string uri)
    {
        var absoluteUri = ToAbsoluteUri(uri);
        var cookie = await GetCookie();
        var builder = new HubConnectionBuilder();
        builder.WithUrl(absoluteUri, op =>
        {
            if (cookie is { })
                op.Cookies = cookie;
        }).WithStatefulReconnect();
        return builder;
    }
    #endregion
    #region 转换中心地址
    public virtual string ToAbsoluteUri(string uri)
    {
        var hostProvide = ServiceProvider.GetService<IHostProvide>();
        return hostProvide?.Convert(uri, true) ?? uri;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 服务请求对象
    /// <summary>
    /// 获取一个用于请求服务的对象
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    #endregion
    #region 获取Cookie
    /// <summary>
    /// 获取Cookie，
    /// 如果没有或者不需要Cookie，
    /// 则返回<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    protected virtual Task<CookieContainer?> GetCookie()
        => Task.FromResult<CookieContainer?>(null);
    #endregion
    #endregion
}
