using System.Design;
using System.NetFrancis.Http;

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
            var tag = x.GetRequiredService<Tag<IUriManager>>();
            return tag.Content ??
            throw new NullReferenceException($"请求到的{nameof(IUriManager)}为null，" +
            $"您是不是忘了调用{nameof(UseConfigurationUriManager)}中间件？");
        });
        return services;
    }
    #endregion
}
