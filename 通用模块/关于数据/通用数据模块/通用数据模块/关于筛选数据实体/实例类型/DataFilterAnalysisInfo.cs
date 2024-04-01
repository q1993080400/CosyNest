using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来解析查询表达式的参数
/// </summary>
/// <typeparam name="Obj">表达式的元素类型</typeparam>
public sealed record DataFilterAnalysisInfo<Obj>
{
    #region 数据源
    /// <summary>
    /// 获取数据源
    /// </summary>
    public required IQueryable<Obj> DataSource { get; init; }
    #endregion
    #region 查询和排序条件
    /// <summary>
    /// 这个记录描述查询和排序的条件，
    /// 如果为<see langword="null"/>，表示不筛选
    /// </summary>
    public required DataFilterDescription Description { get; init; }
    #endregion
    #region 高优先级排序函数
    /// <summary>
    /// 这个函数允许在执行筛选之后，执行排序之前对<see cref="DataSource"/>进行高优先级的排序，
    /// 如果为<see langword="null"/>，则不执行它
    /// </summary>
    public Func<IQueryable<Obj>, IOrderedQueryable<Obj>>? SortFunction { get; init; }
    #endregion
    #region 重构函数
    /// <summary>
    /// 这个委托允许重构生成的每个查询表达式，
    /// 并返回重构后的表达式，如果为<see langword="null"/>，则不进行重构
    /// </summary>
    public Func<QueryConditionReconsitutionInfo, Expression>? Reconsitution { get; init; }
    #endregion
}
