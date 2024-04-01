using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录描述如何渲染单个查询条件
/// </summary>
public sealed record RenderQueryCondition : RenderFilterCondition
{
    #region 逻辑运算符
    /// <summary>
    /// 描述查询条件的逻辑运算符
    /// </summary>
    public required LogicalOperator LogicalOperator { get; init; }
    #endregion
    #region 枚举的值
    /// <summary>
    /// 如果要筛选枚举，
    /// 则这个集合描述枚举可能的值和描述
    /// </summary>
    public required IEnumerable<EnumItem> EnumItem { get; init; }
    #endregion
    #region 排除查询条件
    /// <summary>
    /// 如果这个值不为<see langword="null"/>，
    /// 表示当这个条件的值等于这个字面量的时候，
    /// 排除这个查询条件
    /// </summary>
    public required string? ExcludeFilter { get; init; }
    #endregion
    #region 默认值
    /// <summary>
    /// 查询条件的默认值字面量
    /// </summary>
    public required string? DefaultValue { get; init; }
    #endregion
}
