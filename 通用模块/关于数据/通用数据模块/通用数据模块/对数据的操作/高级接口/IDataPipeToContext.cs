using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个用来推送数据的管道上下文
/// </summary>
public interface IDataPipeToContext : IDataPipeFromContext
{
    #region 关于添加与更新数据
    /// <summary>
    /// 通过管道将数据添加到数据源，
    /// 如果数据存在，则将其更新
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <param name="datas">待添加或更新的数据</param>
    /// <param name="info">用来添加或更新数据的参数</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    Task AddOrUpdate<Data>(IEnumerable<Data> datas, AddOrUpdateInfo<Data>? info = null, CancellationToken cancellation = default)
        where Data : class;
    #endregion
    #region 关于删除数据
    #region 传入实体集合
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
