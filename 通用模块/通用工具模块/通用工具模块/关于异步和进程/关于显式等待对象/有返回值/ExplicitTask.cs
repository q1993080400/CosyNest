namespace System.Threading.Tasks;

/// <summary>
/// 表示一个只能显式完成的可等待对象
/// </summary>
public sealed class ExplicitTask<T>
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
    /// <param name="result">任务的返回值</param>
    public void Completed(T result)
        => GetAwaiter().Completed(result, false);
    #endregion
    #region 是否成功完成
    /// <summary>
    /// 获取任务是否成功完成
    /// </summary>
    public bool IsCompleted => Awaiter.IsCompleted;
    #endregion
    #region 提供可等待对象
    /// <summary>
    /// 提供可等待对象
    /// </summary>
    /// <returns></returns>
    public ExplicitAwaiter<T> GetAwaiter()
    {
        if ((TimeOut, IsStart) is ({ } @out, false))
            Task.Run(async () =>
            {
                await Task.Delay(@out);
                Awaiter.Completed(default, true);
            }).ConfigureAwait(false);
        IsStart = true;
        return Awaiter;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 是否启动
    /// <summary>
    /// 获取这个异步任务是否启动
    /// </summary>
    private bool IsStart { get; set; }
    #endregion
    #region 缓存可等待对象
    /// <summary>
    /// 缓存可等待对象
    /// </summary>
    private ExplicitAwaiter<T> Awaiter { get; } = new();
    #endregion
    #endregion
}
