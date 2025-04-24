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
    public TimerInfo? Timer(CancellationToken cancellationToken = default)
    {
        #region 进行等待的本地函数
        async Task<bool> Wait()
        {
            #region 返回等待时间的本地函数
            TimeSpan GetDelay()
            {
                if (StartTime is { } startTime)
                {
                    StartTime = null;
                    var delay = startTime - DateTimeOffset.Now;
                    return delay > TimeSpan.Zero ? delay : TimeSpan.FromMilliseconds(1);
                }
                return Interval;
            }
            #endregion
            var delay = GetDelay();
            using var timer = new PeriodicTimer(delay);
            return await timer.WaitForNextTickAsync(cancellationToken);
        }
        #endregion
        return cancellationToken.IsCancellationRequested ?
            null :
            new()
            {
                NextTimeState = (NextTimeState.HasNext, (StartTime ?? DateTimeOffset.Now).Add(Interval)),
                Wait = Wait
            };
    }
    #endregion
    #endregion
    #region 内部成员
    #region 定时器启动的时间
    /// <summary>
    /// 获取定时器启动的时间，
    /// 如果为<see langword="null"/>，
    /// 或者这个时间发生在过去，表示立即启动
    /// </summary>
    private DateTimeOffset? StartTime { get; set; }
    #endregion
    #region 定时器触发间隔
    /// <summary>
    /// 返回定时器的触发间隔
    /// </summary>
    private TimeSpan Interval { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="interval">定时器的触发间隔</param>
    /// <param name="startTime">定时器第一次触发的时间，
    /// 如果为<see langword="null"/>，或者这个时间发生在过去，立即触发一次定时器</param>
    public TimerCycle(TimeSpan interval, DateTimeOffset? startTime)
    {
        if (interval.TotalMilliseconds < 1)
            throw new NotSupportedException($"定时器目前的触发周期是{interval}，它不能小于1毫秒");
        Interval = interval;
        StartTime = startTime ?? DateTimeOffset.Now;
    }
    #endregion
}
