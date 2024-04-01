using System.Design;
using System.NetFrancis;
using System.NetFrancis.Http;

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
    /// <param name="configuration">配置对象，它可以用来寻找回退路径</param>
    /// <returns></returns>
    public static IServiceCollection AddUriManagerServer(this IServiceCollection services, IConfiguration configuration)
    {
        const string key = "FallbackUri";
        var fallbackUri = configuration.GetValue<string>(key) ??
            throw new KeyNotFoundException($"请在配置文件中设置回退路径，它名叫{key}");
        services.AddScoped<Tag<IUriManager>>();
        services.AddScoped(x =>
        {
            var uri = x.GetRequiredService<Tag<IUriManager>>().Content?.Uri.Text;
            return CreateNet.UriManager(uri ?? fallbackUri);
        });
        return services;
    }
    #endregion
}
