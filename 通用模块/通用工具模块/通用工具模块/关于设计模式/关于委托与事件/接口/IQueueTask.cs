namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以排队执行同步任务和异步任务
/// </summary>
public interface IQueueTask
{
    #region 排队同步任务
    /// <summary>
    /// 排队执行一个同步任务
    /// </summary>
    /// <param name="task">待排队执行的任务</param>
    void Queue(Action task);
    #endregion
    #region 排队异步任务
    /// <summary>
    /// 排队执行一个异步任务
    /// </summary>
    /// <inheritdoc cref="Queue(Action)"/>
    Task Queue(Func<Task> task);

    /*问：此处使用Func<Task>，而不是Task，这有什么含义？
      答：意义在于，有些异步方法其实是同步的，它们只是为了符合接口的签名，
      执行一些同步操作（有可能非常耗时），然后直接返回IsCompleted为true的Task，
      给这种Task排队毫无意义，因此，需要把整个异步方法都包括进去*/
    #endregion
}
