namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个双向的数据管道，
/// 它既可以拉取又可以推送数据
/// </summary>
public interface IDataPipe : IDataPipeTo, IDataPipeFrom
{
    #region 执行事务
    #region 无返回值
    /// <summary>
    /// 执行一个事务，如果事务出现问题，
    /// 则它做出的改动全部撤销
    /// </summary>
    /// <param name="transaction">要执行的事务委托，
    /// 它可以访问一个<see cref="IDataPipe"/>对象，只要将这个对象一直传递下去，
    /// 就可以保证事务一致性</param>
    /// <returns></returns>
    Task Transaction(Func<IDataPipe, Task> transaction);
    #endregion
    #region 有返回值
    /// <summary>
    /// 执行一个事务，并返回它的返回值，如果事务出现问题，
    /// 则它做出的改动全部撤销
    /// </summary>
    /// <typeparam name="Obj">事务返回值的类型</typeparam>
    /// <inheritdoc cref="Transaction(Func{IDataPipe, Task})"/>
    Task<Obj> Transaction<Obj>(Func<IDataPipe, Task<Obj>> transaction);
    #endregion
    #endregion
}
