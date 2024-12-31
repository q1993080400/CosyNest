namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是搜索视图的一种实现方式，
/// 它以面板的形式渲染搜索条件
/// </summary>
public sealed partial class SearchPanel : ComponentBase
{
    #region 组件参数
    #region 有关渲染
    #region 渲染整个组件的委托
    /// <summary>
    /// 用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSearchPanelInfo> RenderComponent { get; set; }
    #endregion
    #region 获取如何渲染筛选条件
    /// <summary>
    /// 这个委托异步获取一个集合，
    /// 它指示如何渲染筛选条件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Task<RenderFilterGroup[]>> GetRenderCondition { get; set; }
    #endregion
    #region 搜索视图状态
    /// <summary>
    /// 搜索视图的状态
    /// </summary>
    [Parameter]
    [EditorRequired]
    public SearchViewerState SearchViewerState { get; set; }
    #endregion
    #region 渲染单个属性的委托
    /// <summary>
    /// 这个委托用来渲染单个搜索或排序条件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSearchPanelPropertyInfo> RenderProperty { get; set; }
    #endregion
    #region 渲染提交区域的委托
    /// <summary>
    /// 获取一个渲染提交区域的委托，
    /// 它的参数是当前渲染状态
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSearchPanelSubmitInfo> RenderSubmit { get; set; }
    #endregion
    #endregion
    #region 有关业务
    #region 元素编号对象
    /// <summary>
    /// 获取元素编号对象，
    /// 它能够保证在提交搜索后，
    /// 正确地跳转到容器的最顶层，
    /// 避免不正确的渲染
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IElementNumber ElementNumber { get; set; }
    #endregion
    #region 提交搜索
    /// <summary>
    /// 这个委托可以用来提交搜索，
    /// 它具有一个参数，用来描述搜索条件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback<SearchPanelSubmitInfo> Submit { get; set; }
    #endregion
    #region 清除搜索后发生的事件
    /// <summary>
    /// 当清除搜索后发生的事件，
    /// 注意：不要在这个委托中刷新组件，
    /// 因为组件会自动刷新
    /// </summary>
    [Parameter]
    public Func<Task>? OnClear { get; set; }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 缓存描述如何渲染筛选条件的对象
    /// <summary>
    /// 这个属性缓存描述如何渲染筛选条件的对象
    /// </summary>
    private RenderFilterGroup[]? CacheRenderCondition { get; set; }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;
        CacheRenderCondition = await GetRenderCondition();
        SearchViewerState.InitializeDefaultValue(CacheRenderCondition);
        await SubmitFunction();
    }
    #endregion
    #region 跳转到顶层的方法
    /// <summary>
    /// 跳转到顶层
    /// </summary>
    /// <returns></returns>
    private Task JumpToTop()
        => ElementNumber.JumpToElement();
    #endregion
    #region 用来提交搜索的方法
    /// <summary>
    /// 这个方法是最终用来提交搜索的方法
    /// </summary>
    /// <returns></returns>
    private async Task SubmitFunction()
    {
        var info = new SearchPanelSubmitInfo()
        {
            DataFilterDescription = SearchViewerState.GenerateFilter()
        };
        await Submit.InvokeAsync(info);
        await JumpToTop();
    }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderSearchPanelInfo? GetRenderInfo()
    {
        if (CacheRenderCondition is null)
            return null;
        #region 用来清除的委托
        async Task Clear()
        {
            SearchViewerState.Clear(CacheRenderCondition);
            if (OnClear is { })
                await OnClear();
            await SubmitFunction();
        }
        #endregion
        return new()
        {
            SearchViewerState = SearchViewerState,
            RenderSubmit = RenderSubmit(new()
            {
                Clear = Clear,
                Submit = SubmitFunction,
                GoToTop = JumpToTop
            }),
            RenderCondition = CacheRenderCondition.Select(x =>
            {
                var renderProperty = new RenderSearchPanelPropertyInfo()
                {
                    RenderConditionGroup = x,
                    SearchViewerState = SearchViewerState,
                    Submit = SubmitFunction,
                    Clear = Clear
                };
                return RenderProperty(renderProperty);
            }).ToArray()
        };
    }
    #endregion
    #endregion
}
