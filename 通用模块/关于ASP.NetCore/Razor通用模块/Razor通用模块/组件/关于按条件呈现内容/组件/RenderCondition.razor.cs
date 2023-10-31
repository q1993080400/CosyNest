namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许按条件渲染内容
/// </summary>
/// <typeparam name="Condition">渲染条件的类型</typeparam>
public sealed partial class RenderCondition<Condition> : ComponentBase, IContentComponent<RenderFragment<Condition>>
{
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<Condition> ChildContent { get; set; }
    #endregion
    #region 用来获取条件的委托
    /// <summary>
    /// 用来获取条件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Task<Condition>> GetCondition { get; set; }
    #endregion
    #region 是否将条件作为级联参数
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则这个组件的条件会被作为级联参数传递下去，
    /// 注意：它是固定参数，而且假设这个参数永不刷新
    /// </summary>
    [Parameter]
    public bool AsCascadingParameter { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 是否已初始化条件
    /// <summary>
    /// 获取是否已经初始化条件
    /// </summary>
    private bool InitializationCondition { get; set; }
    #endregion
    #region 条件的缓存
    /// <summary>
    /// 获取条件的缓存
    /// </summary>
    private Condition ConditionCache { get; set; }
    #endregion
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsync()
    {
        ConditionCache = await GetCondition();
        InitializationCondition = true;
    }
    #endregion
    #endregion
}
