namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个枚举指示虚拟化组件的异步集合枚举状态
/// </summary>
public enum VirtualizationEnumerableState
{
    /// <summary>
    /// 未开始枚举
    /// </summary>
    NotEnumerable,
    /// <summary>
    /// 正在枚举
    /// </summary>
    InEnumerable,
    /// <summary>
    /// 已经枚举完毕，而且没有任何元素
    /// </summary>
    Empty,
    /// <summary>
    /// 已经枚举完毕，而且至少有一个元素
    /// </summary>
    Complete
}
