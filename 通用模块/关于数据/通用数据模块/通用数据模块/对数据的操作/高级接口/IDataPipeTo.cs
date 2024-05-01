using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以作为一个支持将数据更改推送到数据源的管道
/// </summary>
public interface IDataPipeTo : IDataContext
{
    #region 关于添加与更新数据
    /// <summary>
    /// 通过管道将数据添加到数据源，
    /// 如果数据存在，则将其更新
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <param name="datas">待添加或更新的数据</param>
    /// <param name="isAdd">这个委托传入数据，返回一个布尔值，
    /// 如果为<see langword="true"/>，表示添加数据，否则表示更新数据，
    /// 如果不指定，则自动判断，在显式指定主键的情况下，必须显式指定这个委托，否则会出现异常</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    Task AddOrUpdate<Data>(IEnumerable<Data> datas, Func<Data, bool>? isAdd = null, CancellationToken cancellation = default)
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
