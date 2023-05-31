using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是<see cref="MediaViewer"/>的开箱即用版，
/// 它支持通过九宫格（或更多）的方式预览媒体，
/// 并支持滑动等功能
/// </summary>
public sealed partial class MediaViewerBootstrap : ComponentBase
{
    #region 组件参数
    #region 要展示的媒体
    /// <summary>
    /// 获取或设置要展示的媒体，
    /// 如果媒体中存在非图片非视频的文件，则会忽略掉它们
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<MediaSource> MediaSource { get; set; }
    #endregion
    #region 用来渲染组件的委托
    /// <summary>
    /// 获取用来渲染组件主体部分的委托，
    /// 它的参数是一个集合，集合的元素是渲染每一个封面的委托，
    /// 如果为<see langword="null"/>，则使用一个默认方法
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<RenderFragment>>? RenderComponent { get; set; }
    #endregion
    #region 渲染封面的委托
    /// <summary>
    /// 获取或设置渲染封面的委托，
    /// 如果为<see langword="null"/>，
    /// 则使用一个默认方法
    /// </summary>
    [Parameter]
    public RenderFragment<RenderCoverInfo>? RenderCover { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 捕获的Modal对象
    /// <summary>
    /// 获取捕获的<see cref="Components.Modal"/>对象
    /// </summary>
    private Modal? Modal { get; set; }
    #endregion
    #region 跑马灯ID
    /// <summary>
    /// 获取跑马灯的ID，
    /// 它被用来搜索video，
    /// 来实现视频的自动播放和暂停
    /// </summary>
    private string CarouselID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 获取要渲染的媒体
    /// <summary>
    /// 枚举要渲染的媒体，
    /// 它以正在渲染的那个媒体作为起始顺序
    /// </summary>
    /// <param name="info">用来渲染组件的参数</param>
    /// <returns></returns>
    private static IEnumerable<MediaSource> Medias(RenderMediaViewerInfo info)
    {
        var index = info.PreviewMediaIndex;
        if (index < 0)
            return Array.Empty<MediaSource>();
        var medias = info.Medias.ToArray();
        return medias[index..].Concat(medias[..index]).Where(x => x.MediaSourceType is not MediaSourceType.File).ToArray();
    }
    #endregion
    #region 转换封面渲染参数
    /// <summary>
    /// 转换封面渲染参数，使其能够支持本组件所需要的一些功能
    /// </summary>
    /// <param name="info">待转换的封面渲染参数</param>
    /// <returns></returns>
    private RenderCoverInfo ConvertRenderCoverInfo(RenderCoverInfo info)
        => info with
        {
            PreviewEvent = async () =>
            {
                await info.PreviewEvent();
                this.StateHasChanged();
                await Task.Delay(100);
                await Modal!.Show();
                await JSWindow.InvokeVoidAsync("ObserverMediaViewer", new[] { CarouselID });
            }
        };
    #endregion
    #endregion
}
