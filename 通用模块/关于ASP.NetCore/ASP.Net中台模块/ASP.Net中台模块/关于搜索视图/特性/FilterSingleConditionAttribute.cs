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
    #region 访问表达式
    /// <summary>
    /// 属性访问表达式，
    /// 通过它可以访问要查询或排序的属性，
    /// 如果为<see langword="null"/>，
    /// 则自动获取
    /// </summary>
    public string? PropertyAccess { get; init; }
    #endregion
    #region 查询条件默认值
    /// <summary>
    /// 查询条件的默认值字面量
    /// </summary>
    public string? PropertyAccessDefaultValue { get; init; }
    #endregion
    #region 逻辑运算符
    /// <summary>
    /// 描述查询条件的逻辑运算符
    /// </summary>
    public RenderLogicalOperator LogicalOperator { get; init; }
    #endregion
    #region 抽象成员实现：获取渲染条件组
    public override RenderFilterGroup ConvertConditioGroup(MemberInfo memberInfo)
    {
        var propertyAccess = PropertyAccess ?? GetPropertyAccess(memberInfo);
        var filterTargets = GetFilterTargets(memberInfo);
        var filterObjectType = GetFilterObjectType(filterTargets);
        var enumItem = EnumItem.Create(filterTargets);
        return new FilterSingleConditionInfo()
        {
            Describe = Describe,
            FilterObjectType = filterObjectType,
            LogicalOperator = LogicalOperator,
            PropertyAccess = propertyAccess,
            CanSort = CanSort,
            EnumItem = enumItem,
            ExcludeFilter = ExcludeFilter,
            IsVirtually = IsVirtually,
            Order = Order,
            PropertyAccessDefaultValue = PropertyAccessDefaultValue
        }.ConvertConditioGroup();
    }
    #endregion
}
