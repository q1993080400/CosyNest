namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录被用来充当虚拟化组件的渲染参数
/// </summary>
/// <typeparam name="Obj">要渲染的元素的类型</typeparam>
public record VirtualizationRender<Obj>
{
    #region 要渲染的元素
    /// <summary>
    /// 要渲染的元素
    /// </summary>
    public required Obj Element { get; init; }
    #endregion
    #region 元素的索引
    /// <summary>
    /// 元素在集合中的索引
    /// </summary>
    public required int Index { get; init; }
    #endregion
    #region 元素的ID
    /// <summary>
    /// 元素的ID，通过它，
    /// JS可以找到这个元素
    /// </summary>
    public required string ID { get; init; }
    #endregion
}
