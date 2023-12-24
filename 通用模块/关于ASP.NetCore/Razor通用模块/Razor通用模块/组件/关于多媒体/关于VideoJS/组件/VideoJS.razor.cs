namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是使用VideoJS实现的播放器
/// </summary>
public sealed partial class VideoJS : ComponentBase
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取本组件的渲染参数，
    /// 注意：如果这个参数被更改，
    /// 只有对<see cref="RenderVideoJS.Source"/>的更改会得到响应
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderVideoJS RenderVideoJS { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region CSS的路径
    /// <summary>
    /// 获取用来加载CSS文件的路径
    /// </summary>
    public static string CSSUri { get; set; }
        = "https://vjs.zencdn.net/8.5.2/video-js.css";
    #endregion
    #region JS文件的路径
    /// <summary>
    /// 获取用来加载JS文件的路径
    /// </summary>
    public static string JSUri { get; set; }
        = "https://vjs.zencdn.net/8.5.2/video.min.js";
    #endregion
    #endregion
    #region 内部成员
    #region 获取播放器的ID
    /// <summary>
    /// 获取播放器的ID
    /// </summary>
    private string ID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsync()
    {
        await Task.Delay(1);
        var source = RenderVideoJS.Source;
        var audioOnlyMode = RenderVideoJS.AudioOnlyMode ??
            source.All(x => x.MediumType.StartsWith("audio"));
        var options = new
        {
            Autoplay = RenderVideoJS.Autoplay ? "any" : (object)false,
            Sources = source.Select(x =>
            new
            {
                Src = x.Uri,
                Type = x.MediumType
            }).ToArray(),
            AudioOnlyMode = audioOnlyMode,
            RenderVideoJS.Loop,
            RenderVideoJS.Controls,
            RenderVideoJS.Width,
            RenderVideoJS.Height,
            Responsive = true,
            SrcHash = ToolEqual.CreateHash(RenderVideoJS.Source.Select(x => x.Uri).ToArray())
        };
        await JSRuntime.InvokeVoidAsync("InitializationVideoJS", ID, options, CSSUri, JSUri);
    }
    #endregion
    #endregion
}
