namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是用来渲染<see cref="Virtualization{Element}"/>的参数
/// </summary>
/// <typeparam name="Obj"><see cref="Virtualization{Element}"/>的元素的类型</typeparam>
public sealed record RenderVirtualizationInfo<Obj>
{
    #region 用来渲染元素的委托
    /// <summary>
    /// 这个集合的元素是用来渲染每一个元素的委托
    /// </summary>
    public required IReadOnlyCollection<RenderFragment> RenderElement { get; init; }
    #endregion
    #region 用来渲染容器的参数
    /// <summary>
    /// 用来渲染容器的参数
    /// </summary>
    public required RenderVirtualizationContainerInfo RenderContainer { get; init; }
    #endregion
}
