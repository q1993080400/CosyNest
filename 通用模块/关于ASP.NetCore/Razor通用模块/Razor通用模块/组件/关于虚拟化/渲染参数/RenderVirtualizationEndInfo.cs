namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是用来渲染<see cref="Virtualization{Element}"/>末尾部分的参数
/// </summary>
public sealed record RenderVirtualizationEndInfo
{
    #region 末尾元素的ID
    /// <summary>
    /// 获取末尾元素的ID，
    /// 为使组件正常工作，必须将它赋值给末尾元素的ID和@key
    /// </summary>
    public required string EndID { get; init; }
    #endregion
    #region 枚举状态
    /// <summary>
    /// 获取虚拟化组件的异步集合的枚举状态
    /// </summary>
    public required VirtualizationEnumerableState EnumerableState { get; init; }
    #endregion
}
