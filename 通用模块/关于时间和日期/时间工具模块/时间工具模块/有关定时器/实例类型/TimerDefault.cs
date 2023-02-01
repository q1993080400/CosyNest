namespace System.TimeFrancis;

/// <summary>
/// 这个类型是定时器的默认实现
/// </summary>
sealed class TimerDefault
{
    #region 公开成员
    #region 等待定时器到期
    /// <inheritdoc cref="TimeFrancis.Timer"/>
    public async Task Timer()
    {
        var now = DateTimeOffset.Now;
        if (now < StartTime)
        {
            Count++;
            await Task.Delay(StartTime - now);
            return;
        }
        var time = StartTime + Count * Interval;
        var span = time - now;
        Count++;
        if (span.TotalMilliseconds > 0)
            await Task.Delay(span);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 定时器开始的时间
    /// <summary>
    /// 返回定时器第一次触发的时间，
    /// 如果这个时间发生在未来，
    /// 则程序会一直等待到那个时间才会启动定时器
    /// </summary>
    private DateTimeOffset StartTime { get; }
    #endregion
    #region 定时器触发间隔
    /// <summary>
    /// 返回定时器的触发间隔
    /// </summary>
    private TimeSpan Interval { get; }
    #endregion
    #region 定时器触发的次数
    /// <summary>
    /// 获取定时器触发的次数
    /// </summary>
    private double Count { get; set; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="interval">定时器的触发间隔</param>
    /// <param name="startTime">定时器第一次触发的时间，
    /// 如果这个时间发生在未来，则程序会一直等待到那个时间才会启动定时器</param>
    public TimerDefault(TimeSpan interval, DateTimeOffset? startTime)
    {
        ExceptionIntervalOut.Check(1d, null, interval.TotalMilliseconds);
        var now = DateTimeOffset.Now;
        StartTime = startTime ?? now;
        if (startTime < now)
            throw new NotSupportedException($"定时器开始的时间{startTime}早于现在时间，无法创建这个定时器");
        Interval = interval;
    }
    #endregion
}
