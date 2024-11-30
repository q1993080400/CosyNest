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
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFileUploadInfo> ChildContent { get; set; }
    #endregion
    #region 级联参数：上传上下文
    /// <summary>
    /// 获取级联的上传上下文，
    /// 当本组件存在正在上传的任务的时候，
    /// 会阻止用户离开页面，
    /// 它可以避免用户在尚未上传完毕时离开页面
    /// </summary>
    [CascadingParameter]
    private IFileUploadNavigationContext? FileUploadNavigationContext { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        await UploadTaskInfo.DisposeUploadFileObjectURL(JSWindow);
        FileUploadNavigationContext?.CancelUploadTaskInfo(ID);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 上传组件的ID
    /// <summary>
    /// 获取这个上传组件的ID，
    /// 通过它可以找到这个上传组件
    /// </summary>
    private string ID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 当前上传任务
    /// <summary>
    /// 获取当前正在执行的上传任务
    /// </summary>
    private UploadTaskInfo UploadTaskInfo { get; set; } = new([]);
    #endregion
    #region 当选择文件时触发的事件
    /// <summary>
    /// 当选择文件时，触发这个事件
    /// </summary>
    /// <param name="args">选择文件的参数</param>
    /// <returns></returns>
    private async Task OnChange(InputFileChangeEventArgs args)
    {
        await UploadTaskInfo.DisposeUploadFileObjectURL(JSWindow);
        var uploadFile = args.GetMultipleFiles(args.FileCount);
        var uploadFileInfos = await GetUploadFileInfo(uploadFile).ToArrayAsync();
        UploadTaskInfo = new(uploadFileInfos);
        FileUploadNavigationContext?.RegisterUploadTaskInfo(ID, UploadTaskInfo);
        await OnUpload(UploadTaskInfo);
    }
    #endregion
    #region 返回待上传的文件
    /// <summary>
    /// 返回所有待上传的文件
    /// </summary>
    /// <param name="browserFiles">原始的，直接从浏览器中读取的待上传文件</param>
    /// <returns></returns>
    private async IAsyncEnumerable<IHasUploadFile> GetUploadFileInfo(IReadOnlyCollection<IBrowserFile> browserFiles)
    {
        if (browserFiles.Count is 0)
            yield break;
        var uploadFileURLs = await JSWindow.InvokeAsync<string[]>("GetUploadFileURL", ID);
        foreach (var (browserFile, uploadFileURL) in browserFiles.Zip(uploadFileURLs))
        {
            var uploadFile = CreateDataObj.UploadFile(uploadFileURL, uploadFileURL, browserFile.ToUploadFile());
            yield return uploadFile;
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
