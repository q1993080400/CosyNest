namespace System.Design;

/// <summary>
/// 该类型是<see cref="IQueueTask"/>的实现，
/// 它实际上不进行排队，而是独占一个任务
/// </summary>
sealed class QueueTaskExclusive : IQueueTask
{
    #region 说明文档
    /*问：本对象实际上不进行排队，
      而是一次只允许执行一个任务，这有什么意义？
      答：意义在于在进行前端开发时，通过本对象可以避免事件反复触发*/
    #endregion
    #region 关于同步任务
    #region 独占的任务
    /// <summary>
    /// 当此属性为<see langword="true"/>时，
    /// 不允许执行其他同步任务
    /// </summary>
    private bool SynchronizationLocking { get; set; }
    #endregion
    #region 指示InSynchronizationLocking是否执行过
    /// <summary>
    /// 当此属性为<see langword="true"/>时，
    /// 代表<see cref="InSynchronizationLocking"/>已经执行，不可重复执行
    /// </summary>
    private bool SynchronizationInImplement { get; set; }
    #endregion
    #region 当任务被抢占时执行的委托
    /// <summary>
    /// 当同步任务被抢占时，执行这个委托
    /// </summary>
    private Action? InSynchronizationLocking { get; }
    #endregion
    #region 执行任务
    public void Queue(Action task)
    {
        if (!SynchronizationLocking)
        {
            using var _ = FastRealize.Disposable(
                () => SynchronizationLocking = true,
                () =>
                {
                    SynchronizationInImplement = false;
                    SynchronizationLocking = false;
                });
            task();
        }
        else if (!SynchronizationInImplement && InSynchronizationLocking is { })
        {
            SynchronizationInImplement = true;
            InSynchronizationLocking.Invoke();
        }
    }
    #endregion
    #endregion
    #region 关于异步任务
    #region 独占的任务
    /// <summary>
    /// 当此属性为<see langword="true"/>时，
    /// 不允许执行其他异步任务
    /// </summary>
    private bool AsynchronousLocking { get; set; }
    #endregion
    #region 指示InAsynchronousLocking是否执行过
    /// <summary>
    /// 当此属性为<see langword="true"/>时，
    /// 代表<see cref="InAsynchronousLocking"/>已经执行，不可重复执行
    /// </summary>
    private bool AsynchronousInImplement { get; set; }
    #endregion
    #region 当任务被抢占时执行的委托
    /// <summary>
    /// 当异步任务被抢占时，执行这个委托
    /// </summary>
    private Func<Task>? InAsynchronousLocking { get; }
    #endregion
    #region 执行任务
    public async Task Queue(Func<Task> task)
    {
        if (!AsynchronousLocking)
        {
            using var _ = FastRealize.Disposable(
                () => AsynchronousLocking = true,
                () =>
                {
                    AsynchronousInImplement = false;
                    AsynchronousLocking = false;
                });
            await task();
        }
        else if (!AsynchronousInImplement && InAsynchronousLocking is { })
        {
            AsynchronousInImplement = true;
            await InAsynchronousLocking();
        }
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="inSynchronizationLocking">当同步任务被抢占时，执行这个委托，
    /// 如果为<see langword="null"/>，则不执行</param>
    /// <param name="inAsynchronousLocking">当异步任务被抢占时，执行这个委托，
    /// 如果为<see langword="null"/>，则不执行</param>
    public QueueTaskExclusive(Action? inSynchronizationLocking, Func<Task>? inAsynchronousLocking)
    {
        InSynchronizationLocking = inSynchronizationLocking;
        InAsynchronousLocking = inAsynchronousLocking;
    }
    #endregion
}
