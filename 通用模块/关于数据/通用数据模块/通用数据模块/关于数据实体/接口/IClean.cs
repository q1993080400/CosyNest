namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个支持清理资源的数据实体，
/// 按照规范，它被从数据库中删除时，
/// 应该执行一些收尾操作
/// </summary>
/// <typeparam name="Entity">实体的类型</typeparam>
public interface IClean<in Entity>
{
    #region 说明文档
    /*问：为什么清理实体的方法设计为一个静态抽象方法，
      而不是实例方法？
      答：这是为性能考虑，批量清理实体，
      比起一个个枚举实体，然后清理它们更好优化，
      尤其是清理实体时，需要网络连接的情况下，
      你可以将它们合并为一个请求，而不是逐个发起网络请求*/
    #endregion
    #region 清理实体
    /// <summary>
    /// 删除实体，并执行收尾操作
    /// </summary>
    /// <param name="entities">要执行删除和收尾操作的实体</param>
    /// <param name="dataPipe">数据管道对象，它可以用于删除数据</param>
    /// <param name="serviceProvider">一个用来请求服务的对象</param>
    /// <returns></returns>
    abstract static Task CleanAndDelete(IQueryable<Entity> entities, IDataPipe dataPipe, IServiceProvider serviceProvider);
    #endregion
}
