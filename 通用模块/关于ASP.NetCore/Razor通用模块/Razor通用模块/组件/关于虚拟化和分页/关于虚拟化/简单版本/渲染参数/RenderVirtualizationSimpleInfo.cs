namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是用来渲染<see cref="VirtualizationSimple{Element}"/>的参数
/// </summary>
/// <typeparam name="Element"><see cref="VirtualizationSimple{Element}"/>的元素的类型</typeparam>
public record RenderVirtualizationSimpleInfo<Element>
{
    #region 用来渲染元素的委托
    /// <inheritdoc cref="RenderVirtualizationInfo{Obj}.RenderElement"/>
    public required IReadOnlyCollection<RenderFragment> RenderElement { get; init; }
    #endregion
    #region 用来跳跃到容器顶端的方法
    /// <inheritdoc cref="RenderVirtualizationContainerInfo.GoTop"/>
    public required EventCallback GoTop { get; init; }
    #endregion
    #region 指示渲染状态
    /// <summary>
    /// 指示虚拟化组件的渲染状态
    /// </summary>
    public required RenderVirtualizationState State { get; init; }
    #endregion
    #region 留白高度
    /// <summary>
    /// 获取留白高度的样式文本，
    /// 它在容器最下方提供一个空白高度，
    /// 这可以使容器变得更加美观
    /// </summary>
    public required string BlankHeight { get; init; }
    #endregion
    #region 渲染空集合的委托
    /// <summary>
    /// 获取一个渲染空集合时的委托
    /// </summary>
    public required RenderFragment RenderEmpty { get; set; }
    #endregion
}
