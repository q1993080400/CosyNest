namespace System.TimeFrancis;

/// <summary>
/// 这个类型是定时器的实现，
/// 它按照固定的周期触发定时器
/// </summary>
sealed class TimerCycle
{
    #region 公开成员
    #region 等待定时器到期
    /// <inheritdoc cref="TimeFrancis.Timer"/>
    public TimerInfo Timer(CancellationToken cancellationToken = default)
    {
        #region 进行等待的本地函数
        async Task Wait()
        {
            if (Immediately)
            {
                Immediately = false;
                return;
            }
            var delay = Next - DateTimeOffset.Now;
            if (delay > TimeSpan.Zero)
                await Task.Delay(delay, cancellationToken);
            Next = DateTimeOffset.Now + Interval;
        }
        #endregion
        cancellationToken.ThrowIfCancellationRequested();
        return new()
        {
            Next = Next,
            Wait = Wait()
        };
    }
    #endregion
    #endregion
    #region 内部成员
    #region 下次触发的时间
    /// <summary>
    /// 获取下次触发的时间
    /// </summary>
    private DateTimeOffset Next { get; set; }
    #endregion
    #region 定时器触发间隔
    /// <summary>
    /// 返回定时器的触发间隔
    /// </summary>
    private TimeSpan Interval { get; }
    #endregion
    #region 是否立即触发
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则立即触发一次定时器，否则等待定时器完成后才触发
    /// </summary>
    private bool Immediately { get; set; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="interval">定时器的触发间隔</param>
    /// <param name="startTime">定时器第一次触发的时间，
    /// 如果这个时间发生在过去，则引发一个异常</param>
    /// <param name="immediately">如果这个值为<see langword="true"/>，
    /// 则立即触发一次定时器，否则等待定时器完成后才触发</param>
    public TimerCycle(TimeSpan interval, DateTimeOffset startTime, bool immediately)
    {
        ExceptionIntervalOut.Check(1d, null, interval.TotalMilliseconds);
        if (startTime < DateTimeOffset.Now)
            throw new NotSupportedException($"定时器开始的时间{startTime}早于现在时间，无法创建这个定时器");
        Next = startTime;
        Interval = interval;
        Immediately = immediately;
    }
    #endregion
}
