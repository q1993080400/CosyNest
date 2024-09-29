namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个特性可以放在可路由组件上面，
/// 用来标记该组件的标题
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ComponentTitleAttribute : Attribute
{
    #region 组件标题
    /// <summary>
    /// 获取这个组件的标题
    /// </summary>
    public required string Title { get; init; }
    #endregion
}
