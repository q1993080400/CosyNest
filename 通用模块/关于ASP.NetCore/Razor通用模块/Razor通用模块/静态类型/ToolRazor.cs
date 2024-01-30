using System.Reflection;

namespace Microsoft.AspNetCore;

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
    /// <inheritdoc cref="GetRoute(Type)"/>
    public static string? GetRoute<Component>()
        where Component : IComponent
        => GetRoute(typeof(Component));
    #endregion
    #region 不会返回null
    /// <inheritdoc cref="GetRouteSafe(Type)"/>
    /// <inheritdoc cref="GetRoute{Component}"/>
    public static string GetRouteSafe<Component>()
        where Component : IComponent
        => GetRoute<Component>() ??
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
    /// <returns></returns>
    public static string? GetRoute(Type componentType)
        => componentType.GetCustomAttribute<RouteAttribute>()?.Template;
    #endregion
    #region 不会返回null
    /// <summary>
    /// 获取一个组件的路由模板，
    /// 如果它不是可路由组件，则引发异常
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetRoute(Type)"/>
    public static string GetRouteSafe(Type componentType)
        => GetRoute(componentType) ??
        throw new NullReferenceException($"{componentType}没有指定{nameof(RouteAttribute)}特性，它没有路由模板");
    #endregion
    #endregion
    #endregion
    #region 在脚本外追加新的脚本
    /// <summary>
    /// 在一个原有的JS脚本外追加新的JS脚本，
    /// 它们具有不同的变量作用域，不会冲突
    /// </summary>
    /// <param name="script">原有的脚本</param>
    /// <param name="appendScript">追加的新脚本</param>
    /// <returns></returns>
    public static string AppendScript(string script, params string[] appendScript)
    {
        var newScript = appendScript.Select(appendScript =>
        {
            var jsObjectName = ToolASP.CreateJSObjectName();
            return $$"""

            function {{jsObjectName}}()
            {
                {{appendScript}}
            };
            {{jsObjectName}}();
            """;
        }).Prepend(script).ToArrayIfDeBug();
        return string.Join(Environment.NewLine, newScript);
    }
    #endregion
}
