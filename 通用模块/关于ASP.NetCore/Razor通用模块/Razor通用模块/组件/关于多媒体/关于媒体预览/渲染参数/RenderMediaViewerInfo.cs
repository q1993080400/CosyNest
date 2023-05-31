namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="MediaViewer"/>所需要的参数
/// </summary>
public sealed record RenderMediaViewerInfo
{
    #region 渲染元素的委托
    /// <summary>
    /// 这个集合中的元素是用来渲染单个媒体封面的委托
    /// </summary>
    public required IEnumerable<RenderFragment> RenderElement { get; init; }
    #endregion
    #region 正在进行大图预览的媒体
    /// <summary>
    /// 获取正在进行大图预览的媒体，
    /// 如果没有进行大图预览，则返回<see langword="null"/>
    /// </summary>
    public MediaSource? PreviewMedia
        => PreviewMediaIndex < 0 ? null : Medias[PreviewMediaIndex];
    #endregion
    #region 所有媒体
    /// <summary>
    /// 获取要渲染的所有媒体
    /// </summary>
    public required IReadOnlyList<MediaSource> Medias { get; init; }
    #endregion
    #region 正在大图预览的媒体索引
    /// <summary>
    /// 正在大图预览的媒体索引，
    /// 如果没有大图预览，则返回-1
    /// </summary>
    public required int PreviewMediaIndex { get; init; } = -1;
    #endregion
    #region 退出大图预览的委托
    /// <summary>
    /// 这个委托用于退出大图预览
    /// </summary>
    public required Func<Task> QuitPreview { get; init; }
    #endregion
}
