namespace System.NetFrancis.Http;

/// <summary>
/// 这个特性描述强类型API的路由信息
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
public sealed class RouteDescriptionAttribute(string template) : Attribute
{
    #region 路由模板
    /// <summary>
    /// 返回路由模板，
    /// 它的语法与ApsNet的控制器（或其他后端框架）相同
    /// </summary>
    public string Template { get; init; } = template;
    #endregion
}
