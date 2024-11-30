namespace Microsoft;

public static partial class ExtendRazor
{
    //这个部分类专门用来储存和上传有关的扩展方法

    #region 注入IFileUploadNavigationContext
    #region 可指定阻止用户离开的事件
    /// <summary>
    /// 以范围模式注入一个<see cref="IFileUploadNavigationContext"/>，
    /// 并将其作为级联参数传递给所有组件
    /// </summary>
    /// <param name="services">待注入服务的容器</param>
    /// <returns></returns>
    /// <inheritdoc cref="CreateRazor.FileUploadNavigationContext(NavigationManager, Func{IReadOnlyList{UploadTaskInfo}, Task{bool}})"/>
    public static IServiceCollection AddFileUploadNavigationContext(this IServiceCollection services, Func<IReadOnlyList<UploadTaskInfo>, Task<bool>> blockNavigation)
        => services.AddCascadingValue(serviceProvider =>
        {
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            return CreateRazor.FileUploadNavigationContext(navigationManager, blockNavigation);
        });
    #endregion
    #region 直接弹窗提示
    /// <summary>
    /// 以范围模式注入一个<see cref="IFileUploadNavigationContext"/>，
    /// 并将其作为级联参数传递给所有组件，
    /// 当正在上传文件，且用户试图离开的时候，
    /// 会给予一个弹窗提示，询问用户是否离开
    /// </summary>
    /// <param name="services">待注入服务的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddFileUploadNavigationContextPrompt(this IServiceCollection services)
        => services.AddCascadingValue(serviceProvider =>
        {
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            var jsWindows = serviceProvider.GetRequiredService<IJSWindow>();
            return CreateRazor.FileUploadNavigationContextPrompt(navigationManager, jsWindows);
        });
    #endregion
    #endregion
    #region 安全获取上传锁
    /// <param name="context">要获取上传锁的<see cref="IFileUploadNavigationContext"/>，
    /// 如果它为<see langword="null"/>，则返回一个不执行任何操作的<see cref="IDisposable"/></param>
    /// <inheritdoc cref="IFileUploadNavigationContext.UploadLock"/>
    public static IDisposable UploadLockSecure(this IFileUploadNavigationContext? context)
        => context?.UploadLock() ?? FastRealize.DisposableEmpty();
    #endregion
}
