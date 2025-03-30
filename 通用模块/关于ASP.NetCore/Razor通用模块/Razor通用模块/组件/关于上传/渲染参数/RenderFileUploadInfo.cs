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
    /// 获取正在进行的上传任务
    /// </summary>
    public required UploadTaskInfo UploadTaskInfo { get; init; }
    #endregion
    #region 通过剪切板进行上传的委托
    /// <summary>
    /// 通过剪切板进行上传的委托，
    /// 它的返回值是中途出现的错误提示，
    /// 如果未出现任何错误，则返回<see langword="null"/>
    /// </summary>
    public required Func<Task<string?>> OnUploadFromClipboard { get; init; }
    #endregion
    #region 是否正在执行选择文件事件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示正在执行选择文件事件，
    /// 注意：上传可能在后续才触发，它不等于触发上传事件
    /// </summary>
    public required bool InInvokeOnChange { get; init; }
    #endregion
    #region 上传文件的选项
    /// <summary>
    /// 获取上传文件的选项
    /// </summary>
    public required UploadFileOptions UploadFileOptions { get; init; }
    #endregion
    #region 上传组件的ID
    /// <summary>
    /// 获取这个上传组件的ID，
    /// 通过它可以找到这个上传组件，
    /// 它必须被赋值给InputFile的id属性
    /// </summary>
    public required string ID { get; init; }
    #endregion
}
