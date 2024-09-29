using System.Security.Claims;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关身份验证的扩展方法

    #region 注册服务端身份验证服务
    /// <summary>
    /// 以范围模式注入一个<see cref="AuthenticationStateProvider"/>，
    /// 它在服务端运行，执行标准的AspNetCore身份验证，并通过预呈现状态将身份验证结果传给客户端，
    /// 每隔一段时间，都会重新刷新验证
    /// </summary>
    /// <param name="services">用来添加服务的容器</param>
    /// <returns></returns>
    /// <inheritdoc cref="PersistingRevalidatingAuthenticationStateProvider(PersistentComponentState, Func{ClaimsPrincipal, PersistentComponentState, Task}, ILoggerFactory, TimeSpan?)"/>
    public static IServiceCollection AddAuthenticationStateProviderServer(this IServiceCollection services,
        Func<ClaimsPrincipal, PersistentComponentState, Task> setState,
        TimeSpan? revalidationInterval = null)
        => services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
        {
            var persistentComponentState = serviceProvider.GetRequiredService<PersistentComponentState>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            return new PersistingRevalidatingAuthenticationStateProvider(persistentComponentState, setState, loggerFactory, revalidationInterval);
        });
    #endregion
}
