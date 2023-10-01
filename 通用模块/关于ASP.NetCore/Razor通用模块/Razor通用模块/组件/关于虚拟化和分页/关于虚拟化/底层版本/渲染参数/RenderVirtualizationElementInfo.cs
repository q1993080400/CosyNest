namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录被用来充当渲染虚拟化组件的元素的参数
/// </summary>
/// <typeparam name="Obj">要渲染的元素的类型</typeparam>
public record RenderVirtualizationElementInfo<Obj>
{
    #region 要渲染的元素
    /// <summary>
    /// 获取要渲染的元素
    /// </summary>
    public required Obj Element { get; init; }
    #endregion
    #region 元素的索引
    /// <summary>
    /// 获取这个元素的索引
    /// </summary>
    public required int Index { get; init; }
    #endregion
    #region 元素ID
    /// <summary>
    /// 获取分配给这个元素的ID，
    /// 通过这个，可以找到这个元素，
    /// 必须将其赋值给呈现元素的容器
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region 用来删除元素的委托
    /// <summary>
    /// 获取可以用来删除这个元素的委托
    /// </summary>
    public required EventCallback Delete { get; init; }
    #endregion
}
