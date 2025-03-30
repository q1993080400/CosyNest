using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是渲染<see cref="BootstrapFileUpload"/>组件预览部分的默认方法
/// </summary>
public sealed partial class BootstrapFileUploadPreview : ComponentBase
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取渲染预览区域的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderBootstrapFileUploadPreviewInfo RenderPreviewInfo { get; set; }
    #endregion
    #endregion
}
