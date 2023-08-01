namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个枚举指示虚拟化组件的渲染状态
/// </summary>
public enum RenderVirtualizationState
{
    /// <summary>
    /// 指示该组件已经渲染成功，
    /// 且至少有一个元素被渲染
    /// </summary>
    HasElements,
    /// <summary>
    /// 指示该组件已经渲染成功，
    /// 但是没有元素被渲染
    /// </summary>
    NotElements,
    /// <summary>
    /// 指示该组件还没有渲染完毕，
    /// 它还在进行初始化
    /// </summary>
    NotReady
}
