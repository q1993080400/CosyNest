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
    #region 开始属性访问表达式
    /// <summary>
    /// 获取作为区间开始的属性访问表达式，
    /// 如果为<see langword="null"/>，
    /// 则自动获取
    /// </summary>
    public string? PropertyAccessStart { get; init; }
    #endregion
    #region 结束属性访问表达式
    /// <summary>
    /// 获取作为区间结束的属性访问表达式，
    /// 如果为<see langword="null"/>，
    /// 默认为和<see cref="PropertyAccessStart"/>相同
    /// </summary>
    public string? PropertyAccessEnd { get; init; }
    #endregion
    #region 区间开始查询条件默认值
    /// <summary>
    /// 区间开始查询条件的默认值字面量
    /// </summary>
    public string? PropertyAccessStartDefaultValue { get; init; }
    #endregion
    #region 区间结束查询条件默认值
    /// <summary>
    /// 区间结束条件的默认值字面量
    /// </summary>
    public string? PropertyAccessEndDefaultValue { get; init; }
    #endregion
    #region 排序依据
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则<see cref="PropertyAccessStart"/>作为排序依据，
    /// 否则<see cref="PropertyAccessEnd"/>作为排序依据
    /// </summary>
    public bool SortFromStart { get; init; } = true;
    #endregion
    #region 抽象成员实现：获取渲染条件组
    public override RenderFilterGroup ConvertConditioGroup(MemberInfo memberInfo)
    {
        var propertyAccessStart = PropertyAccessStart ?? GetPropertyAccess(memberInfo);
        var propertyAccessEnd = PropertyAccessEnd ?? propertyAccessStart;
        var filterTargets = GetFilterTargets(memberInfo);
        var filterObjectType = GetFilterObjectType(filterTargets);
        var enumItem = EnumItem.Create(filterTargets);
        return new FilterMultipleConditionInfo()
        {
            Describe = Describe,
            FilterObjectType = filterObjectType,
            PropertyAccessStart = propertyAccessStart,
            PropertyAccessEnd = propertyAccessEnd,
            CanSort = CanSort,
            EnumItem = enumItem,
            ExcludeFilter = ExcludeFilter,
            IsVirtually = IsVirtually,
            Order = Order,
            PropertyAccessEndDefaultValue = PropertyAccessEndDefaultValue,
            PropertyAccessStartDefaultValue = PropertyAccessStartDefaultValue,
            SortFromStart = SortFromStart
        }.ConvertConditioGroup();
    }
    #endregion
}
