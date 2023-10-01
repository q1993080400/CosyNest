namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是<see cref="Virtualization{Element}"/>的开箱即用版本，
/// 它可以用来实现虚拟化加载，当滚动到末尾时加载新的内容
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
    #region 渲染末尾的委托
    /// <inheritdoc cref="Virtualization{Element}.RenderEnd"/>
    [Parameter]
    public RenderFragment<string>? RenderEnd { get; set; }
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
    public IAsyncEnumerable<Element> Elements { get; set; }
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
    #region 是否为倒序容器
    /// <inheritdoc cref="Virtualization{Element}.IsReverse"/>
    [Parameter]
    public bool IsReverse { get; set; }
    #endregion
    #endregion
}
