using System.NetFrancis;

using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendNet
{
    //这个部分类专门用来声明有关强类型调用的扩展方法

    #region 注入强类型调用工厂
    #region 通过Http请求进行调用
    /// <summary>
    /// 以范围模式注入一个<see cref="IStrongTypeInvokeFactory"/>，
    /// 它可以用来创建通过发起Http请求进行强类型调用的对象
    /// </summary>
    /// <param name="services">要添加的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddHttpStrongTypeInvokeFactory(this IServiceCollection services)
        => services.AddScoped(serviceProvider =>
        {
            var httpClient = serviceProvider.GetRequiredService<IHttpClient>();
            return httpClient.StrongTypeFactory();
        });
    #endregion
    #region 通过内联服务进行调用
    /// <summary>
    /// 以范围模式注入一个<see cref="IStrongTypeInvokeFactory"/>，
    /// 它可以用来创建通过内联服务进行强类型调用的对象，
    /// 该对象实际上不发起Http请求
    /// </summary>
    /// <param name="services">要添加的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddInlineStrongTypeInvokeFactory(this IServiceCollection services)
        => services.AddScoped<IStrongTypeInvokeFactory>(serviceProvider => new InlineStrongTypeInvokeFactory(serviceProvider));
    #endregion
    #endregion
    #region 将Http客户端封装成强类型调用
    /// <summary>
    /// 将Http客户端封装成一个<see cref="IHttpStrongTypeInvoke{API}"/>，
    /// 它通过发起Http请求进行强类型调用
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IHttpStrongTypeInvoke{API}"/>
    /// <inheritdoc cref="HttpStrongTypeInvoke{API}.HttpStrongTypeInvoke(IHttpClient)"/>
    public static IHttpStrongTypeInvoke<API> StrongType<API>(this IHttpClient httpClient)
        where API : class
        => new HttpStrongTypeInvoke<API>(httpClient);
    #endregion
    #region 将Http客户端封装成强类型调用工厂
    /// <summary>
    /// 将Http客户端封装成一个<see cref="IStrongTypeInvokeFactory"/>，
    /// 它是一个工厂，可以创建通过发起Http请求进行强类型调用的对象
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="HttpStrongTypeInvokeFactory(IHttpClient)"/>
    public static IStrongTypeInvokeFactory StrongTypeFactory(this IHttpClient httpClient)
        => new HttpStrongTypeInvokeFactory(httpClient);
    #endregion
}
