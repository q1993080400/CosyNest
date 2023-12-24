namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个无外观的音频播放器
/// </summary>
public sealed partial class Audio : ComponentBase, IContentComponent<RenderFragment<RenderAudio>>
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderAudio RenderAudio { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderAudio> ChildContent { get; set; }
    #endregion
    #endregion
}
