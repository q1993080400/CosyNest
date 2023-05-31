using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本类型可用于阻止导航，但是不会锁定用户新的导航
/// </summary>
sealed class LocationChangingHandlerDisposable : IDisposable
{
    #region 公开方法
    #region 释放对象
    public void Dispose()
    {
        Disposable.Dispose();
    }
    #endregion
    #endregion
    #region 内部方法
    #region 导航对象
    /// <summary>
    /// 用来执行导航的对象
    /// </summary>
    private NavigationManager NavigationManager { get; }
    #endregion
    #region 导航事件
    /// <summary>
    /// 当导航触发时的事件，
    /// 它返回一个布尔值，指示是否应该阻止导航
    /// </summary>
    private Func<LocationChangingContext, ValueTask<bool>> LocationChangingHandler { get; }
    #endregion
    #region 用来释放事件的对象
    /// <summary>
    /// 用来释放事件的对象
    /// </summary>
    private IDisposable Disposable { get; set; }
    #endregion
    #region 注册进NavigationManager的事件
    /// <summary>
    /// 注册进<see cref="NavigationManager"/>中的事件
    /// </summary>
    /// <param name="context">导航上下文对象</param>
    /// <returns></returns>
    private async ValueTask OnBeforeInternalNavigation(LocationChangingContext context)
    {
        if (!await LocationChangingHandler(context))
            return;
        context.PreventNavigation();
        Dispose();
        Disposable = NavigationManager.RegisterLocationChangingHandler(OnBeforeInternalNavigation);
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="navigationManager">用来执行导航的对象</param>
    /// <param name="locationChangingHandler">当导航触发时的事件，
    /// 它返回一个布尔值，指示是否应该阻止导航</param>
    public LocationChangingHandlerDisposable(NavigationManager navigationManager, Func<LocationChangingContext, ValueTask<bool>> locationChangingHandler)
    {
        NavigationManager = navigationManager;
        LocationChangingHandler = locationChangingHandler;
        Disposable = navigationManager.RegisterLocationChangingHandler(OnBeforeInternalNavigation);
    }
    #endregion
}
