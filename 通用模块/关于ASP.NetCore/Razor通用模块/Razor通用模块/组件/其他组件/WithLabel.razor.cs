namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件由一个文本标签和控件组成
/// </summary>
public sealed partial class WithLabel : ComponentBase, IContentComponent<RenderFragment<Guid>>
{
    #region 组件参数
    #region 组件的主体部分
    /// <summary>
    /// 这个委托的参数是可以用来进行关联的<see cref="Guid"/>，
    /// 它被用来渲染组件的主体部分
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<Guid>? ChildContent { get; set; }
    #endregion
    #region 标签的文本
    /// <summary>
    /// 标签的文本
    /// </summary>
    [EditorRequired]
    [Parameter]
    public string Label { get; set; }
    #endregion
    #region 标签的CSS样式
    /// <summary>
    /// 标签的CSS样式
    /// </summary>
    [Parameter]
    public string? LabelCSS { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取参数展开，它适用于组件的容器部分
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }
    #endregion
    #endregion
}
