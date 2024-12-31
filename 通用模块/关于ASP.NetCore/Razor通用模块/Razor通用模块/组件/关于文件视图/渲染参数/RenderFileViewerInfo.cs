namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FileViewer"/>组件的参数
/// </summary>
public sealed record RenderFileViewerInfo
{
    #region 渲染每个文件的委托
    /// <summary>
    /// 这个集合枚举每一个渲染单个文件的委托
    /// </summary>
    public required IReadOnlyCollection<RenderFragment> RenderFiles { get; init; }
    #endregion
    #region 渲染所有文件的委托
    /// <summary>
    /// 获取一个委托，
    /// 它依次渲染<see cref="RenderFiles"/>中的所有文件
    /// </summary>
    public RenderFragment RenderAllFile
        => RenderFiles.MergeRender();
    #endregion
    #region 渲染预览文件的委托
    /// <summary>
    /// 获取用来渲染预览文件的委托
    /// </summary>
    public required RenderFragment RenderFilePreview { get; init; }
    #endregion
}
