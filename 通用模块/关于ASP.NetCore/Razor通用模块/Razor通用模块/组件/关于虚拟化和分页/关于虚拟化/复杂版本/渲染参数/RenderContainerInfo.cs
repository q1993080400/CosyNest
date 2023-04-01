namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 用来渲染<see cref="Virtualization{Element}"/>容器部分的参数
/// </summary>
public sealed record RenderContainerInfo
{
    #region 容器元素的ID
    /// <summary>
    /// 获取容器元素的ID，
    /// JS需要通过这个ID找到容器，
    /// 才能够正确地添加虚拟化功能
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region 容器的CSS类名
    /// <summary>
    /// 获取容器的CSS类名，
    /// 它可以为容器添加默认样式
    /// </summary>
    public required string CSS { get; init; }
    #endregion
    #region OnScroll脚本
    /// <summary>
    /// 获取要添加到容器的OnScroll事件的脚本，
    /// 它为虚拟化提供支持
    /// </summary>
    public required string OnScrollScript { get; init; }
    #endregion
    #region 用来跳跃到容器顶端的方法
    /// <summary>
    /// 调用这个方法，可以直接跳跃到容器顶端
    /// </summary>
    public required EventCallback GoTop { get; init; }
    #endregion
}
