using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是渲染<see cref="BootstrapFileViewer"/>中预览部分的默认方法
/// </summary>
public sealed partial class BootstrapFileViewerPreview : ComponentBase
{
    #region 组件参数
    #region 用来渲染预览部分的参数
    /// <summary>
    /// 用来渲染预览部分的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFilePreviewInfo RenderFilePreviewInfo { get; set; }
    #endregion
    #endregion
}
