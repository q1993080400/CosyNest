using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是渲染<see cref="BootstrapFileUpload"/>组件上传部分的默认方法
/// </summary>
public sealed partial class BootstrapFileUploadRegion : ComponentBase
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取渲染上传区域的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderBootstrapFileUploadRegionInfo RenderUploadInfo { get; set; }
    #endregion
    #endregion
}
