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
    #region 构造虚拟查询条件的函数
    /// <summary>
    /// 构造虚拟查询条件的函数，
    /// 它的参数是虚拟查询条件，
    /// 返回值是构造好的查询条件表达式
    /// </summary>
    public Func<QueryCondition, Expression<Func<Obj, bool>>>? GenerateVirtuallyQuery { get; init; }
    #endregion
    #region 构造虚拟排序条件的函数
    /// <summary>
    /// 构造虚拟排序条件的函数，
    /// 它的第一个参数是虚拟排序条件，
    /// 第二个参数是当前排序好的表达式，
    /// 返回值是经过虚拟排序的表达式
    /// </summary>
    public Func<SortCondition, IOrderedQueryable<Obj>, IOrderedQueryable<Obj>>? GenerateVirtuallySort { get; init; }
    #endregion
    #region 高优先级排序函数
    /// <summary>
    /// 这个函数允许在执行筛选之后，
    /// 执行排序之前对<see cref="DataSource"/>进行高优先级的排序，
    /// 它比<see cref="Description"/>指定的排序条件优先级更高，
    /// 如果为<see langword="null"/>，则不执行它
    /// </summary>
    public Func<IQueryable<Obj>, IOrderedQueryable<Obj>>? SortFunction { get; init; }
    #endregion
    #region 是否跳过虚拟条件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 指示跳过虚拟查询和排序条件，
    /// 它们已经被用户自行处理
    /// </summary>
    public bool SkipVirtually { get; init; }
    #endregion
    #region 重构函数
    /// <summary>
    /// 这个委托允许重构生成的每个真实查询表达式，
    /// 并返回重构后的表达式，如果为<see langword="null"/>，则不进行重构
    /// </summary>
    public Func<QueryConditionReconsitutionInfo, Expression>? Reconsitution { get; init; }
    #endregion
}
