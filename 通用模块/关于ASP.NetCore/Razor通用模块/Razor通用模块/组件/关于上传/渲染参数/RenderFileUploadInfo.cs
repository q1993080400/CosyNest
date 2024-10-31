namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是<see cref="FileUpload"/>的渲染参数
/// </summary>
public sealed record RenderFileUploadInfo
{
    #region 选择文件时触发的事件
    /// <summary>
    /// 获取选择文件时触发的事件
    /// </summary>
    public required EventCallback<InputFileChangeEventArgs> OnChange { get; init; }
    #endregion
    #region 上传任务
    /// <summary>
    /// 获取正在进行的上传任务，
    /// 如果没有上传任务，则为<see langword="null"/>
    /// </summary>
    public required UploadTaskInfo? UploadTaskInfo { get; init; }
    #endregion
    #region 上传组件的ID
    /// <summary>
    /// 获取这个上传组件的ID，
    /// 通过它可以找到这个上传组件
    /// </summary>
    public required string ID { get; init; }
    #endregion
}
