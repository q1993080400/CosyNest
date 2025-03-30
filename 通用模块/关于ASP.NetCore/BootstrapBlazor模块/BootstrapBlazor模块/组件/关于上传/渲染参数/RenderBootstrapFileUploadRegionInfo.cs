using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个记录是渲染<see cref="BootstrapFileUpload"/>组件上传部分的参数
/// </summary>
public sealed record RenderBootstrapFileUploadRegionInfo
{
    #region 通过剪切板进行上传的委托
    /// <summary>
    /// 通过剪切板进行上传的委托
    /// </summary>
    public required Func<Task> OnUploadFromClipboard { get; init; }
    #endregion
    #region 用来进行上传的参数
    /// <summary>
    /// 获取用来进行上传的参数
    /// </summary>
    public UploadFileOptions UploadFileOptions
        => RenderFileUploadInfo.UploadFileOptions;
    #endregion
    #region 可接受文件类型
    /// <summary>
    /// 获取上传组件可接受的文件类型，
    /// 它的语法和Web中input标签的accept属性相同
    /// </summary>
    public required string? Accept { get; init; }
    #endregion
    #region 是否可选择多个文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示可以选择多个文件，否则仅可选择单个文件
    /// </summary>
    public required bool Multiple { get; init; }
    #endregion
    #region 上传按钮文字
    /// <summary>
    /// 获取上传按钮的文字
    /// </summary>
    public required string UploadButtonText { get; init; }
    #endregion
    #region 是否正在上传
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示上传正在进行中，你可以给予一些提示，
    /// 提醒用户上传完毕前不要离开
    /// </summary>
    public required bool InUpload { get; init; }
    #endregion
    #region 是否显示提示
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则显示一个提示，指示如何删除文件和上传文件的最大大小
    /// </summary>
    public required bool ShowPrompt { get; init; }
    #endregion
    #region 基础版本的渲染参数
    /// <summary>
    /// 基础版本的渲染参数
    /// </summary>
    public required RenderFileUploadInfo RenderFileUploadInfo { get; init; }
    #endregion
}
