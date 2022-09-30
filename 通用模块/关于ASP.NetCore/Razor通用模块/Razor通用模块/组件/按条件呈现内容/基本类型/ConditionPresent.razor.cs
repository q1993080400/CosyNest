namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件可以根据不同的条件呈现不同的内容
/// </summary>
/// <typeparam name="Condition">呈现条件的类型</typeparam>
public abstract partial class ConditionPresent<Condition> : ComponentBase, IContentComponent<RenderFragment<Condition?>>
{
    #region 获取条件
    /// <summary>
    /// 获取呈现的条件
    /// </summary>
    protected abstract Condition? GetCondition { get; }
    #endregion
    #region 子内容
    /// <summary>
    /// 获取要呈现的子内容，
    /// 它的参数就是呈现条件，
    /// 注意：呈现条件可能为<see langword="null"/>
    /// </summary>
    [Parameter]
    public RenderFragment<Condition?>? ChildContent { get; set; }
    #endregion
}
