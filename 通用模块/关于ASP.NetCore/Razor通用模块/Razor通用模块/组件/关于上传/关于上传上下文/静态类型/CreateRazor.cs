namespace Microsoft.AspNetCore.Components;

public static partial class CreateRazor
{
    //这个部分类专门用来声明用来创建和上传有关对象的方法

    #region 创建IFileUploadNavigationContext
    #region 直接创建
    /// <summary>
    /// 创建一个<see cref="IFileUploadNavigationContext"/>，
    /// 在上传文件的时候，如果用户试图离开页面，
    /// 它可以阻止这个过程，以防止离开页面时，文件还没有上传完毕
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="FileUploadNavigationContext.FileUploadNavigationContext(NavigationManager, Func{IReadOnlyList{IHasUploadFile}, Task{bool}}))"/>
    public static IFileUploadNavigationContext FileUploadNavigationContext
        (NavigationManager navigationManager, Func<IReadOnlyList<IHasUploadFile>, Task<bool>> blockNavigation)
        => new FileUploadNavigationContext(navigationManager, blockNavigation);
    #endregion
    #region 通过弹出窗进行提示
    /// <summary>
    /// 创建一个<see cref="IFileUploadNavigationContext"/>，
    /// 在上传文件的时候，如果用户试图离开页面，
    /// 它会弹窗向用户询问，是否确定离开页面
    /// </summary>
    /// <param name="jsWindow">JS运行时对象，它用来进行弹窗</param>
    /// <returns></returns>
    /// <inheritdoc cref="FileUploadNavigationContext.FileUploadNavigationContext(NavigationManager, Func{IReadOnlyList{IHasUploadFile}, Task{bool}}))"/>
    public static IFileUploadNavigationContext FileUploadNavigationContextPrompt(NavigationManager navigationManager, IJSWindow jsWindow)
        => FileUploadNavigationContext(navigationManager, async uploadTask =>
        {
            var prompt = $"有{uploadTask.Count}个文件还没有上传完毕，确定离开页面吗？";
            return !await jsWindow.Confirm(prompt);
        });
    #endregion
    #endregion
}
