using System.Media;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以用来上传文件
/// </summary>
public sealed partial class FileUpload : ComponentBase, IContentComponent<RenderFragment<RenderFileUploadInfo>>, IAsyncDisposable
{
    #region 组件参数
    #region 用来上传文件的委托
    /// <summary>
    /// 当开始上传时，执行这个委托，
    /// 它的参数就是当前上传任务
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<UploadTaskInfo, Task> OnUpload { get; set; }
    #endregion
    #region 当上传时离开页面的委托
    /// <summary>
    /// 当有文件正在上传，
    /// 且用户试图离开页面时，触发这个委托，
    /// 它的参数是当前上传任务，
    /// 返回值是是否允许用户离开页面，
    /// 它可以避免用户在尚未上传完毕时离开页面
    /// </summary>
    [Parameter]
    public Func<UploadTaskInfo, Task<bool>>? OnUploadLeave { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFileUploadInfo> ChildContent { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        if (UploadTaskInfo is { })
            await UploadTaskInfo.DisposeAsync();
        LocationChangingHandler?.Dispose();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 导航事件
    /// <summary>
    /// 当释放这个对象的时候，
    /// 会解除所有导航事件
    /// </summary>
    private IDisposable? LocationChangingHandler { get; set; }
    #endregion
    #region 上传组件的ID
    /// <summary>
    /// 获取这个上传组件的ID，
    /// 通过它可以找到这个上传组件
    /// </summary>
    private string ID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 当前上传任务
    /// <summary>
    /// 获取当前正在执行的上传任务，
    /// 如果没有上传任务，则为<see langword="null"/>
    /// </summary>
    private UploadTaskInfo? UploadTaskInfo { get; set; }
    #endregion
    #region 重写OnParametersSet方法
    protected override void OnParametersSet()
    {
        LocationChangingHandler?.Dispose();
        if (OnUploadLeave is { })
            LocationChangingHandler = NavigationManager.RegisterLocationChangingHandler(async context =>
            {
                if ((UploadTaskInfo, OnUploadLeave) is ({ Uploading: true }, { }) && !await OnUploadLeave(UploadTaskInfo))
                    context.PreventNavigation();
            });
    }
    #endregion
    #region 当选择文件时触发的事件
    /// <summary>
    /// 当选择文件时，触发这个事件
    /// </summary>
    /// <param name="args">选择文件的参数</param>
    /// <returns></returns>
    private async Task OnChange(InputFileChangeEventArgs args)
    {
        if (UploadTaskInfo is { })
            await UploadTaskInfo.DisposeAsync();
        var uploadFile = args.GetMultipleFiles(args.FileCount);
        var uploadFileInfos = uploadFile.Select(x => new UploadFileInfo()
        {
            File = x,
            PreviewInfo = null
        }).ToArray();
        var initializationFileInfos = await InitializationPreview(uploadFileInfos).ToArrayAsync();
        UploadTaskInfo = new(JSWindow)
        {
            UploadFileInfo = initializationFileInfos
        };
        await OnUpload(UploadTaskInfo);
    }
    #endregion
    #region 初始化待上传的文件
    /// <summary>
    /// 初始化所有待上传的文件，
    /// 它会设置<see cref="UploadFileInfo.PreviewInfo"/>属性
    /// </summary>
    /// <param name="uploadFileInfos">待初始化的上传文件</param>
    /// <returns></returns>
    private async IAsyncEnumerable<UploadFileInfo> InitializationPreview(UploadFileInfo[] uploadFileInfos)
    {
        if (uploadFileInfos.Length is 0)
            yield break;
        var previewFiles = uploadFileInfos.PackIndex().
            Where(x => x.Elements.MediumFileType is MediumFileType.Image or MediumFileType.Video).ToArray();
        var previewIndex = previewFiles.Select(x => x.Index).ToArray();
        var urls = await JSWindow.InvokeAsync<string[]>("InitializationPreviewImage", ID, previewIndex);
        var fileAndUrl = uploadFileInfos.Zip(urls).ToArray();
        foreach (var (file, url) in fileAndUrl)
        {
            yield return file with
            {
                PreviewInfo = url is null ? null : new()
                {
                    PreviewImageUri = url
                }
            };
        }
    }
    #endregion
    #region 返回渲染参数
    /// <summary>
    /// 返回本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderFileUploadInfo GetRenderInfo()
        => new()
        {
            OnChange = new(this, OnChange),
            UploadTaskInfo = UploadTaskInfo,
            ID = ID
        };
    #endregion
    #endregion
}
