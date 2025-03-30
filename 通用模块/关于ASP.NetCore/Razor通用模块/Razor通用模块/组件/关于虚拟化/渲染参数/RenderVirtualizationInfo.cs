using System.Diagnostics.CodeAnalysis;

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
    [field: AllowNull]
    public IReadOnlyCollection<RenderVirtualizationElementInfo<Element>> RenderElementInfo
        => field ??= GenerateRenderElementInfo(RenderElements);
    #endregion
    #region 要渲染的每个元素
    /// <summary>
    /// 获取要渲染的每个元素
    /// </summary>
    public required IReadOnlyCollection<Element> RenderElements { get; init; }
    #endregion
    #region 转换渲染参数的委托
    /// <summary>
    /// 这个委托传入要渲染的元素，
    /// 返回渲染参数的集合
    /// </summary>
    public required Func<IEnumerable<Element>, IReadOnlyCollection<RenderVirtualizationElementInfo<Element>>> GenerateRenderElementInfo { get; init; }
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
    #region 对偶元素的ID
    /// <summary>
    /// 获取对偶元素的ID，
    /// 对偶元素是和末尾元素成双成对的一个元素，
    /// 它们分别位于容器的两端
    /// </summary>
    public required string PairingID { get; init; }
    #endregion
    #region 枚举状态
    /// <summary>
    /// 获取虚拟化组件的异步集合的枚举状态
    /// </summary>
    public VirtualizationEnumerableState EnumerableState
        => RenderEndInfo.EnumerableState;
    #endregion
    #region 是否枚举完毕
    /// <summary>
    /// 获取集合是否已经枚举完毕（无论集合有没有元素）
    /// </summary>
    public bool IsComplete
        => RenderEndInfo.IsComplete;
    #endregion
}
