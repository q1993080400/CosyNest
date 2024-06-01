using System.DataFrancis;
using System.Reflection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个特性应用在实体类或者实体类的属性上，
/// 表示它是一个复合的筛选条件，
/// 它实际包含两个筛选条件，大于等于和小于等于
/// </summary>
/// <inheritdoc cref="FilterConditionAttribute{BusinessInterface}"/>
public sealed class FilterMultipleConditionAttribute<BusinessInterface> : FilterConditionAttribute<BusinessInterface>
    where BusinessInterface : class, IGetRenderAllFilterCondition
{
    #region 另一个属性访问表达式
    /// <summary>
    /// 另一个属性访问表达式，按照规范，
    /// <see cref="PropertyAccess"/>被映射为大于等于条件，
    /// 本属性被映射为小于等于条件，
    /// 如果为<see langword="null"/>，
    /// 表示在同一数据库属性上执行筛选
    /// </summary>
    public string? PropertyAccessOther { get; init; }
    #endregion
    #region 第一查询条件默认值
    /// <summary>
    /// 第一查询条件的默认值字面量
    /// </summary>
    public string? PropertyAccessDefaultValue { get; init; }
    #endregion
    #region 第二查询条件默认值
    /// <summary>
    /// 第二查询条件的默认值字面量
    /// </summary>
    public string? PropertyAccessOtherDefaultValue { get; init; }
    #endregion
    #region 排序依据
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则<see cref="PropertyAccess"/>作为排序依据，
    /// 否则<see cref="PropertyAccessOther"/>作为排序依据
    /// </summary>
    public bool SortPropertyAccess { get; init; } = true;
    #endregion
    #region 抽象成员实现：获取渲染条件组
    public override RenderConditionGroup ConvertConditioGroup(MemberInfo memberInfo)
    {
        var propertyAccess = GetPropertyAccess(memberInfo);
        var propertyAccessOther = PropertyAccessOther ?? propertyAccess;
        var filterObjectType = GetFilterObjectType(FilterObjectType, memberInfo);
        var enumItem = GetEnumItem(memberInfo);
        return new()
        {
            Describe = Describe,
            FilterObjectType = filterObjectType,
            Order = Order,
            FirstQueryCondition = new()
            {
                LogicalOperator = LogicalOperator.GreaterThanOrEqual,
                PropertyAccess = propertyAccess,
                EnumItem = enumItem,
                DefaultValue = PropertyAccessDefaultValue,
                ExcludeFilter = null,
                IsVirtually = IsVirtually
            },
            SecondQueryCondition = new()
            {
                LogicalOperator = LogicalOperator.LessThanOrEqual,
                PropertyAccess = propertyAccessOther,
                EnumItem = enumItem,
                DefaultValue = PropertyAccessOtherDefaultValue,
                ExcludeFilter = null,
                IsVirtually = IsVirtually

            },
            SortCondition = CanSort ? new()
            {
                PropertyAccess = SortPropertyAccess ? propertyAccess : propertyAccessOther,
                IsVirtually = IsVirtually
            } : null
        };
    }
    #endregion
}
