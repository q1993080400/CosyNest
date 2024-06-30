namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是用来生成复合筛选渲染条件的中间对象
/// </summary>
public sealed record FilterMultipleConditionInfo : FilterConditionInfo
{
    #region 开始属性访问表达式
    /// <summary>
    /// 获取作为区间开始的属性访问表达式
    /// </summary>
    public required string PropertyAccessStart { get; init; }
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
    public bool SortFromStart { get; init; }
    #endregion
    #region 抽象成员实现：获取渲染条件组
    public override RenderFilterGroup ConvertConditioGroup()
    {
        var propertyAccessEnd = PropertyAccessEnd ?? PropertyAccessStart;
        return new()
        {
            Describe = Describe,
            Order = Order,
            RenderFilterQuery = new()
            {
                FilterAction = new()
                {
                    ExcludeFilter = ExcludeFilter,
                    LogicalOperator = RenderLogicalOperator.None
                },
                FilterTarget = new FilterTargetMultiple()
                {

                    EnumItem = EnumItem,
                    FilterObjectType = FilterObjectType,
                    IsVirtually = IsVirtually,
                    PropertyAccessStart = new()
                    {
                        Name = PropertyAccessStart,
                        DefaultValue = PropertyAccessStartDefaultValue
                    },
                    PropertyAccessEnd = new()
                    {
                        Name = propertyAccessEnd,
                        DefaultValue = PropertyAccessEndDefaultValue
                    }
                }
            },
            RenderFilterSort = CanSort ? new()
            {
                FilterAction = new(),
                FilterTarget = new()
                {
                    EnumItem = EnumItem,
                    FilterObjectType = FilterObjectType,
                    IsVirtually = IsVirtually,
                    PropertyAccess = new()
                    {
                        Name = SortFromStart ? PropertyAccessStart : propertyAccessEnd,
                        DefaultValue = null
                    }
                }
            } : null
        };
    }
    #endregion
}
