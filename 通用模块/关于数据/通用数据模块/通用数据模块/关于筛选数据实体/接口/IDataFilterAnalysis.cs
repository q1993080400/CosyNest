namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来解析一个<see cref="DataFilterDescription"/>，
/// 并将它转换为查询表达式
/// </summary>
public interface IDataFilterAnalysis
{
    #region 解析为查询表达式
    /// <summary>
    /// 将<see cref="DataFilterDescription"/>解析为查询表达式，
    /// 通过它可以提取数据
    /// </summary>
    /// <typeparam name="Obj">实体类的类型</typeparam>
    /// <param name="info">解析表达式的参数</param>
    /// <returns></returns>
    IOrderedQueryable<Obj> Analysis<Obj>(DataFilterAnalysisInfo<Obj> info);
    #endregion
}
