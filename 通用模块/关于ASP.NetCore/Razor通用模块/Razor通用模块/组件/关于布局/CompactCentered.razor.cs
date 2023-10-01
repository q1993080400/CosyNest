namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件提供紧凑居中布局，
/// 它通常适用于按钮等情况
/// </summary>
public sealed partial class CompactCentered : ComponentBase
{
    #region 组件参数
    #region 子内容
    /// <summary>
    /// 获取组件的子内容
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    #endregion
    #region 不要填充父容器
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则不填充父容器，否则将父容器填满
    /// </summary>
    [Parameter]
    public bool NotFill { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个参数展开控制容器的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
}
