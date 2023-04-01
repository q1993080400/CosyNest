using System.DataFrancis;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="SearchViewer{Obj}"/>的渲染参数
/// </summary>
/// <typeparam name="Obj">实体类的类型</typeparam>
public sealed record SearchViewerInfo<Obj>
{
    #region 公开成员
    #region 生成数据筛选条件
    /// <summary>
    /// 生成数据筛选条件
    /// </summary>
    /// <returns></returns>
    public DataFilterDescription<Obj> Generate()
        => new()
        {
            QueryCondition = QueryCondition.Values.ToArray(),
            SortCondition = SortCondition.Values.ToArray(),
        };
    #endregion
    #region 关于查询条件
    #region 调整查询条件
    /// <summary>
    /// 调整查询条件
    /// </summary>
    /// <param name="propertyAccess">属性访问表达式，
    /// 它决定应该访问实体类的什么属性，支持递归</param>
    /// <param name="logicalOperator">用来比较实体的逻辑运算符</param>
    /// <param name="compareValue">用来和实体类属性进行比较的值，
    /// 如果为<see langword="null"/>，表示删除这个查询条件</param>
    /// <returns></returns>
    public void AdjustQuery(string propertyAccess, LogicalOperator logicalOperator, object? compareValue)
    {
        var key = GenerateQueryKey(propertyAccess, logicalOperator);
        if (compareValue is { })
        {
            var condition = new QueryCondition<Obj>()
            {
                CompareValue = compareValue,
                LogicalOperator = logicalOperator,
                PropertyAccess = propertyAccess
            };
            QueryCondition[key] = condition;
        }
        else
            QueryCondition.Remove(key);
    }
    #endregion
    #region 清空查询条件
    /// <summary>
    /// 清空查询条件
    /// </summary>
    public void ClearQuery()
        => QueryCondition.Clear();
    #endregion
    #region 绑定查询条件
    #region 不指定默认值
    /// <summary>
    /// 返回一个对象，组件将值绑定到它的<see cref="QueryBind{Property, Obj}.Bind"/>属性等同于绑定到查询条件
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="QueryBind{Property, Obj}"/>
    /// <inheritdoc cref="QueryBind{Property, Obj}.QueryBind(SearchViewerInfo{Obj}, string, LogicalOperator)"/>
    public QueryBind<Property, Obj> QueryBind<Property>(string propertyAccess, LogicalOperator logicalOperator)
        => new(this, propertyAccess, logicalOperator);
    #endregion
    #region 指定默认值
    /// <summary>
    /// 返回一个对象，组件将值绑定到它的<see cref="QueryBind{Property, Obj}.Bind"/>属性等同于绑定到查询条件，
    /// 并为查询条件指定默认值
    /// </summary>
    /// <param name="default">查询条件的默认值</param>
    /// <returns></returns>
    /// <inheritdoc cref="QueryBind{Property}(string, LogicalOperator)"/>
    public QueryBind<Property, Obj> QueryBind<Property>(string propertyAccess, LogicalOperator logicalOperator, Property @default)
    {
        var bind = QueryBind<Property>(propertyAccess, logicalOperator);
        var key = GenerateQueryKey(propertyAccess, logicalOperator);
        if (!QueryCondition.ContainsKey(key))
            bind.Bind = @default;
        return bind;
    }
    #endregion
    #endregion
    #endregion
    #region 关于排序条件
    #region 调整排序条件
    /// <summary>
    /// 调整排序条件
    /// </summary>
    /// <param name="isAscending">如果这个值为<see langword="true"/>，表示升序，
    /// 为<see langword="false"/>，表示降序，为<see langword="null"/>，表示删除这个排序条件</param>
    /// <returns></returns>
    /// <inheritdoc cref="AdjustQuery(string, LogicalOperator, object?)"/>
    public void AdjustSort(string propertyAccess, bool? isAscending)
    {
        if (isAscending is { } ascending)
        {
            var condition = new SortCondition<Obj>()
            {
                IsAscending = ascending,
                Priority = Seed,
                PropertyAccess = propertyAccess
            };
            SortCondition[propertyAccess] = condition;
        }
        else
            SortCondition.Remove(propertyAccess);
    }
    #endregion
    #region 清空排序条件
    /// <summary>
    /// 清空排序条件
    /// </summary>
    public void ClearSort()
        => SortCondition.Clear();
    #endregion
    #region 绑定排序条件
    /// <summary>
    /// 返回一个对象，组件将值绑定到它的<see cref="SortBind{Property, Obj}.Bind"/>属性等同于绑定到排序条件
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="SortBind{Property, Obj}"/>
    /// <inheritdoc cref="SortBind{Property, Obj}.SortBind(SearchViewerInfo{Obj}, string, Func{Property, bool?}, Func{bool?, Property})"/>
    public SortBind<Property, Obj> SortBind<Property>(string propertyAccess, Func<Property?, bool?> toSortCondition, Func<bool?, Property?> fromSortCondition)
        => new(this, propertyAccess, toSortCondition, fromSortCondition);
    #endregion
    #endregion
    #region 清空所有条件
    /// <summary>
    /// 清空查询条件和排序条件
    /// </summary>
    public void ClearAll()
    {
        ClearQuery();
        ClearSort();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 生成查询条件的键
    /// <summary>
    /// 生成用来获取查询条件的键，
    /// 它的目的是当逻辑运算符为大于或小于时，
    /// 属性的值是一个区间，有一个最大值和最小值，通过它可以避免键名称冲突
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="AdjustQuery(string, LogicalOperator, object?)"/>
    internal static string GenerateQueryKey(string propertyAccess, LogicalOperator logicalOperator)
        => propertyAccess + (logicalOperator switch
        {
            LogicalOperator.GreaterThan or LogicalOperator.GreaterThanOrEqual => "Min",
            LogicalOperator.LessThan or LogicalOperator.LessThanOrEqual => "Max",
            _ => ""
        });
    #endregion
    #region 递增种子
    private int SeedField { get; set; }

    /// <summary>
    /// 获取一个递增种子，
    /// 它可以用来作为<see cref="SortCondition{Obj}.Priority"/>
    /// </summary>
    private int Seed => SeedField++;
    #endregion
    #region 缓存查询条件
    /// <summary>
    /// 根据属性名称缓存查询条件的字典
    /// </summary>
    internal Dictionary<string, QueryCondition<Obj>> QueryCondition { get; } = new();
    #endregion
    #region 缓存排序条件
    /// <summary>
    /// 根据属性名称缓存排序条件的字典
    /// </summary>
    internal Dictionary<string, SortCondition<Obj>> SortCondition { get; } = new();
    #endregion
    #endregion
}
