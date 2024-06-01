namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是用来渲染<see cref="Virtualization{Element}"/>的参数
/// </summary>
/// <typeparam name="Obj"><see cref="Virtualization{Element}"/>的元素的类型</typeparam>
public sealed record RenderVirtualizationInfo<Obj>
{
    #region 数据源
    /// <summary>
    /// 获取数据源，
    /// 它枚举所有需要渲染的数据
    /// </summary>
    public required IEnumerable<Obj> DataSource { get; init; }
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
}
