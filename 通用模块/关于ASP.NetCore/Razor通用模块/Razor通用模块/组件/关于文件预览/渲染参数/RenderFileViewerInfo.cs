namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FileViewer"/>所需要的参数
/// </summary>
public sealed record RenderFileViewerInfo
{
    #region 渲染元素的委托
    /// <summary>
    /// 这个集合中的元素是用来渲染单个媒体封面的委托
    /// </summary>
    public required IEnumerable<RenderFragment> RenderElement { get; init; }
    #endregion
    #region 正在进行大图预览的文件
    /// <summary>
    /// 获取正在进行大图预览的文件，
    /// 如果没有进行大图预览，则返回<see langword="null"/>
    /// </summary>
    public FileSource? PreviewFile
        => PreviewFileIndex < 0 ? null : Files[PreviewFileIndex];
    #endregion
    #region 所有文件
    /// <summary>
    /// 获取要渲染的所有文件
    /// </summary>
    public required IReadOnlyList<FileSource> Files { get; init; }
    #endregion
    #region 正在大图预览的文件索引
    /// <summary>
    /// 正在大图预览的文件索引，
    /// 如果没有大图预览，则返回-1
    /// </summary>
    public required int PreviewFileIndex { get; init; } = -1;
    #endregion
    #region 退出大图预览的委托
    /// <summary>
    /// 这个委托用于退出大图预览
    /// </summary>
    public required EventCallback QuitPreview { get; init; }
    #endregion
}
