using System.Reflection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个特性应用在实体类或者实体类的属性上，
/// 表示它是一个单一的筛选条件
/// </summary>
/// <inheritdoc cref="FilterConditionAttribute{BusinessInterface}"/>
public sealed class FilterSingleConditionAttribute<BusinessInterface> : FilterConditionAttribute<BusinessInterface>
    where BusinessInterface : class, IGetRenderAllFilterCondition
{
    #region 逻辑运算符
    /// <summary>
    /// 描述查询条件的逻辑运算符
    /// </summary>
    public RenderLogicalOperator LogicalOperator { get; init; } = RenderLogicalOperator.Equal;
    #endregion
    #region 第一查询条件默认值
    /// <summary>
    /// 第一查询条件的默认值字面量
    /// </summary>
    public string? PropertyAccessDefaultValue { get; init; }
    #endregion
    #region 排除查询条件
    /// <summary>
    /// 如果这个值不为<see langword="null"/>，
    /// 表示当这个条件的值等于这个字面量的时候，
    /// 排除这个查询条件
    /// </summary>
    public string? ExcludeFilter { get; init; }
    #endregion
    #region 抽象成员实现：获取渲染条件组
    public override RenderConditionGroup ConvertConditioGroup(MemberInfo memberInfo)
    {
        var propertyAccess = GetPropertyAccess(memberInfo);
        var filterObjectType = GetFilterObjectType(memberInfo);
        var enumItem = GetEnumItem(memberInfo);
        return new()
        {
            Describe = Describe,
            FilterObjectType = filterObjectType,
            Order = Order,
            FirstQueryCondition = new()
            {
                LogicalOperator = LogicalOperator.ToLogicalOperator(),
                PropertyAccess = propertyAccess,
                EnumItem = enumItem,
                DefaultValue = PropertyAccessDefaultValue,
                ExcludeFilter = ExcludeFilter,
                IsVirtually = IsVirtually
            },
            SecondQueryCondition = null,
            SortCondition = CanSort ? new()
            {
                PropertyAccess = propertyAccess,
                IsVirtually = IsVirtually
            } : null
        };
    }
    #endregion
}
