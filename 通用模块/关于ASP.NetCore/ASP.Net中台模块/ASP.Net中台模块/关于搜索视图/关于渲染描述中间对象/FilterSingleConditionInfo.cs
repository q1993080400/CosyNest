namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是用来生成单一筛选渲染条件的中间对象
/// </summary>
public sealed record FilterSingleConditionInfo : FilterConditionInfo
{
    #region 访问表达式
    /// <summary>
    /// 属性访问表达式，
    /// 通过它可以访问要查询或排序的属性
    /// </summary>
    public required string PropertyAccess { get; init; }
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
    public required RenderLogicalOperator LogicalOperator { get; init; }
    #endregion
    #region 抽象成员实现：获取渲染条件组
    public override RenderFilterGroup ConvertConditioGroup()
        => new()
        {
            Describe = Describe,
            Order = Order,
            RenderFilterQuery = new()
            {
                FilterAction = new()
                {
                    ExcludeFilter = ExcludeFilter,
                    LogicalOperator = LogicalOperator
                },
                FilterTarget = new FilterTargetSingle()
                {
                    EnumItem = EnumItem,
                    FilterObjectType = FilterObjectType,
                    IsVirtually = IsVirtually,
                    PropertyAccess = new()
                    {
                        Name = PropertyAccess,
                        DefaultValue = PropertyAccessDefaultValue
                    }
                }
            },
            RenderFilterSort = CanSort ? new()
            {
                FilterAction = new(),
                FilterTarget = new FilterTargetSingle()
                {
                    EnumItem = EnumItem,
                    FilterObjectType = FilterObjectType,
                    IsVirtually = IsVirtually,
                    PropertyAccess = new()
                    {
                        Name = PropertyAccess,
                        DefaultValue = null
                    }
                }
            } : null
        };
    #endregion
}
