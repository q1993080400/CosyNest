using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以将值绑定到复合查询条件，
/// 它具有一个开始和结束
/// </summary>
/// <typeparam name="Property">属性值的类型</typeparam>
sealed class BindMultipleQueryCondition<Property> :
   BindFilterCondition<FilterTargetMultiple, FilterActionQuery, Property>, IBindRange<Property>
{
    #region 筛选范围
    public BindRange<Property> Range { get; }
    #endregion
    #region 生成查询条件
    public override DataCondition[] GenerateFilter()
    {
        var filterTargetMultiple = RenderFilter.FilterTarget;
        #region 本地函数
        DataCondition? Fun(string identification, Property? value, bool greater)
            => TrimValue(value) is { } trimValue ?
            new QueryCondition()
            {
                CompareValue = trimValue,
                Identification = identification,
                IsVirtually = filterTargetMultiple.IsVirtually,
                LogicalOperator = greater ? LogicalOperator.GreaterThanOrEqual : LogicalOperator.LessThanOrEqual
            } : null;
        #endregion
        DataCondition?[] array =
            [
                Fun(filterTargetMultiple.PropertyAccessStart.Name,Range.Start,true),
                Fun(filterTargetMultiple.PropertyAccessEnd.Name,Range.End,false)
            ];
        return [.. array.WhereNotNull()];
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的筛选条件初始化对象
    /// </summary>
    /// <inheritdoc cref="BindFilterCondition{Target, Action, Property}.BindFilterCondition(RenderFilter{Target, Action})"/>
    public BindMultipleQueryCondition(RenderFilter<FilterTargetMultiple, FilterActionQuery> renderFilter)
        : base(renderFilter)
    {
        var filterTargetMultiple = renderFilter.FilterTarget;
        Range = new()
        {
            Start = filterTargetMultiple.PropertyAccessStart.GetDefaultValue<Property>(),
            End = filterTargetMultiple.PropertyAccessEnd.GetDefaultValue<Property>(),
        };
    }
    #endregion
}
