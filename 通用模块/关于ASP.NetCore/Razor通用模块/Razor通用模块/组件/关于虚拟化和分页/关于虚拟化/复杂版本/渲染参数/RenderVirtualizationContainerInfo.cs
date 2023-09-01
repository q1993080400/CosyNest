namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 用来渲染<see cref="Virtualization{Element}"/>容器部分的参数
/// </summary>
public sealed record RenderVirtualizationContainerInfo
{
    #region 容器元素的ID
    /// <summary>
    /// 获取容器元素的ID，
    /// 为了正常使用虚拟化功能，
    /// 必须将这个ID赋值给容器
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region 容器的CSS类名
    /// <summary>
    /// 获取容器的CSS类名，
    /// 它可以为容器添加默认样式，
    /// 为了正常使用虚拟化功能，
    /// 必须将这个CSS类名赋值给容器
    /// </summary>
    public required string CSS { get; init; }
    #endregion
    #region OnScroll脚本
    /// <summary>
    /// 获取要添加到容器的OnScroll事件的脚本，
    /// 为了正常使用虚拟化功能，
    /// 必须将这个脚本注册到onscroll事件
    /// </summary>
    public required string OnScrollScript { get; init; }
    #endregion
    #region 用来跳跃到容器顶端的方法
    /// <summary>
    /// 调用这个方法，可以直接跳跃到容器顶端
    /// </summary>
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
    public required RenderFragment RenderEmpty { get; set; } = _ => { };
    #endregion
}
