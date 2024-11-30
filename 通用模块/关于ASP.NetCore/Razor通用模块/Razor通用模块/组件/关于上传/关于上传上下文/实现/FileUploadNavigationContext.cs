﻿using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IFileUploadNavigationContext"/>的实现，
/// 可以视为一个上传文件时，用来阻止导航的上下文
/// </summary>
/// <param name="navigationManager">导航管理对象，它用来注册阻止导航的事件</param>
/// <param name="blockNavigation">这个委托传入正在上传的任务，返回一个布尔值，它指示是否应该阻止导航</param>
sealed class FileUploadNavigationContext(NavigationManager navigationManager, Func<IReadOnlyList<UploadTaskInfo>, Task<bool>> blockNavigation) : IFileUploadNavigationContext
{
    #region 公开成员
    #region 注册上传任务
    public void RegisterUploadTaskInfo(string componentID, UploadTaskInfo uploadTaskInfo)
    {
        CacheUploadTask = CacheUploadTask.SetItem(componentID, uploadTaskInfo);
    }
    #endregion
    #region 注销上传任务
    public void CancelUploadTaskInfo(string componentID)
    {
        CacheUploadTask = CacheUploadTask.Remove(componentID);
    }
    #endregion
    #region 获取上传锁
    public IDisposable UploadLock()
        => navigationManager.RegisterLocationChangingHandler(async context =>
        {
            var uploading = CacheUploadTask.Values.Where(x => x.UploadFileInfo.Count > 0).ToArray();
            if (uploading.Length is 0)
                return;
            if (await blockNavigation(uploading))
                context.PreventNavigation();
        });
    #endregion
    #endregion
    #region 内部成员
    #region 缓存上传任务的字典
    /// <summary>
    /// 这个字典的键是上传组件的ID，
    /// 值是该组件的上传任务的缓存
    /// </summary>
    private ImmutableDictionary<string, UploadTaskInfo> CacheUploadTask { get; set; }
    = ImmutableDictionary<string, UploadTaskInfo>.Empty;
    #endregion
    #endregion
}
