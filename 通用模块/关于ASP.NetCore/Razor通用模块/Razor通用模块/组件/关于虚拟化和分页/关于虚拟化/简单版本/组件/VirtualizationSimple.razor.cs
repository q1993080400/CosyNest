namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是虚拟化组件的开箱即用版本
/// </summary>
/// <inheritdoc cref="Virtualization{Element}"/>
public sealed partial class VirtualizationSimple<Element> : ComponentBase
{
    #region 组件参数
    #region 渲染每个元素的委托
    /// <inheritdoc cref="Virtualization{Element}.RenderElement"/>
    [EditorRequired]
    [Parameter]
    public RenderFragment<RenderVirtualizationElementInfo<Element>> RenderElement { get; set; }
    #endregion
    #region 渲染组件的委托
    /// <inheritdoc cref="Virtualization{Element}.RenderComponent"/>
    [Parameter]
    public RenderFragment<RenderVirtualizationSimpleInfo<Element>>? RenderComponent { get; set; }
    #endregion
    #region 渲染空集合时的委托
    /// <inheritdoc cref="Virtualization{Element}.RenderEmpty"/>
    [Parameter]
    public RenderFragment RenderEmpty { get; set; } = _ => { };
    #endregion
    #region 枚举元素的迭代器
    /// <inheritdoc cref="Virtualization{Element}.Elements"/>
    [EditorRequired]
    [Parameter]
    public IAsyncEnumerator<Element> Elements { get; set; }
    #endregion
    #region 每次渲染增加的数量
    /// <inheritdoc cref="Virtualization{Element}.Plus"/>
    [Parameter]
    public int Plus { get; set; } = 5;
    #endregion
    #region 初始渲染的数量
    /// <inheritdoc cref="Virtualization{Element}.Initial"/>
    [Parameter]
    public int Initial { get; set; } = 35;
    #endregion
    #region 跳跃到指定位置
    /// <inheritdoc cref="Virtualization{Element}.Jump"/>
    [Parameter]
    public int? Jump { get; set; }
    #endregion
    #region 是否为倒序容器
    /// <inheritdoc cref="Virtualization{Element}.IsReverse"/>
    [Parameter]
    public bool IsReverse { get; set; }
    #endregion
    #region 留白高度
    /// <inheritdoc cref="Virtualization{Element}.BlankHeight"/>
    [Parameter]
    public string BlankHeight { get; set; } = "margin-top:15dvh;";
    #endregion
    #region 参数展开
    /// <summary>
    /// 该参数展开控制父div容器的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
}
