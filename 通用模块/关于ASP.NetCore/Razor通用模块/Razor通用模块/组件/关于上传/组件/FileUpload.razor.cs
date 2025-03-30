namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以用来上传文件
/// </summary>
public sealed partial class FileUpload : ComponentBase, IAsyncDisposable
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
    #region 用来渲染整个组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFileUploadInfo> RenderComponent { get; set; }
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
    #region 级联参数：用来进行上传的参数
    /// <summary>
    /// 获取用来进行上传的参数
    /// </summary>
    [CascadingParameter]
    private UploadFileOptions UploadFileOptions { get; set; } = new();
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        await DisposableObjectURL();
        FileUploadNavigationContext?.CancelUploadTaskInfo(ID);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 是否正在执行选择文件事件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示正在执行选择文件事件，
    /// 注意：上传可能在后续才触发，它不等于触发上传事件
    /// </summary>
    private bool InInvokeOnChange { get; set; }
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
    /// 获取当前正在执行的上传任务
    /// </summary>
    private UploadTaskInfo UploadTaskInfo { get; set; }
    #endregion
    #region 释放文件对象Uri
    /// <summary>
    /// 释放JS文件对象
    /// </summary>
    /// <returns></returns>
    private async Task DisposableObjectURL()
    {
        try
        {
            var blobUri = UploadTaskInfo.AllBlobUri.ToArray();
            if (blobUri.Length > 0)
                await JSWindow.InvokeVoidAsync("DisposableObjectURL", blobUri);
        }
        catch (JSDisconnectedException)
        {
        }
    }
    #endregion
    #region 从剪切板上传的方法
    /// <summary>
    /// 通过读取剪切板的内容上传文件
    /// </summary>
    /// <returns>中途出现的错误提示，
    /// 如果未出现任何错误，则返回<see langword="null"/></returns>
    private async Task<string?> OnUploadFromClipboard()
    {
        var copyContent = await JSWindow.Navigator.Clipboard.ReadObject(true);
        if (copyContent is null)
            return "未能获取复制粘贴权限，或者复制了不支持的内容";
        var clipboardItems = copyContent.ClipboardItems;
        if (clipboardItems.Count is 0)
            return "没有复制任何内容";
        var binaryItems = clipboardItems.OfType<IClipboardItemBinary>().ToArray();
        if (binaryItems.Length is 0)
            return "你复制的是文字，不能用来上传";
        var files = binaryItems.Select(x => x.ToBrowserFile()).ToArray();
        var inputFileChangeEventArgs = new InputFileChangeEventArgs(files);
        await OnChange(inputFileChangeEventArgs);
        return null;
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
        InInvokeOnChange = true;
        this.StateHasChanged();
        try
        {
            await DisposableObjectURL();
            var uploadFile = args.GetMultipleFiles(args.FileCount);
            var (uploadFileInfos, hugeFiles) = await GetUploadFiles(uploadFile);
            UploadTaskInfo = new()
            {
                UploadFiles = uploadFileInfos,
                HugeFiles = hugeFiles,
                UploadFileOptions = UploadFileOptions
            };
            FileUploadNavigationContext?.RegisterUploadTaskInfo(ID, uploadFileInfos);
            await OnUpload(UploadTaskInfo);
        }
        finally
        {
            InInvokeOnChange = false;
        }
    }
    #endregion
    #region 返回待上传的文件
    /// <summary>
    /// 返回所有待上传的文件
    /// </summary>
    /// <param name="browserFiles">原始的，直接从浏览器中读取的待上传文件</param>
    /// <returns>一个元组，它的项分别是可以上传的文件，以及因为大小超过限制，而不能上传的文件</returns>
    private async Task<(IReadOnlyList<IHasUploadFile> UploadFileInfos, IReadOnlyList<IBrowserFile> HugeFiles)> GetUploadFiles(IReadOnlyCollection<IBrowserFile> browserFiles)
    {
        if (browserFiles.Count is 0)
            return ([], []);
        var maxAllowedSize = UploadFileOptions.MaxAllowedSize;
        var (uploadFiles, hugeFiles) = browserFiles.Split(x => x.Size <= maxAllowedSize);
        var uploadFileURLs = await JSWindow.InvokeAsync<string[]>("GetUploadFileURL", ID, maxAllowedSize);
        var upload = uploadFiles.Zip(uploadFileURLs).Select(tuple =>
        {
            var (browserFile, uploadFileURL) = tuple;
            return UploadFileClientFactory(uploadFileURL, uploadFileURL, BrowserFileConvert(browserFile, UploadFileOptions));
        }).ToArray();
        return (upload, hugeFiles);
    }
    #endregion
    #region 重写OnInitialized方法
    protected override void OnInitialized()
    {
        UploadTaskInfo = new()
        {
            UploadFiles = [],
            HugeFiles = [],
            UploadFileOptions = UploadFileOptions
        };
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
            OnUploadFromClipboard = OnUploadFromClipboard,
            InInvokeOnChange = InInvokeOnChange,
            UploadFileOptions = UploadFileOptions,
            ID = ID
        };
    #endregion
    #endregion
}
