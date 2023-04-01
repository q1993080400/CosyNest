﻿namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来解析一个<see cref="DataFilterDescription{Obj}"/>，
/// 并将它转换为查询表达式
/// </summary>
public interface IDataFilterAnalysis
{
    #region 解析为查询表达式
    /// <summary>
    /// 将<see cref="DataFilterDescription{Obj}"/>解析为查询表达式，
    /// 通过它可以提取数据
    /// </summary>
    /// <param name="dataSource">数据源</param>
    /// <param name="description">待解析的<see cref="DataFilterDescription{Obj}"/></param>
    /// <param name="sortFunction">这个函数允许在执行筛选之后，执行排序之前对<paramref name="dataSource"/>进行高优先级的排序，
    /// 如果为<see langword="null"/>，则不执行它</param>
    /// <returns></returns>
    /// <inheritdoc cref="DataFilterDescription{Obj}"/>
    IOrderedQueryable<Obj> Analysis<Obj>(IQueryable<Obj> dataSource, DataFilterDescription<Obj> description, Func<IQueryable<Obj>, IOrderedQueryable<Obj>>? sortFunction = null);
    #endregion
}
