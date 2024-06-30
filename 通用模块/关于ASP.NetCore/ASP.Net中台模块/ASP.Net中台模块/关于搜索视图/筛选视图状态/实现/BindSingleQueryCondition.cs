using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以将值绑定到单一查询条件
/// </summary>
/// <typeparam name="Property">属性值的类型</typeparam>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <inheritdoc cref="BindFilterCondition{Target, Action, Property}.BindFilterCondition(RenderFilter{Target, Action})"/>
sealed class BindSingleQueryCondition<Property>(RenderFilter<FilterTargetSingle, FilterActionQuery> renderFilter) :
   BindFilterCondition<FilterTargetSingle, FilterActionQuery, Property>(renderFilter), IBindProperty<Property>
{
    #region 属性的值
    public Property? Value { get; set; } = renderFilter.FilterTarget.GetDefaultValue<Property>();
    #endregion
    #region 生成查询条件
    public override DataCondition[] GenerateFilter()
    {
        var filterAction = RenderFilter.FilterAction;
        var filterTargetSingle = RenderFilter.FilterTarget;
        var logicalOperator = filterAction.LogicalOperator.ToLogicalOperator(filterTargetSingle.FilterObjectType);
        return (TrimValue(Value), filterAction) switch
        {
            (null, _) => [],
            ({ } value, { ExcludeFilter: var excludeFilter })
            => excludeFilter is null || !Equals(value, excludeFilter.To<Property>()) ?
            [new QueryCondition()
            {
                CompareValue = value,
                LogicalOperator = logicalOperator,
                Identification = filterTargetSingle.PropertyAccess.Name,
                IsVirtually = filterTargetSingle.IsVirtually
            }] : [],
            _ => throw new NotSupportedException("不能生成查询条件")
        };
    }
    #endregion
}
