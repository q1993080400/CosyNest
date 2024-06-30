namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是表示一个查询筛选动作
/// </summary>
public sealed record FilterActionQuery : FilterAction
{
    #region 逻辑运算符
    /// <summary>
    /// 描述查询条件的逻辑运算符
    /// </summary>
    public required RenderLogicalOperator LogicalOperator { get; init; }
    #endregion
    #region 排除查询条件
    /// <summary>
    /// 如果这个值不为<see langword="null"/>，
    /// 表示当这个条件的值等于这个字面量的时候，
    /// 排除这个查询条件
    /// </summary>
    public required string? ExcludeFilter { get; init; }
    #endregion
}
