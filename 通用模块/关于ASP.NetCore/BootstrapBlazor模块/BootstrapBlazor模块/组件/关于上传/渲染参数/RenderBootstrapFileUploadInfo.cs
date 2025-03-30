using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个记录是渲染<see cref="BootstrapFileUpload"/>组件的参数
/// </summary>
public sealed record RenderBootstrapFileUploadInfo
{
    #region 用来渲染预览部分的参数
    /// <summary>
    /// 获取用来上传预览部分的参数
    /// </summary>
    public required RenderFragment RenderPreview { get; init; }
    #endregion
    #region 获取用来渲染上传部分的参数
    /// <summary>
    /// 获取用来渲染上传部分的参数
    /// </summary>
    public required RenderFragment RenderUpload { get; init; }
    #endregion
}
