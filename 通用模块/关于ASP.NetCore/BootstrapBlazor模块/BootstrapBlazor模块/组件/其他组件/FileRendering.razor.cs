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
    #region 宽度
    /// <summary>
    /// 获取描述宽度的样式（不是CSS类名），
    /// 如果为<see langword="null"/>，则自动确定
    /// </summary>
    [Parameter]
    public string? Width { get; set; }
    #endregion
    #region 用来获取文件下载名称的委托
    /// <summary>
    /// 这个委托传入原始文件名，
    /// 返回新的文件名，它可以用来作为下载的文件的名称，
    /// 如果为<see langword="null"/>，则使用默认方法
    /// </summary>
    [Parameter]
    public Func<string, string>? GetFileName { get; set; }
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
