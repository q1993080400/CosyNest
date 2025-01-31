using System.NetFrancis;
using System.Reflection;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace System;

public static partial class ExtendBlazorServer
{
    //这个部分类专门声明有关强类型调用的扩展方法

    #region 注入强类型调用
    #region 正式方法
    /// <summary>
    /// 以范围模式注入一个<see cref="IStrongTypeInvokeFactory"/>，
    /// 它专门用于Server模式，可以用来发起强类型调用
    /// </summary>
    /// <param name="services">要注入的服务容器</param>
    /// <param name="interfaceAssembly">强类型调用接口所在的程序集，可以为多个程序集</param>
    /// <param name="realizeAssembly">强类型调用实现所在的程序集，可以为多个程序集</param>
    /// <returns></returns>
    public static IServiceCollection AddStrongTypeInvokeFactoryServer(this IServiceCollection services, IEnumerable<Assembly> interfaceAssembly, IEnumerable<Assembly> realizeAssembly)
    {
        services.AddInlineStrongTypeInvokeFactory();
        services.AddAllInlineInvokeServiceFactory(interfaceAssembly, realizeAssembly);
        return services;
    }
    #endregion
    #region 辅助方法
    #region 添加强类型调用接口工厂
    /// <summary>
    /// 扫描程序集，并添加所有强类型调用接口工厂
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="AddStrongTypeInvokeFactoryServer(IServiceCollection, IEnumerable{Assembly}, IEnumerable{Assembly})"/>
    private static IServiceCollection AddAllInlineInvokeServiceFactory(this IServiceCollection services, IEnumerable<Assembly> interfaceAssembly, IEnumerable<Assembly> realizeAssembly)
    {
        var interfaceTypes = interfaceAssembly.Select(x => x.GetExportedTypes()).SelectMany().
            Where(x => x is { IsInterface: true } && x.IsDefined<ServerAPIAttribute>()).ToArray();
        var realizeTypes = realizeAssembly.Select(x => x.GetExportedTypes()).SelectMany().
            Where(x => x is { IsAbstract: false } && typeof(ControllerBase).IsAssignableFrom(x)).ToArray();
        var method = typeof(ExtendBlazorServer).GetMethod(nameof(AddSingleInlineInvokeServiceFactory),
            BindingFlags.Static | BindingFlags.NonPublic, [typeof(IServiceCollection)]).ThrowIfNull();
        foreach (var interfaceType in interfaceTypes)
        {
            var realizeType = realizeTypes.Where(interfaceType.IsAssignableFrom).SingleOrDefaultSecure();
            if (realizeType is null)
                continue;
            var makeMethod = method.MakeGenericMethod(interfaceType, realizeType);
            makeMethod.Invoke(null, [services]);
        }
        return services;
    }
    #endregion
    #region 添加单个强类型调用接口工厂
    /// <summary>
    /// 添加单个强类型调用接口工厂
    /// </summary>
    /// <typeparam name="Interface">接口的类型</typeparam>
    /// <typeparam name="Realize">实现的类型</typeparam>
    /// <param name="services">要添加的服务容器</param>
    /// <returns></returns>
    private static IServiceCollection AddSingleInlineInvokeServiceFactory<Interface, Realize>(IServiceCollection services)
        where Interface : class
        where Realize : ControllerBase, Interface, new()
        => services.AddTransient<InlineInvokeServiceFactory<Interface>>(serviceProvider =>
        async () =>
        {
            var authenticationStateProvider = serviceProvider.GetRequiredService<AuthenticationStateProvider>();
            var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
            return new Realize()
            {
                ControllerContext = new()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = serviceProvider,
                        User = authenticationState.User
                    }
                }
            };
        });
    #endregion
    #endregion
    #endregion
}
