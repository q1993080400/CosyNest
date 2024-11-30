using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 有关Razor的工具类
/// </summary>
public static class ToolRazor
{
    #region 返回一个组件的路由模板
    #region 注意事项
    /*路由模板不等于路由，它还可能包含参数等信息*/
    #endregion
    #region 泛型方法
    #region 会返回null
    /// <typeparam name="Component">组件的类型</typeparam>
    /// <inheritdoc cref="GetRoute(Type, ValueTuple{string, string}[])"/>
    public static string? GetRoute<Component>(params (string Name, string Value)[] parameter)
        where Component : IComponent
        => GetRoute(typeof(Component), parameter);
    #endregion
    #region 不会返回null
    /// <summary>
    /// 获取一个组件的路由模板，
    /// 如果它不是可路由组件，则引发异常
    /// </summary>
    /// <inheritdoc cref="GetRoute{Component}(ValueTuple{string, string}[])"/>
    public static string GetRouteSafe<Component>(params (string Name, string Value)[] parameter)
        where Component : IComponent
        => GetRoute<Component>(parameter) ??
        throw new NullReferenceException($"{typeof(Component)}没有指定{nameof(RouteAttribute)}特性，它没有路由模板");
    #endregion
    #endregion
    #region 非泛型方法
    #region 会返回null
    /// <summary>
    /// 获取一个组件的路由模板，
    /// 如果它不是可路由组件，则返回<see langword="null"/>
    /// </summary>
    /// <param name="componentType">组件的类型</param>
    /// <param name="parameter">枚举查询字符串参数的名称和值</param>
    /// <returns></returns>
    public static string? GetRoute(Type componentType, params (string Name, string Value)[] parameter)
    {
        if (!typeof(IComponent).IsAssignableFrom(componentType))
            throw new NotSupportedException($"{componentType}不是一个Blazor组件");
        var template = componentType.GetCustomAttribute<RouteAttribute>()?.Template;
        return (template, parameter) switch
        {
            (null, _) => null,
            ({ }, []) => template,
            _ => $"{template}?{parameter.Join(static x => $"{x.Name}={x.Value}", "&")}"
        };
    }
    #endregion
    #region 不会返回null
    /// <summary>
    /// 获取一个组件的路由模板，
    /// 如果它不是可路由组件，则引发异常
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetRoute(Type, ValueTuple{string, string}[])"/>
    public static string GetRouteSafe(Type componentType, params (string Name, string Value)[] parameter)
        => GetRoute(componentType, parameter) ??
        throw new NullReferenceException($"{componentType}没有指定{nameof(RouteAttribute)}特性，它没有路由模板");
    #endregion
    #endregion
    #endregion
}
