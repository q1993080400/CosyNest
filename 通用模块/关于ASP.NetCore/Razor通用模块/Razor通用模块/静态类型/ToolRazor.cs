using System.Reflection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关Razor的工具类
/// </summary>
public static class ToolRazor
{
    #region 返回一个组件的路由模板
    /// <summary>
    /// 获取一个组件的路由模板，
    /// 如果它不是可路由组件，则引发异常
    /// </summary>
    /// <param name="componentType">要获取路由模板的组件的类型</param>
    /// <returns></returns>
    public static string? GetRoute(Type componentType)
        => typeof(IComponent).IsAssignableFrom(componentType) ?
        componentType.GetCustomAttribute<RouteAttribute>()?.Template :
        throw new NotSupportedException($"{componentType}没有实现{nameof(IComponent)}，不是组件");

    /*注意：路由模板不等于路由，它还可能包含参数等信息*/
    #endregion
}
