namespace System.Business;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个业务层数据源
/// </summary>
/// <typeparam name="Obj">数据的类型</typeparam>
public interface IBusinessDataPipe<Obj>
    where Obj : class
{
    #region 根据ID查找数据
    /// <summary>
    /// 根据ID查找数据
    /// </summary>
    /// <param name="id">数据的ID</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<Obj> Query(Guid id, CancellationToken cancellationToken = default);
    #endregion
    #region 添加或修改数据
    /// <summary>
    /// 添加或修改数据
    /// </summary>
    /// <param name="objs">待添加或修改的数据</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task AddOrUpdate(IEnumerable<Obj> objs, CancellationToken cancellationToken = default);
    #endregion
    #region 删除数据
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="objIDs">数据的</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Delete(IEnumerable<Guid> objIDs, CancellationToken cancellationToken = default);
    #endregion
}
