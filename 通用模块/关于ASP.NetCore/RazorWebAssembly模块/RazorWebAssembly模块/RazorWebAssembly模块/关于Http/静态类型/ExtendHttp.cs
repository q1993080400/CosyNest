using System.NetFrancis;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace System;

public static partial class ExtendBlazorWebAssembly
{
    //这个部分类专门用来声明和Http有关的扩展方法

    #region 注入携带Cookie的IHttpClient
    /// <summary>
    /// 以范围模式注入一个<see cref="IHttpClient"/>，
    /// 它专门用于WebAssembly模式，可以自动携带Cookie
    /// </summary>
    /// <param name="services">要注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClientWebAssembly(this IServiceCollection services)
    {
        services.AddIHttpClient();
        services.AddTransient<CookieHandler>();
        services.AddHttpClient(CreateNet.HttpClientName).
            AddHttpMessageHandler<CookieHandler>();
        return services;
    }
    #endregion
    #region 注入强类型调用
    /// <summary>
    /// 以范围模式注入一个<see cref="IStrongTypeInvokeFactory"/>，
    /// 它专门用于WebAssembly模式，可以用来发起强类型调用
    /// </summary>
    /// <param name="services">要注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddStrongTypeInvokeFactoryWebAssembly(this IServiceCollection services)
    {
        services.AddIHttpClientWebAssembly();
        services.AddHttpStrongTypeInvokeFactory();
        return services;
    }
    #endregion
    #region 注入IHostProvide
    /// <summary>
    /// 以范围模式注入一个<see cref="IHostProvide"/>，
    /// 它专门用于WebAssembly模式，可以用来提供主机Uri
    /// </summary>
    /// <param name="services">要注入的服务容器</param>
    /// <param name="webAssemblyHostBuilder">用来创建WebAssembly主机的对象</param>
    /// <returns></returns>
    public static IServiceCollection AddHostProvideWebAssembly(this IServiceCollection services, WebAssemblyHostBuilder webAssemblyHostBuilder)
    {
        var host = webAssemblyHostBuilder.HostEnvironment.BaseAddress;
        services.AddScoped(_ => CreateNet.HostProvide(host));
        services.AddScoped(_ => new TagLazy<UriHost>(() => Task.FromResult(new UriHost(host))));
        return services;
    }
    #endregion
}