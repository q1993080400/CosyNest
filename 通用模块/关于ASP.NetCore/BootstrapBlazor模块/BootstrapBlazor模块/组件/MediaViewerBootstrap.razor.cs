using Microsoft.AspNetCore.Components;

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
    /// 获取或设置要展示的媒体
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<MediaSource> MediaSource { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 捕获的Modal对象
    /// <summary>
    /// 获取捕获的<see cref="Components.Modal"/>对象
    /// </summary>
    private Modal? Modal { get; set; }
    #endregion
    #region OnTimeUpdate事件的脚本
    /// <summary>
    /// 返回当触发OnTimeUpdate事件时，要执行的脚本
    /// </summary>
    /// <param name="id">播放器标签的ID</param>
    /// <returns></returns>
    private static string OnTimeUpDate(string id)
        => $$"""
        var play=document.querySelector('div.active #{{id}}');
        if(play!=null)
        {
            play.muted=false;
        }
        else
        {
           var video= document.getElementById('{{id}}');
           if(video!=null)
               video.muted=true;
        }
        """;
    #endregion
    #region OnLoadedMetaData事件的脚本
    /// <summary>
    /// 返回当触发OnLoadedMetaData事件时，要执行的脚本
    /// </summary>
    /// <param name="id">播放器标签的ID</param>
    /// <returns></returns>
    private static string OnLoadedMetaData(string id)
        => $$"""
        var video=document.getElementById('{{id}}');
        video.muted=true;
        """;
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
        var medias = info.Medias.ToArray();
        return medias[index..].Concat(medias[..index]).ToArray();
    }
    #endregion
    #endregion
}
