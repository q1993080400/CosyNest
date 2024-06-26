﻿namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件能够以九宫格（或更多更少）的形式展示媒体或文件，
/// 并支持预览功能
/// </summary>
public sealed partial class FileViewer : ComponentBase
{
    #region 组件参数
    #region 要展示的媒体或文件
    /// <summary>
    /// 获取或设置要展示的媒体或文件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<FileSource> FileSource { get; set; }
    #endregion
    #region 渲染组件的委托
    /// <summary>
    /// 获取用来渲染组件的委托
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<RenderFileViewerInfo> RenderComponent { get; set; }
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
    private RenderFileViewerInfo GetRenderInfo()
    {
        var fileSource = FileSource.ToArray();
        #region 本地函数
        EventCallback Fun(int index)
            => new(this, () =>
            {
                PreviewMediaIndex = index;
                return Task.CompletedTask;
            });
        #endregion
        var renderCover = fileSource.PackIndex().Select(x => new RenderCoverInfo()
        {
            FileSource = x.Elements,
            PreviewEvent = Fun(x.Index)
        }).Select(x => RenderCover(x)).ToArray();
        return new()
        {
            PreviewFileIndex = PreviewMediaIndex,
            Files = fileSource,
            QuitPreview = Fun(-1),
            RenderElement = renderCover
        };
    }
    #endregion
    #endregion
}
