namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以获取一个条件，
/// 并将这个条件传递给子组件
/// </summary>
/// <typeparam name="Condition">条件的类型</typeparam>
public sealed partial class RenderCondition<Condition> : ComponentBase, IContentComponent<RenderFragment<Condition>>
    where Condition : class
{
    #region 组件参数
    #region 获取条件的委托
    /// <summary>
    /// 这个委托可以异步获取这个条件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Task<Condition>> GetCondition { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<Condition> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 条件的缓存
    /// <summary>
    /// 对条件的缓存
    /// </summary>
    private Condition? CacheCondition { get; set; }
    #endregion
    #region 重写OnParametersSetAsync方法
    protected override async Task OnParametersSetAsync()
    {
        CacheCondition = await GetCondition();
    }
    #endregion
    #endregion
}
