namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件能够以九宫格（或更多）的形式展示媒体，
/// 并支持预览功能
/// </summary>
public sealed partial class MediaViewer : ComponentBase
{
    #region 组件参数
    #region 要展示的媒体
    /// <summary>
    /// 获取或设置要展示的媒体
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<MediaSource> MediaSource { get; set; }
    #endregion
    #region 渲染组件的委托
    /// <summary>
    /// 获取用来渲染组件的委托
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<RenderMediaViewerInfo> RenderComponent { get; set; }
    #endregion
    #region 渲染封面的委托
    /// <summary>
    /// 获取或设置渲染封面的委托
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<RenderCoverInfo> RenderCover { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 正在进行大图预览的媒体的索引
    /// <summary>
    /// 获取正在进行大图预览的媒体的索引，
    /// 如果没有进行大图渲染，则返回-1
    /// </summary>
    private int PreviewMediaIndex { get; set; } = -1;
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取用来渲染这个组件的参数
    /// </summary>
    /// <returns></returns>
    private RenderMediaViewerInfo GetRenderInfo()
    {
        var mediaSource = MediaSource.ToArray();
        #region 本地函数
        Func<Task> Fun(int index)
            => () =>
            {
                PreviewMediaIndex = index;
                return Task.CompletedTask;
            };
        #endregion
        var renderCover = mediaSource.PackIndex().Select(x => new RenderCoverInfo()
        {
            MediaSource = x.Elements,
            PreviewEvent = Fun(x.Index)
        }).Select(x => RenderCover(x)).ToArray();
        return new()
        {
            PreviewMediaIndex = PreviewMediaIndex,
            Medias = mediaSource,
            QuitPreview = Fun(-1),
            RenderElement = renderCover
        };
    }
    #endregion
    #endregion
}
