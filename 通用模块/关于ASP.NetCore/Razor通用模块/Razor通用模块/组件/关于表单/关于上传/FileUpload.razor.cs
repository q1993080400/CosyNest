using System.NetFrancis;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 该组件可以用于上传文件，
/// 它在UI上不显示，上传对话框的样式由其他组件提供
/// </summary>
public sealed partial class FileUpload : ComponentBase, IContentComponent<RenderFragment>, IDisposable
{
    #region 组件参数
    #region 选择文件时触发的事件
    /// <summary>
    /// 获取或设置选择文件时触发的事件，
    /// 它的第一个参数是被选择的文件，
    /// 第二个参数是一个用于取消异步操作的令牌，
    /// 返回值是文件是否上传成功
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<IReadOnlyList<IBrowserFile>, CancellationToken, Task<bool>> OnSelectFile { get; set; }
    #endregion
    #region 子内容
    /// <summary>
    /// 获取组件的子内容，
    /// 它允许自定义组件的样式
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment? ChildContent { get; set; }
    #endregion
    #region 是否应该阻止导航
    /// <summary>
    /// 获取是否应该阻止导航，
    /// 它可以防止上传过程中，
    /// 因为离开页面所导致的上传失败，
    /// 它的参数就是目前是否有文件正在上传
    /// </summary>
    [Parameter]
    public Func<bool, Task<bool>> BlockNavigation { get; set; } = _ => Task.FromResult(false);
    #endregion
    #region 当上传状态改变时触发的事件
    /// <summary>
    /// 当上传状态改变的时候，触发这个事件，
    /// 它的参数就是新的上传状态
    /// </summary>
    [Parameter]
    public EventCallback<UploadStatus> OnUploadStatusChangeed { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个字典为上传组件提供参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public void Dispose()
    {
        Cancel();
        CancellationTokenSource.Dispose();
    }
    #endregion
    #region 取消上传
    /// <summary>
    /// 取消上传文件
    /// </summary>
    /// <returns>指示在取消上传之前的文件上传状态，
    /// 通过检查它，可以执行一些清理操作</returns>
    public UploadStatus Cancel()
    {
        var uploadStatus = UploadStatus;
        CancellationTokenSource.Cancel();
        return uploadStatus;
    }
    #endregion
    #region 文件上传状态
    /// <summary>
    /// 获取当前文件上传状态
    /// </summary>
    public UploadStatus UploadStatus { get; private set; }
    #endregion
    #endregion
    #region 内部成员
    #region 用于取消异步操作的令牌
    /// <summary>
    /// 这个对象可以用于提供取消异步操作的令牌
    /// </summary>
    private CancellationTokenSource CancellationTokenSource { get; } = new();
    #endregion
    #region 关于选择文件
    #region 选择文件时执行的方法
    /// <summary>
    /// 选择文件时执行的方法
    /// </summary>
    /// <param name="args">用来描述被选择文件的对象</param>
    private async Task OnChange(InputFileChangeEventArgs args)
    {
        var files = args.GetMultipleFiles(args.FileCount);
        UploadStatus = UploadStatus.Uploading;
        await OnUploadStatusChangeed.InvokeAsync(UploadStatus);
        try
        {
            var isSuccess = await OnSelectFile(files, CancellationTokenSource.Token);
            UploadStatus = isSuccess ? UploadStatus.UploadCompleted : UploadStatus.UploadFailed;
        }
        catch (Exception ex)
        {
            UploadStatus = UploadStatus.UploadFailed;
            if (ex is not OperationCanceledException)
                throw;
        }
        await OnUploadStatusChangeed.InvokeAsync(UploadStatus);
    }
    #endregion
    #endregion
    #endregion
}
