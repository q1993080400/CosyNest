namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="Virtualization{Element}"/>每个元素的参数
/// </summary>
/// <inheritdoc cref="Virtualization{Element}"/>
public sealed record RenderVirtualizationElementInfo<Element>
{
    #region 要渲染的元素
    /// <summary>
    /// 获取要渲染的元素
    /// </summary>
    public required Element RenderElement { get; init; }
    #endregion
    #region 元素的索引
    /// <summary>
    /// 获取这个元素对应的索引
    /// </summary>
    public required int Index { get; init; }
    #endregion
    #region 组件ID
    /// <summary>
    /// 获取为这个元素所对应的组件分配的ID，
    /// 将它分配到Web标签的id属性，
    /// 可以让其他上下文能够找到这个元素，注意：
    /// 它不是实体的ID
    /// </summary>
    public required string ComponentID { get; init; }
    #endregion
}
