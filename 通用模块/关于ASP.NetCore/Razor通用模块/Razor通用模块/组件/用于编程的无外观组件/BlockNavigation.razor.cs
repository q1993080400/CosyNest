namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许阻止用户离开页面
/// </summary>
public sealed partial class BlockNavigation : ComponentBase, IDisposable
{
    #region 组件参数
    #region 是否阻止导航
    /// <summary>
    /// 这个委托返回一个布尔值，
    /// 它指示是否阻止导航
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Task<bool>> IsBlock { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public void Dispose()
    {
        NavigationLock?.Dispose();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 导航锁
    /// <summary>
    /// 获取导航锁，
    /// 只要它未被释放，就会持续阻止导航
    /// </summary>
    private IDisposable? NavigationLock { get; set; }
    #endregion
    #region 重写OnAfterRender
    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;
        NavigationLock = NavigationManager.RegisterLocationChangingHandlerDisposable(IsBlock);
    }
    #endregion
    #endregion
}
