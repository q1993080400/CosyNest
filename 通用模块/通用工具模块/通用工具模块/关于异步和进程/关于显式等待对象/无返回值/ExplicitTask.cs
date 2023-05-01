namespace System.Threading.Tasks;

/// <summary>
/// 表示一个只能显式完成的可等待对象
/// </summary>
public sealed class ExplicitTask
{
    #region 公开成员
    #region 超时时间
    /// <summary>
    /// 获取超时时间，
    /// 如果它不为<see langword="null"/>，
    /// 超过这个时间后，任务自动失败
    /// </summary>
    public TimeSpan? TimeOut { get; init; }
    #endregion
    #region 通知任务已完成
    /// <summary>
    /// 调用本方法以完成任务
    /// </summary>
    public void Completed()
        => (Awaiter ??= GetAwaiter()).Completed();
    #endregion
    #region 是否成功完成
    /// <summary>
    /// 获取任务是否成功完成
    /// </summary>
    public bool IsCompleted => Awaiter?.IsCompleted ?? false;
    #endregion
    #region 提供可等待对象
    /// <summary>
    /// 提供可等待对象，
    /// 如果该对象被等待两次，会引发异常
    /// </summary>
    /// <returns></returns>
    public ExplicitAwaiter GetAwaiter()
    {
        Awaiter = Awaiter ??= new();
        if (TimeOut is { } @out)
            Task.Run(async () =>
            {
                await Task.Delay(@out);
                if (!IsCompleted)
                    throw new TimeoutException($"显式等待的执行时间超过了{@out}上限");
            });
        return Awaiter;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 缓存可等待对象
    /// <summary>
    /// 缓存可等待对象
    /// </summary>
    private ExplicitAwaiter? Awaiter { get; set; }
    #endregion
    #endregion
}
