namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是用来渲染<see cref="Virtualization{Element}"/>的参数
/// </summary>
/// <inheritdoc cref="Virtualization{Element}"/>
public sealed record RenderVirtualizationInfo<Element>
{
    #region 用来渲染每个元素的参数
    /// <summary>
    /// 这个集合枚举用来渲染每个元素的参数
    /// </summary>
    public required IReadOnlyCollection<RenderVirtualizationElementInfo<Element>> RenderElementInfo { get; init; }
    #endregion
    #region 要渲染的每个元素
    /// <summary>
    /// 获取要渲染的每个元素，
    /// 它仅包含元素，不包含其他对象
    /// </summary>
    public IReadOnlyCollection<Element> RenderElements
        => RenderElementInfo.Select(x => x.RenderElement).ToArray();
    #endregion
    #region 用来渲染加载点的委托
    /// <summary>
    /// 获取用来渲染加载点的委托，
    /// 当用户看到这个元素的时候，
    /// 自动加载新的数据，
    /// 为使组件正常工作，必须渲染它
    /// </summary>
    public required RenderFragment RenderLoadingPoint { get; init; }
    #endregion
    #region 用来渲染末尾的参数
    /// <summary>
    /// 获取用来渲染末尾的参数，
    /// 如果你有比较特殊的需求，
    /// 光靠<see cref="RenderLoadingPoint"/>无法实现，
    /// 这个属性可以帮助你
    /// </summary>
    public required RenderVirtualizationEndInfo RenderEndInfo { get; init; }
    #endregion
    #region 枚举状态
    /// <summary>
    /// 获取虚拟化组件的异步集合的枚举状态
    /// </summary>
    public VirtualizationEnumerableState EnumerableState
        => RenderEndInfo.EnumerableState;
    #endregion
}
