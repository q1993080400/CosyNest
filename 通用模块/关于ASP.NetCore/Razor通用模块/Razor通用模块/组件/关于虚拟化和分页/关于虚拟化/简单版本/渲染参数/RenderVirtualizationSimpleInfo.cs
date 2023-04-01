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
    /// <inheritdoc cref="RenderContainerInfo.GoTop"/>
    public required EventCallback GoTop { get; init; }
    #endregion
}
