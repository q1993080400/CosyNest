using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来重构数据查询表达式的参数
/// </summary>
public sealed record QueryConditionReconsitutionInfo
{
    #region 要重构的查询条件
    /// <summary>
    /// 获取要重构的查询条件
    /// </summary>
    public required QueryCondition QueryCondition { get; init; }
    #endregion
    #region 为查询条件生成的表达式
    /// <summary>
    /// 获取为查询条件生成的原始表达式
    /// </summary>
    public required Expression Expression { get; init; }
    #endregion
    #region 表达式的参数
    /// <summary>
    /// 获取表达式的参数，它代表要查询的实体
    /// </summary>
    public required ParameterExpression Parameter { get; init; }
    #endregion
}
