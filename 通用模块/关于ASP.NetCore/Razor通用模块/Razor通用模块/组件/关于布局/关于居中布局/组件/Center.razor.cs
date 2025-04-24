namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件是一个居中布局，
/// 它自动将子内容放在中部居中位置
/// </summary>
public sealed partial class Center : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 居中模式
    /// <summary>
    /// 居中模式，
    /// 它指示如何居中，相对什么居中
    /// </summary>
    [Parameter]
    public CenterMod CenterMod { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 额外CSS
    /// <summary>
    /// 获取或设置额外的CSS类名，
    /// 它可以给组件增加其他的样式
    /// </summary>
    [Parameter]
    public string? AdditionalCSS { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 该字典指示封装九宫格的容器的特性
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
}
