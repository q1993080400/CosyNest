namespace Microsoft.AspNetCore.Components.Authorization;

/// <summary>
/// 本组件与<see cref="AuthorizeRouteView"/>类似，
/// 但是区别之处在于，可以自定义获取授权时显示的内容
/// </summary>
public sealed partial class AuthorizeRouteCustomView : ComponentBase
{
    #region 组件参数
    #region 获得授权时显示的内容
    /// <summary>
    /// 当获得授权时，显示这个内容，
    /// 它的参数是当前路由数据
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RouteData> Authorized { get; set; }
    #endregion
    #region 路由数据
    /// <summary>
    /// 当前路由数据
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RouteData RouteData { get; set; }
    #endregion
    #region 正在授权时显示的内容
    /// <summary>
    /// 获取正在授权时显示的内容
    /// </summary>
    [Parameter]
    public RenderFragment? Authorizing { get; set; }
    #endregion
    #region 未授权时显示的内容
    /// <summary>
    /// 获取当未授权时显示的内容
    /// </summary>
    [Parameter]
    public RenderFragment<AuthenticationState>? NotAuthorized { get; set; }
    #endregion
    #region 授权要求的策略
    /// <summary>
    /// 获取授权要求的策略，
    /// 可以要求多个策略，
    /// 如果不填，默认根据路由数据获取
    /// </summary>
    [Parameter]
    public string[]? Policy { get; set; }
    #endregion
    #region 授权要求的角色
    /// <summary>
    /// 获取授权要求的角色，
    /// 可以要求多个角色
    /// 如果不填，默认根据路由数据获取
    /// </summary>
    [Parameter]
    public string[]? Roles { get; set; }
    #endregion
    #region 要控制访问的资源
    /// <summary>
    /// 获取授权时要控制访问的资源
    /// </summary>
    [Parameter]
    public object? Resource { get; set; }
    #endregion
    #endregion
}
