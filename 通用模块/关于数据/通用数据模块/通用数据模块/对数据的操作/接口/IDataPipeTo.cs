﻿using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以作为一个支持将数据更改推送到数据源的管道
/// </summary>
public interface IDataPipeTo
{
    #region 关于添加与更新数据
    #region 添加数据
    #region 说明文档
    /*问：既然已经有了添加或更新数据，
      那么为什么还需要添加数据？
      答：这是因为有的时候，数据在被添加之前需要指定自己的ID，
      如果是添加或更新，那么ID会被自动赋值，无法满足这个需求*/
    #endregion
    #region 传入同步集合
    /// <summary>
    /// 添加一个数据，
    /// 如果数据已经存在，会引发一个异常
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <param name="datas">待添加的数据</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Add<Data>(IEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class, IData;
    #endregion
    #region 传入单个数据
    /// <param name="data">待添加的数据</param>
    /// <inheritdoc cref="Add{Data}(IEnumerable{Data}, CancellationToken)"/>
    Task Add<Data>(Data data, CancellationToken cancellation = default)
        where Data : class, IData
        => Add(new[] { data }, cancellation);
    #endregion
    #endregion
    #region 添加或更新数据
    #region 传入同步集合
    /// <summary>
    /// 通过管道将数据添加到数据源，
    /// 如果数据存在，则将其更新
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <param name="datas">待添加或更新的数据</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    Task AddOrUpdate<Data>(IEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class, IData;
    #endregion
    #region 传入异步集合
    /// <inheritdoc cref="AddOrUpdate{Data}(IEnumerable{Data}, CancellationToken)"/>
    async Task AddOrUpdate<Data>(IAsyncEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class, IData
        => await AddOrUpdate(await datas.ToListAsync(), cancellation);
    #endregion
    #region 传入单个数据
    /// <param name="data">待添加或更新的数据</param>
    /// <inheritdoc cref="AddOrUpdate{Data}(IEnumerable{Data}, CancellationToken)"/>
    Task AddOrUpdate<Data>(Data data, CancellationToken cancellation = default)
       where Data : class, IData
       => AddOrUpdate(new[] { data }, cancellation);
    #endregion
    #endregion
    #endregion
    #region 关于删除数据
    #region 说明文档
    /*实现本API请遵循以下规范：
      #如果IData.ID为null，不执行任何操作，
      如果不为null，则删除数据后，将这个属性写入null，
      以注销它的主键*/
    #endregion
    #region 传入同步集合
    /// <summary>
    /// 从数据源中删除指定的数据
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <param name="datas">待删除的数据</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class, IData;
    #endregion
    #region 传入异步集合
    /// <inheritdoc cref="Delete{Data}(IEnumerable{Data}, CancellationToken)"/>
    async Task Delete<Data>(IAsyncEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class, IData
        => await Delete(await datas.ToListAsync(), cancellation);
    #endregion
    #region 按条件删除数据
    /// <summary>
    /// 直接从数据源中删除符合指定谓词的数据，不返回结果集
    /// </summary>
    /// <param name="expression">一个用来指定删除条件的表达式</param>
    /// <inheritdoc cref="IDataPipeFrom.Query{Data}"/>
    Task Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation = default)
        where Data : class, IData;
    #endregion
    #endregion
}
