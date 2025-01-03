﻿using System.DataFrancis;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明和上传有关的扩展方法

    #region 筛选启用的可预览对象
    /// <summary>
    /// 筛选集合中所有被启用的<see cref="IHasReadOnlyPreviewFile"/>
    /// </summary>
    /// <typeparam name="Obj">要筛选的集合的元素的类型</typeparam>
    /// <param name="objs">要筛选的集合</param>
    /// <returns></returns>
    public static IEnumerable<Obj> WhereEnable<Obj>(this IEnumerable<Obj?> objs)
        where Obj : IHasReadOnlyPreviewFile
        => objs.Where(x => x is { IsEnable: true })!;
    #endregion
    #region 筛选未上传的对象
    /// <summary>
    /// 筛选集合中所有被启用，而且未上传的对象
    /// </summary>
    /// <typeparam name="Obj">要筛选的集合的元素的类型</typeparam>
    /// <param name="objs">要筛选的集合</param>
    /// <returns></returns>
    public static IEnumerable<Obj> WhereEnableAndNotUpload<Obj>(this IEnumerable<Obj?> objs)
        where Obj : IHasReadOnlyPreviewFile
        => objs.Where(x => x is { IsEnable: true } and not IHasUploadFileClient { IsUploadCompleted: true })!;
    #endregion
    #region 筛选未保存的对象
    /// <summary>
    /// 筛选集合中所有被启用，而且未保存到服务器中的对象
    /// </summary>
    /// <param name="files">要筛选的集合</param>
    /// <returns></returns>
    public static IEnumerable<IHasUploadFileServer> WhereEnableAndNotSave(this IEnumerable<IHasPreviewFile?> files)
        => files.OfType<IHasUploadFileServer>().
        Where(x => x is { IsEnable: true, SaveState: not UploadFileSaveState.SaveCompleted });
    #endregion
    #region 检查是否存在不可上传的对象
    /// <summary>
    /// 如果一个可预览文件集合中存在应该上传，
    /// 但是不可上传的对象，则抛出一个异常，
    /// 在开始上传之前，请务必调用本方法
    /// </summary>
    /// <param name="files">要检查的可预览文件集合</param>
    public static void CheckCannotUpload(this IEnumerable<IHasPreviewFile> files)
    {
        if (files.Any(x => x is IHasUploadFileServer { SaveState: not UploadFileSaveState.HasUploadFileName }))
            throw new NotSupportedException($"可预览文件集合之中存在应该上传，但是不可上传的对象");
    }
    #endregion
}
