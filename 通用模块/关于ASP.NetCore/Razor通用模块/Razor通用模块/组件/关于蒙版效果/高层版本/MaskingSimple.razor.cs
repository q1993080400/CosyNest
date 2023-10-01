namespace Microsoft.AspNetCore.Components;

/// <summary>
/// <see cref="Masking"/>的开箱即用版，
/// 可以让放置在其中的组件自动获得蒙版效果
/// </summary>
public sealed partial class MaskingSimple : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 子内容
    [Parameter]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 占满全屏
    /// <inheritdoc cref="Masking.IsFullScreen"/>
    [Parameter]
    public bool IsFullScreen { get; set; }
    #endregion
    #region 蒙版模式
    /// <inheritdoc cref="Masking.MaskingMod"/>
    [Parameter]
    public Func<IMaskingParameter, RenderMasking> MaskingMod { get; set; } = Masking.MaskingAcrylic;
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取容器的参数展开，
    /// 为了使本组件生效，必须指定组件的背景
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
}
