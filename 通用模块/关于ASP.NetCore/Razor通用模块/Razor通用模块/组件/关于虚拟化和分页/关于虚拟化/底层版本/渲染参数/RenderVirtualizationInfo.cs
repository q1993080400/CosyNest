namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是用来渲染<see cref="Virtualization{Element}"/>的参数
/// </summary>
/// <typeparam name="Obj"><see cref="Virtualization{Element}"/>的元素的类型</typeparam>
public sealed record RenderVirtualizationInfo<Obj>
{
    #region 是否渲染元素
    /// <summary>
    /// 获取是否渲染至少一个元素
    /// </summary>
    public required bool AnyElement { get; init; }
    #endregion
    #region 渲染所有元素的委托
    /// <summary>
    /// 获取渲染所有元素的委托，
    /// 它的数量比实际渲染的元素数量多一到两个，
    /// 因为在末尾存在一个不可见的占位符
    /// </summary>
    public required IEnumerable<RenderFragment> RenderElement { get; init; }
    #endregion
    #region 渲染空集合时的委托
    /// <summary>
    /// 渲染空集合时的委托
    /// </summary>
    public required RenderFragment RenderEmpty { get; init; }
    #endregion
    #region 自动渲染元素
    /// <summary>
    /// 获取一个自动渲染元素的委托，
    /// 当存在要渲染的元素时，它依次渲染<see cref="RenderElement"/>，
    /// 否则，它渲染<see cref="RenderEmpty"/>
    /// </summary>
    public required RenderFragment RenderElementAuto { get; init; }
    #endregion
    #region 回到顶部的委托
    /// <summary>
    /// 调用这个委托，可以回到容器
    /// </summary>
    public required Func<Task> GoTop { get; init; }
    #endregion
    #region 跳转到顶的元素的ID
    /// <summary>
    /// 获取跳转到顶的元素的ID，
    /// 当调用<see cref="GoTop"/>的时候，会跳转到这个元素身上，
    /// 如果不设置这个属性，则不能使用这个功能
    /// </summary>
    public required string GoTopID { get; init; }
    #endregion
}
