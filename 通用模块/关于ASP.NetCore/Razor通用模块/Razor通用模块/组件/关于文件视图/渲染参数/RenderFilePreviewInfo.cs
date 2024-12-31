namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FileViewer"/>组件预览部分的参数
/// </summary>
public sealed record RenderFilePreviewInfo
{
    #region 预览文件的集合
    /// <summary>
    /// 假设从本文件开始预览文件，
    /// 则依次返回应该预览的文件列表
    /// </summary>
    public required IEnumerable<IHasReadOnlyPreviewFile> PreviewFile { get; init; }
    #endregion
    #region 用于关闭预览的委托
    /// <summary>
    /// 这个委托可以用来退出预览文件的状态
    /// </summary>
    public required EventCallback ClosePreview { get; init; }
    #endregion
}
