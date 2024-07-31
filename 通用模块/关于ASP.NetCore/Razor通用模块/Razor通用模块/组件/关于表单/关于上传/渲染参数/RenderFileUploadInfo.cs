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
    public required Func<InputFileChangeEventArgs, Task> OnChange { get; init; }
    #endregion
    #region 是否正在上传中
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示正在执行一个未完成的上传操作
    /// </summary>
    public required bool InUpload { get; init; }
    #endregion
    #region 防止重复上传的CSS
    /// <summary>
    /// 这个属性返回一个CSS类名，
    /// 如果上传操作正在进行，
    /// 通过它可以禁用组件的点击事件，防止重复上传
    /// </summary>
    public string PreventDuplicationCSS
        => InUpload ? "prohibitClicking" : "";
    #endregion
}
