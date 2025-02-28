using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
    #region 内部成员
    #region 容器ID
    /// <summary>
    /// 获取容器的ID
    /// </summary>
    private string ID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 下载文件
    /// <summary>
    /// 下载当前正在预览的这个文件
    /// </summary>
    /// <returns></returns>
    private async Task OnDownload()
    {
        await JSWindow.InvokeVoidAsync("DownloadPreviewFile", ID);
    }
    #endregion
    #endregion
}
