using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件能够渲染一个文件图标，
/// 并提供文件下载功能
/// </summary>
public sealed partial class FileRendering : ComponentBase
{
    #region 组件参数
    #region 文件Uri
    /// <summary>
    /// 获取文件的Uri
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Uri { get; set; }
    #endregion
    #region 文件名
    /// <summary>
    /// 获取文件的名称，
    /// 如果为<see langword="null"/>，
    /// 则从<see cref="Uri"/>中获取名称
    /// </summary>
    [Parameter]
    public string? FileName { get; set; }
    #endregion
    #region 是否禁用下载
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则使用div标签渲染容器，
    /// 否则使用a标签渲染容器，它可以提供下载功能
    /// </summary>
    [Parameter]
    public bool DisableDownload { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取最外层a标签的参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }
    #endregion
    #endregion
}
