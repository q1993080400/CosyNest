using System.Collections.Immutable;
using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是筛选视图的状态，
/// 通过它可以调整筛选条件，
/// 并生成最终的筛选结果
/// </summary>
public sealed class SearchViewerState
{
    #region 公开成员
    #region 生成筛选条件
    /// <summary>
    /// 生成筛选条件
    /// </summary>
    /// <returns></returns>
    public DataFilterDescription GenerateFilter()
    {
        #region 本地函数
        static IReadOnlyCollection<Obj> Fun<Obj>(IEnumerable<IGenerateFilter> generates)
            where Obj : DataCondition
            => generates.Select(static x => x.GenerateFilter()).
            SelectMany(static x => x).
            OfType<Obj>().ToArray();
        #endregion
        return new()
        {
            QueryCondition = Fun<QueryCondition>(QueryCondition.Values),
            SortCondition = Fun<SortCondition>(SortCondition.Values)
        };
    }
    #endregion
    #region 显式删除条件
    /// <summary>
    /// 显式删除查询或排序条件
    /// </summary>
    /// <param name="identification">要删除的查询或排序条件的标识</param>
    public void DeleteCondition(string identification)
    {
        QueryCondition = QueryCondition.Remove(identification);
        SortCondition = SortCondition.Remove(identification);
    }
    #endregion
    #region 绑定筛选条件
    #region 绑定查询条件
    #region 通用方法
    /// <summary>
    /// 绑定查询的通用方法
    /// </summary>
    /// <typeparam name="Property">属性的值的类型</typeparam>
    /// <param name="renderFilter">查询的条件</param>
    /// <returns></returns>
    public IBind<Property> Bind<Property>(RenderFilter<FilterTarget, FilterActionQuery> renderFilter)
    {
        var filterAction = renderFilter.FilterAction;
        switch (renderFilter.FilterTarget)
        {
            case FilterTargetSingle filterTargetSingle:
                var singleQuery = new RenderFilter<FilterTargetSingle, FilterActionQuery>()
                {
                    FilterAction = filterAction,
                    FilterTarget = filterTargetSingle
                };
                return Bind<Property>(singleQuery);
            case FilterTargetMultiple filterTargetMultiple:
                var multipleQuery = new RenderFilter<FilterTargetMultiple, FilterActionQuery>()
                {
                    FilterAction = filterAction,
                    FilterTarget = filterTargetMultiple
                };
                return Bind<Property>(multipleQuery);
            case var other:
                throw new NotSupportedException($"无法识别{other.GetType()}类型的绑定目标，无法生成查询条件");
        }
    }
    #endregion
    #region 绑定多项查询条件
    /// <summary>
    /// 返回一个查询条件绑定，
    /// 它可以绑定到多项查询条件
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Bind{Property}(RenderFilter{FilterTarget, FilterActionQuery})"/>
    public IBindRange<Property> Bind<Property>(RenderFilter<FilterTargetMultiple, FilterActionQuery> renderFilter)
    {
        switch (QueryCondition.GetValueOrDefault(renderFilter.FilterTarget.Identification))
        {
            case null:
                var bind = new BindMultipleQueryCondition<Property>(renderFilter);
                QueryCondition = QueryCondition.SetItem(bind.Identification, bind);
                return bind;
            case BindMultipleQueryCondition<Property> bindMultipleQuery:
                return bindMultipleQuery;
            case var other:
                throw new NotSupportedException($"找到了{other.GetType()}类型的查询条件，但是无法绑定它");
        }
    }
    #endregion
    #region 绑定单一查询条件
    /// <summary>
    /// 返回一个查询条件绑定，
    /// 将值绑定到它的<see cref="BindSingleQueryCondition{Property}.Value"/>就可以绑定为查询条件
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Bind{Property}(RenderFilter{FilterTarget, FilterActionQuery})"/>
    public IBindProperty<Property> Bind<Property>(RenderFilter<FilterTargetSingle, FilterActionQuery> renderFilter)
    {
        switch (QueryCondition.GetValueOrDefault(renderFilter.FilterTarget.Identification))
        {
            case null:
                var bind = new BindSingleQueryCondition<Property>(renderFilter);
                QueryCondition = QueryCondition.SetItem(bind.Identification, bind);
                return bind;
            case BindSingleQueryCondition<Property> bindSingleQuery:
                return bindSingleQuery;
            case var other:
                throw new NotSupportedException($"找到了{other.GetType()}类型的查询条件，但是无法绑定它");
        }
    }
    #endregion
    #endregion 
    #region 绑定排序条件
    /// <summary>
    /// 返回一个排序条件绑定，
    /// 将值绑定到它的<see cref="IBindProperty.Value"/>就可以绑定为排序条件
    /// </summary>
    /// <param name="renderFilter">排序的条件</param>
    /// <returns></returns>
    public IBindProperty<SortStatus> Bind(RenderFilter<FilterTargetSingle, FilterActionSort> renderFilter)
    {
        switch (SortCondition.GetValueOrDefault(renderFilter.FilterTarget.Identification))
        {
            case null:
                var bind = new BindSortCondition(renderFilter);
                SortCondition = SortCondition.SetItem(bind.Identification, bind);
                return bind;
            case BindSortCondition bindSortCondition:
                return bindSortCondition;
            case var other:
                throw new NotSupportedException($"找到了{other.GetType()}类型的查询条件，但是无法绑定它");
        }
    }
    #endregion
    #endregion
    #region 获取所有条件
    /// <summary>
    /// 获取现有的所有搜索和排序条件
    /// </summary>
    public IEnumerable<IHasFilterIdentification> AllCondition
        => QueryCondition.Values.Concat(SortCondition.Values).Cast<IHasFilterIdentification>();
    #endregion
    #region 清空筛选条件
    /// <summary>
    /// 清除所有查询或排序条件
    /// </summary>
    /// <param name="renderFilterGroup">如果这个值不为<see langword="null"/>，
    /// 则在清除条件之后，还会重新初始化默认值</param>
    public void Clear(IEnumerable<RenderFilterGroup>? renderFilterGroup = null)
    {
        QueryCondition = QueryCondition.Clear();
        SortCondition = SortCondition.Clear();
        if (renderFilterGroup is { })
            InitializeDefaultValue(renderFilterGroup);
    }
    #endregion
    #region 初始化默认值
    /// <summary>
    /// 如果有默认值，则初始化这些默认值
    /// </summary>
    /// <param name="renderFilterGroup">所有渲染筛选条件，
    /// 函数会寻找它们当中具有默认值的筛选条件，并初始化这些默认值</param>
    public void InitializeDefaultValue(IEnumerable<RenderFilterGroup> renderFilterGroup)
    {
        var hasDefaultValue = renderFilterGroup.Where(x => x.HasDefaultValue).ToArray();
        var identifying = QueryCondition.Values.Select(x => x.GenerateFilter()).
            SelectMany(x => x).
            WhereNotNull().
            Select(x => x.Identification).ToHashSet();
        foreach (var item in hasDefaultValue)
        {
            if (identifying.Contains(item.Identification))
                continue;
            var filterQuery = item.RenderFilterQuery;
            #region 本地函数
            void Bind<Property>()
                => this.Bind<Property>(filterQuery);
            #endregion
            switch (filterQuery.FilterTarget.FilterObjectType)
            {
                case FilterObjectType.Bool:
                    Bind<bool>();
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
    }
    #endregion
    #endregion
    #region 内部成员
    #region 所有查询条件
    /// <summary>
    /// 按标识索引所有查询条件
    /// </summary>
    private ImmutableDictionary<string, IGenerateFilter> QueryCondition { get; set; }
        = ImmutableDictionary<string, IGenerateFilter>.Empty;
    #endregion
    #region 所有排序条件
    /// <summary>
    /// 按标识索引所有排序条件
    /// </summary>
    private ImmutableDictionary<string, IGenerateFilter> SortCondition { get; set; }
        = ImmutableDictionary<string, IGenerateFilter>.Empty;
    #endregion
    #endregion
}
