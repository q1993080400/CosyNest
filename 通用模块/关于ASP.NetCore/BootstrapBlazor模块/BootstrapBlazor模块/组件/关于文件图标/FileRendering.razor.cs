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
    #region 高亮文本
    /// <summary>
    /// 获取高亮文本的集合
    /// </summary>
    [Parameter]
    public IReadOnlyCollection<string>? Highlight { get; set; }
    #endregion
    #region 高亮组
    /// <summary>
    /// 获取高亮组，
    /// 通过指定它，可以为高亮指定不同的样式
    /// </summary>
    [Parameter]
    public string HighlightGroup { get; set; } = "highlightGroup";
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
