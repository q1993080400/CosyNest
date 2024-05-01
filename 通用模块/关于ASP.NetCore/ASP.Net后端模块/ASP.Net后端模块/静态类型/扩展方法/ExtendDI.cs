using System.Design;
using System.NetFrancis;
using System.NetFrancis.Http;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关依赖注入的扩展方法

    #region 注入UriManager
    /// <summary>
    /// 以范围模式注入一个<see cref="IUriManager"/>，
    /// 它必须和<see cref="UseConfigurationUriManager(IApplicationBuilder)"/>配合使用
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddUriManagerServer(this IServiceCollection services)
    {
        services.AddScoped<Tag<IUriManager>>();
        services.AddScoped(x =>
        {
            const string key = "FallbackUri";
            var configuration = x.GetRequiredService<IConfiguration>();
            var fallbackUri = configuration.GetValue<string>(key) ??
                throw new KeyNotFoundException($"请在配置文件中设置回退路径，它名叫{key}");
            var uri = x.GetRequiredService<Tag<IUriManager>>().Content?.Uri.Text;
            return CreateNet.UriManager(uri ?? fallbackUri);
        });
        return services;
    }
    #endregion
    #region 注入SignalR中心服务，且使用MessagePack协议
    /// <summary>
    /// 注入SignalR中心服务，且使用MessagePack协议，
    /// 由于本框架的SignalR客户端只支持MessagePack协议，
    /// 请务必使用本方法来注入SignalR中心
    /// </summary>
    /// <param name="serviceCollection">服务容器</param>
    /// <returns></returns>
    public static ISignalRServerBuilder AddSignalRWithMessagePackProtocol(this IServiceCollection serviceCollection)
        => serviceCollection.AddSignalR().AddMessagePackProtocol();
    #endregion
}
