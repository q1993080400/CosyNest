using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是渲染<see cref="BootstrapFileViewer"/>中单个文件的默认方法
/// </summary>
public sealed partial class BootstrapSingleFile : ComponentBase
{
    #region 组件参数
    #region 用来渲染单个文件的参数
    /// <summary>
    /// 用来渲染单个文件的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderSingleFileInfo RenderFileInfo { get; set; }
    #endregion
    #region 是否禁用下载
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则使用div标签渲染容器，
    /// 为<see langword="false"/>，
    /// 则使用a标签渲染容器，它可以提供下载功能，
    /// 为<see langword="null"/>，则自动判断，
    /// 如果为组件提供了onclick事件，
    /// 则使用div标签渲染，否则使用a标签渲染
    /// </summary>
    [Parameter]
    public bool? DisableDownload { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取容器标签的参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }
    #endregion
    #endregion
}
