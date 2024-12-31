namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个上传文件时，用来阻止导航的上下文，
/// 当存在多个上传组件时，它可以保证用来阻止导航的事件仅会触发一次
/// </summary>
public interface IFileUploadNavigationContext
{
    #region 注册上传任务
    /// <summary>
    /// 注册一个上传任务，
    /// 当有任务上传，且试图离开页面的时候，
    /// 会触发事件
    /// </summary>
    /// <param name="componentID">上传组件的ID</param>
    /// <param name="hasUploadFiles">这个上传组件所对应的上传文件</param>
    void RegisterUploadTaskInfo(string componentID, IEnumerable<IHasUploadFile> hasUploadFiles);
    #endregion
    #region 注销上传任务
    /// <summary>
    /// 注销上传任务，
    /// 它不会再与试图离开页面的事件相关联
    /// </summary>
    /// <inheritdoc cref="RegisterUploadTaskInfo(string, IEnumerable{IHasUploadFile})"/>
    void CancelUploadTaskInfo(string componentID);
    #endregion
    #region 获取上传锁
    /// <summary>
    /// 开始侦听阻止导航事件，
    /// 并返回一个<see cref="IDisposable"/>，
    /// 当它被释放时，该事件也会被释放
    /// </summary>
    /// <returns></returns>
    IDisposable UploadLock();
    #endregion
}
