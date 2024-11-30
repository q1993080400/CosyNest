namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FileViewer"/>组件的参数
/// </summary>
public sealed record RenderFileViewerInfo
{
    #region 渲染所有文件的委托
    /// <summary>
    /// 这个集合枚举每一个渲染单个文件的委托
    /// </summary>
    public required IReadOnlyCollection<RenderFragment> RenderAllFile { get; init; }
    #endregion
}
