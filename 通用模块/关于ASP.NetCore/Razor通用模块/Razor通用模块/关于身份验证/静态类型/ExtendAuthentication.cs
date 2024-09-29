using System.Security.Claims;

namespace System;

public static partial class ExtendRazor
{
    //这个部分类专门用来声明有关身份验证的扩展方法

    #region 注册客户端身份验证服务
    /// <summary>
    /// 以范围模式注入一个<see cref="AuthenticationStateProvider"/>，
    /// 它在客户端运行，通过预呈现状态获取身份验证信息，
    /// 除非刷新客户端，否则身份验证信息不会改变，
    /// 因此在登录和注销的时候，请刷新页面
    /// </summary>
    /// <param name="services">用来添加服务的容器</param>
    /// <returns></returns>
    /// <inheritdoc cref="PersistentAuthenticationStateProvider(PersistentComponentState, Func{PersistentComponentState, ClaimsPrincipal?})"/>
    public static IServiceCollection AddAuthenticationStateProviderClient
        (this IServiceCollection services,
        Func<PersistentComponentState, ClaimsPrincipal?> getUser)
        => services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
        {
            var persistentComponentState = serviceProvider.GetRequiredService<PersistentComponentState>();
            return new PersistentAuthenticationStateProvider(persistentComponentState, getUser);
        });
    #endregion
}
