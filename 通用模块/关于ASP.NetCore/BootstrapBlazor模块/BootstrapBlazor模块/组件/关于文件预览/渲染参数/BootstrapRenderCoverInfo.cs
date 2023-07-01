using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个记录是渲染<see cref="BootstrapFileViewer"/>的封面时所专用的渲染参数
/// </summary>
public sealed record BootstrapRenderCoverInfo
{
    #region 基础渲染参数
    /// <summary>
    /// 获取基础版本的渲染参数
    /// </summary>
    public required RenderCoverInfo RenderCoverInfo { get; init; }
    #endregion
    #region 封面大小
    /// <inheritdoc cref="BootstrapFileViewer.CoverSize"/>
    public string? CoverSize { get; set; }
    #endregion
}
