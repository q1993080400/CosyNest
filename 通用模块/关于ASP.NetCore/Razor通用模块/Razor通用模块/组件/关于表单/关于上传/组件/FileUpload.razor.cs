
namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以用来上传文件
/// </summary>
public sealed partial class FileUpload : ComponentBase, IContentComponent<RenderFragment<RenderFileUploadInfo>>
{
    #region 组件参数
    #region 用来上传文件的委托
    /// <summary>
    /// 这个委托可以用来上传文件，
    /// 它的返回值是是否上传成功
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<OnSelectFileEventArgs, Task<bool>> OnSelectFile { get; set; }
    #endregion
    #region 上传状态改变时的委托
    /// <summary>
    /// 当上传状态改变时，触发这个委托，
    /// 它的参数就是新的上传状态
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<FileUploadStatus, Task> OnUploadStatusChange { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFileUploadInfo> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 是否正在上传
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示存在尚未完成的上传进度
    /// </summary>
    private bool OnUpload { get; set; }
    #endregion
    #region 当选择文件时触发的事件
    /// <summary>
    /// 当选择文件时，触发这个事件
    /// </summary>
    /// <param name="args">选择文件的参数</param>
    /// <returns></returns>
    private async Task OnChange(InputFileChangeEventArgs args)
    {
        var fileCount = args.FileCount;
        if (fileCount is 0)
            return;
        var files = args.GetMultipleFiles(fileCount);
        OnUpload = true;
        this.StateHasChanged();
        try
        {
            await OnUploadStatusChange(FileUploadStatus.Uploading);
            var info = new OnSelectFileEventArgs()
            {
                SelectFiles = files
            };
            var isSuccess = await OnSelectFile(info);
            await OnUploadStatusChange(isSuccess ? FileUploadStatus.UploadCompleted : FileUploadStatus.UploadFailed);
        }
        catch (Exception)
        {
            await OnUploadStatusChange(FileUploadStatus.UploadFailed);
            throw;
        }
        finally
        {
            OnUpload = false;
            this.StateHasChanged();
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
            OnChange = OnChange,
            InUpload = OnUpload
        };
    #endregion
    #endregion
}
