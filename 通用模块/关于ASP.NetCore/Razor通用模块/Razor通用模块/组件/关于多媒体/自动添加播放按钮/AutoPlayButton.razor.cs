namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件自动为图片添加一个播放按钮，
/// 表示它是一个视频
/// </summary>
public sealed partial class AutoPlayButton : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 子内容
    [EditorRequired]
    [Parameter]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 是否添加播放按钮
    /// <summary>
    /// 获取是否添加播放按钮
    /// </summary>
    [Parameter]
    public bool AddPlayButton { get; set; } = true;
    #endregion
    #region 按钮的额外CSS
    /// <summary>
    /// 按钮的额外CSS
    /// </summary>
    [Parameter]
    public string? ExtraCSSButton { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取容器的参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
}
