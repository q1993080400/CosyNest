﻿using System.NetFrancis;
using System.Reflection;

using Microsoft.AspNetCore.Components.Authorization;

namespace System;

/// <summary>
/// 关于Razor的扩展方法全部放在这里
/// </summary>
public static partial class ExtendRazor
{
    #region 关于依赖注入
    #region 注入前端对象
    /// <summary>
    /// 向服务容器注入常用前端对象
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddFront(this IServiceCollection services)
    {
        services.AddJSWindow();
        return services;
    }
    #endregion
    #endregion
    #region 关于组件
    #region 返回组件参数
    /// <summary>
    /// 读取一个组件的参数，然后返回
    /// </summary>
    /// <param name="component">要读取参数的组件</param>
    /// <returns></returns>
    public static IDictionary<string, object> GetParameters(this IComponent component)
    {
        var parameters = component.GetType().GetProperties().
            Where(x => x.IsDefined<ParameterAttribute>()).
            Select(x => (x.Name, x.GetValue(component))).ToArray();
        return parameters.Where(x => x.Item2 is { }).ToDictionary()!;
    }
    #endregion
    #region 是否为可路由组件
    /// <summary>
    /// 返回某个类型，是否为可路由的Blazor组件
    /// </summary>
    /// <param name="type">要判断的组件类型</param>
    /// <returns></returns>
    public static bool IsRouteComponent(this Type type)
        => typeof(IComponent).IsAssignableFrom(type) &&
        type.IsDefined<RouteAttribute>();
    #endregion
    #region 公开StateHasChanged方法
    /// <summary>
    /// 公开组件的StateHasChanged方法，并调用它
    /// </summary>
    /// <param name="component">要刷新的组件</param>
    public static void StateHasChanged(this ComponentBase component)
    {
        var stateHasChanged = component.GetType().GetMethod("StateHasChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
        stateHasChanged.Invoke(component, null);
    }
    #endregion
    #region 刷新组件
    /// <summary>
    /// 调用组件的StateHasChanged方法，
    /// 如果它实现了<see cref="IRefresh"/>，还会刷新它
    /// </summary>
    /// <param name="component">要刷新的组件，
    /// 如果它为<see langword="null"/>，不会执行任何操作</param>
    /// <returns></returns>
    public static async Task StateHasChangedRefresh(this ComponentBase? component)
    {
        if (component is null)
            return;
        if (component is IRefresh refresh)
            await refresh.Refresh();
        component.StateHasChanged();
    }
    #endregion
    #region 合并渲染RenderFragment
    /// <summary>
    /// 返回一个新的<see cref="RenderFragment"/>，
    /// 它依次渲染集合中的所有<see cref="RenderFragment"/>
    /// </summary>
    /// <param name="sonRender">要渲染的<see cref="RenderFragment"/>集合</param>
    /// <returns></returns>
    public static RenderFragment MergeRender(this IEnumerable<RenderFragment> sonRender)
        => builder =>
        {
            foreach (var item in sonRender)
            {
                item(builder);
            }
        };
    #endregion
    #endregion
    #region 关于NavigationManager
    #region 导航到组件
    #region 泛型方法
    /// <typeparam name="Component">组件的类型</typeparam>
    /// <inheritdoc cref="NavigateToComponent(NavigationManager, Type, UriParameter?,bool)"/>
    public static void NavigateToComponent<Component>(this NavigationManager navigation, UriParameter? parameter = null, bool forceLoad = false)
        where Component : IComponent
        => navigation.NavigateToComponent(typeof(Component), parameter, forceLoad);
    #endregion
    #region 非泛型方法
    /// <summary>
    /// 导航到组件
    /// </summary>
    /// <param name="navigation">导航对象</param>
    /// <param name="componentType">组件的类型</param>
    /// <param name="parameter">组件路径的参数部分，
    /// 如果为<see langword="null"/>，表示没有参数</param>
    /// <param name="forceLoad">如果这个值为<see langword="true"/>，则绕过缓存强制刷新</param>
    public static void NavigateToComponent(this NavigationManager navigation, Type componentType, UriParameter? parameter = null, bool forceLoad = false)
    {
        var uri = new UriComplete()
        {
            UriExtend = ToolRazor.GetRouteSafe(componentType),
            UriParameter = parameter
        };
        navigation.NavigateTo(uri, forceLoad);
    }
    #endregion
    #endregion
    #endregion 
    #region 公开NotifyAuthenticationStateChanged方法
    /// <summary>
    /// 公开NotifyAuthenticationStateChanged方法，
    /// 它通知用户登录状态发生改变
    /// </summary>
    /// <param name="authenticationStateProvider">要调用方法的<see cref="AuthenticationStateProvider"/>对象</param>
    /// <param name="task">新传入的用户登录状态</param>
    public static void NotifyAuthenticationStateChanged(this AuthenticationStateProvider authenticationStateProvider, Task<AuthenticationState> task)
    {
        if (authenticationStateProvider is IHostEnvironmentAuthenticationStateProvider hostAuthenticationStateProvider)
            hostAuthenticationStateProvider.SetAuthenticationState(task);
        var method = typeof(AuthenticationStateProvider).
            GetMethod("NotifyAuthenticationStateChanged", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(authenticationStateProvider, [task]);
    }
    #endregion
}
