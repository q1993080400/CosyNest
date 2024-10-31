namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录表示一个上传任务
/// </summary>
public sealed record UploadTaskInfo : IAsyncDisposable
{
    #region 公开成员
    #region 所有待上传文件
    /// <summary>
    /// 获取所有待上传文件
    /// </summary>
    public required IReadOnlyList<UploadFileInfo> UploadFileInfo { get; init; }
    #endregion
    #region 上传任务的状态
    /// <summary>
    /// 获取当前上传任务的状态
    /// </summary>
    public UploadTaskStatus UploadTaskStatus { get; set; }
    #endregion
    #region 是否正在上传中
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示目前正在上传中
    /// </summary>
    public bool Uploading
        => UploadTaskStatus is UploadTaskStatus.Uploading;
    #endregion
    #region 防止重复上传的CSS
    /// <summary>
    /// 这个属性返回一个CSS类名，
    /// 如果上传操作正在进行，
    /// 通过它可以禁用组件的点击事件，防止重复上传
    /// </summary>
    public string PreventDuplicationCSS
        => Uploading ? "prohibitClicking" : "";
    #endregion
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        var objectURL = UploadFileInfo.Select(x => x.PreviewInfo?.PreviewImageUri).WhereNotNull().ToArray();
        if (objectURL.Length > 0)
            await JSWindow.InvokeVoidAsync("DisposablePreviewImage", objectURL);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的JS运行时对象
    /// <summary>
    /// 封装的JS运行时对象，
    /// 它被用来执行释放预览图片的JS代码
    /// </summary>
    private IJSWindow JSWindow { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="jsWindow">封装的JS运行时对象</param>
    public UploadTaskInfo(IJSWindow jsWindow)
    {
        JSWindow = jsWindow;
    }
    #endregion
}
