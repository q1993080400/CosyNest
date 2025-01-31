using System.NetFrancis;

namespace System;

public static partial class ExtendBlazorWebAssembly
{
    //这个静态类专门用来声明和Http有关的扩展方法

    #region 注入携带Cookie的IHttpClient
    /// <summary>
    /// 以范围模式注入一个<see cref="IHttpClient"/>，
    /// 它专门用于WebAssembly模式，可以自动携带Cookie
    /// </summary>
    /// <param name="services">要注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClientWebAssembly(this IServiceCollection services)
    {
        services.AddIHttpClient(ServiceLifetime.Scoped);
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
}
