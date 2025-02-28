namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都表示在删除它的时候，可能产生副作用，
/// 副作用指的是：为了彻底删除它，所需要做的准备，
/// 例如删除关联实体，删除保存在硬盘上的文件等
/// </summary>
/// <typeparam name="Entity">实体的类型</typeparam>
public interface IHasDeleteSideEffect<in Entity>
{
    #region 执行删除实体的副作用
    /// <summary>
    /// 执行删除实体时产生的副作用
    /// </summary>
    /// <param name="entities">要删除的实体</param>
    /// <param name="serviceProvider">用于请求服务的对象</param>
    /// <returns></returns>
    static abstract Task InvokeDeleteSideEffect(IQueryable<Entity> entities, IServiceProvider serviceProvider);
    #endregion
}

/// <summary>
/// 这个静态类是<see cref="IHasDeleteSideEffect{Entity}"/>的辅助类
/// </summary>
public static class IHasDeleteSideEffect
{
    #region 返回删除它产生的副作用
    /// <summary>
    /// 返回删除这些实体所产生的副作用，
    /// 如果没有副作用，则返回<see langword="null"/>
    /// </summary>
    /// <typeparam name="DeleteEntity">要删除的实体的类型</typeparam>
    /// <param name="entities">要删除的实体</param>
    /// <returns></returns>
    public static Func<IServiceProvider, Task>? DeleteSideEffect<DeleteEntity>(IReadOnlyCollection<DeleteEntity> entities)
        where DeleteEntity : IHasDeleteSideEffect<DeleteEntity>
        => entities.Count is 0 ?
        null :
        serviceProvider => DeleteEntity.InvokeDeleteSideEffect(entities.AsQueryable(), serviceProvider);
    #endregion
}
