namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件可以根据不同的条件渲染不同的内容
/// </summary>
/// <typeparam name="Condition">渲染条件的类型</typeparam>
public abstract partial class ConditionRender<Condition> : ComponentBase, IContentComponent<RenderFragment<Condition>>
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
    /// 它的参数就是呈现条件
    /// </summary>
    [Parameter]
    public RenderFragment<Condition> ChildContent { get; set; }
    #endregion
}
