namespace System.DataFrancis;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以作为一个支持将数据更改推送到数据源的管道
/// </summary>
public interface IDataPipeTo : IDataContext
{
    #region 执行推送上下文
    #region 无返回值
    /// <summary>
    /// 执行推送任务，它可以把数据推送到数据源，
    /// 推送任务完成后，会自动保存数据库更改
    /// </summary>
    /// <param name="execute">这个委托的参数是推送上下文，
    /// 当它执行完毕的时候，会自动将推送提交到数据源</param>
    /// <returns></returns>
    Task Push(Func<IDataPipeToContext, Task> execute);
    #endregion
    #region 有返回值
    /// <summary>
    /// 执行推送任务，它可以把数据推送到数据源，
    /// 然后返回任务的返回值，
    /// 推送任务完成后，会自动保存数据库更改
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Push(Func{IDataPipeToContext, Task})"/>
    Task<Obj> Push<Obj>(Func<IDataPipeToContext, Task<Obj>> execute);
    #endregion
    #endregion
}
