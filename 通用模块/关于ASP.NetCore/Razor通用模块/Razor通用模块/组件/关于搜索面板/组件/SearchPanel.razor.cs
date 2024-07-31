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
    /// 当清除搜索后发生的事件
    /// </summary>
    [Parameter]
    public Func<Task>? OnClear { get; set; }
    #endregion
    #endregion
    #endregion
    #region 公开成员
    #region 高亮文字参数名称
    /// <summary>
    /// 获取高亮文字的参数名称
    /// </summary>
    public const string HighlightParameter = "4F8A7B08-C197-4D0C-B645-2FC403193F3D";
    #endregion
    #endregion
    #region 内部成员
    #region 应该初始化默认查询条件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该初始化默认查询条件
    /// </summary>
    private bool InitializeDefaultQueryConditions { get; set; } = true;
    #endregion
    #region 缓存描述如何渲染筛选条件的对象
    /// <summary>
    /// 这个属性缓存描述如何渲染筛选条件的对象
    /// </summary>
    private RenderFilterGroup[]? CacheRenderCondition { get; set; }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!InitializeDefaultQueryConditions)
            return;
        InitializeDefaultQueryConditions = false;
        CacheRenderCondition = await GetRenderCondition();
        var hasDefaultValue = CacheRenderCondition.Where(x => x.HasDefaultValue).ToArray();
        foreach (var item in hasDefaultValue)
        {
            var filterQuery = item.RenderFilterQuery;
            #region 本地函数
            void Bind<Property>()
                => SearchViewerState.Bind<Property>(filterQuery);
            #endregion
            switch (filterQuery.FilterTarget.FilterObjectType)
            {
                case FilterObjectType.Bool:
                    Bind<bool?>();
                    break;
                case FilterObjectType.Text:
                    Bind<string>();
                    break;
                case FilterObjectType.Num:
                    Bind<double?>();
                    break;
                case FilterObjectType.Date:
                    Bind<DateTimeOffset?>();
                    break;
                case FilterObjectType.Enum:
                    Bind<string?>();
                    break;
                case var filterObjectType:
                    throw new NotSupportedException($"不能识别筛选对象类型{filterObjectType}");
            }
        }
        await SubmitFunction();
    }
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
        await ElementNumber.JumpToElement(0, false)();
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
            SearchViewerState.Clear();
            InitializeDefaultQueryConditions = true;
            if (OnClear is { })
                await OnClear();
        }
        #endregion
        return new()
        {
            SearchViewerState = SearchViewerState,
            RenderSubmit = RenderSubmit(new()
            {
                Clear = Clear,
                Submit = SubmitFunction,
                GoToTop = ElementNumber.JumpToElement(0)
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
