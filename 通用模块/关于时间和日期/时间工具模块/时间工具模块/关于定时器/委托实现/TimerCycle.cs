﻿namespace System.TimeFrancis;

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
            if (Immediately)
            {
                Immediately = false;
                Next = DateTimeOffset.Now + Interval;
                return true;
            }
            var delay = Next - DateTimeOffset.Now;
            if (delay <= TimeSpan.Zero)
                throw new NotSupportedException($"定时器不得在已经过去的时间触发");
            using var timer = new PeriodicTimer(delay);
            var success = await timer.WaitForNextTickAsync(cancellationToken);
            Next = DateTimeOffset.Now + Interval;
            return success;
        }
        #endregion
        return cancellationToken.IsCancellationRequested ?
            null :
            new()
            {
                NextTimeState = (NextTimeState.HasNext, Next.Add(Interval)),
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
    /// 如果这个时间发生在过去，则引发一个异常，
    /// 如果为<see langword="null"/>，立即触发一次定时器</param>
    public TimerCycle(TimeSpan interval, DateTimeOffset? startTime)
    {
        if (interval.TotalMilliseconds < 1)
            throw new NotSupportedException($"定时器目前的触发周期是{interval}，它不能小于1毫秒");
        var now = DateTimeOffset.Now;
        var startTimeValue = startTime ?? now;
        if (startTimeValue < now)
            throw new NotSupportedException($"定时器开始的时间{startTimeValue}早于现在时间，无法创建这个定时器");
        Next = startTimeValue;
        Interval = interval;
        Immediately = startTimeValue == now;
    }
    #endregion
}
