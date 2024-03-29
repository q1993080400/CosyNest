﻿using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以作为一个支持将数据更改推送到数据源的管道
/// </summary>
public interface IDataPipeTo
{
    #region 关于添加与更新数据
    #region 添加或更新数据
    #region 传入同步集合
    /// <summary>
    /// 通过管道将数据添加到数据源，
    /// 如果数据存在，则将其更新
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <param name="datas">待添加或更新的数据</param>
    /// <param name="specifyPrimaryKey">这个委托指示主键是否为显式指定，
    /// 如果一个实体需要被添加到数据库中，且显式指定了它的主键，
    /// 则必须传入本参数，否则会产生异常</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    Task AddOrUpdate<Data>(IEnumerable<Data> datas, Func<Guid, bool>? specifyPrimaryKey = null, CancellationToken cancellation = default)
        where Data : class;
    #endregion
    #region 传入异步集合
    /// <inheritdoc cref="AddOrUpdate{Data}(IEnumerable{Data}, Func{Guid, bool}?, CancellationToken)"/>
    async Task AddOrUpdate<Data>(IAsyncEnumerable<Data> datas, Func<Guid, bool>? specifyPrimaryKey = null, CancellationToken cancellation = default)
        where Data : class
        => await AddOrUpdate(await datas.ToListAsync(), specifyPrimaryKey, cancellation);
    #endregion
    #region 传入单个数据
    /// <param name="data">待添加或更新的数据</param>
    /// <inheritdoc cref="AddOrUpdate{Data}(IEnumerable{Data}, Func{Guid, bool}?, CancellationToken)"/>
    Task AddOrUpdate<Data>(Data data, Func<Guid, bool>? specifyPrimaryKey = null, CancellationToken cancellation = default)
       where Data : class
       => AddOrUpdate<Data>([data], specifyPrimaryKey, cancellation);
    #endregion
    #endregion
    #endregion
    #region 关于删除数据
    #region 传入同步集合
    /// <summary>
    /// 从数据源中删除指定的数据
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <param name="datas">待删除的数据</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class;
    #endregion
    #region 传入异步集合
    /// <inheritdoc cref="Delete{Data}(IEnumerable{Data}, CancellationToken)"/>
    async Task Delete<Data>(IAsyncEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class
        => await Delete(await datas.ToListAsync(), cancellation);
    #endregion
    #region 传入单个数据
    /// <param name="data">要删除的数据</param>
    /// <inheritdoc cref="Delete{Data}(IEnumerable{Data}, CancellationToken)"/>
    Task Delete<Data>(Data data, CancellationToken cancellation = default)
        where Data : class
        => Delete(new[] { data }, cancellation);
    #endregion
    #region 按条件删除数据
    /// <summary>
    /// 直接从数据源中删除符合指定谓词的数据，不返回结果集
    /// </summary>
    /// <param name="expression">一个用来指定删除条件的表达式</param>
    /// <inheritdoc cref="IDataPipeFrom.Query{Data}"/>
    Task Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation = default)
        where Data : class;
    #endregion
    #endregion
}
