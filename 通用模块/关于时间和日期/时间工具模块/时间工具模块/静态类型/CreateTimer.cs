namespace System.TimeFrancis;

/// <summary>
/// 这个静态类可以用来创建与时间和计划任务有关的对象
/// </summary>
public static class CreateTimer
{
    #region 创建定时器
    #region 按照周期重复触发
    /// <summary>
    /// 创建一个<see cref="Timer"/>，
    /// 它从指定的时间开始，按照指定的周期触发
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="TimeFrancis.TimerCycle(TimeSpan, DateTimeOffset?)"/>
    public static Timer TimerCycle(TimeSpan interval, DateTimeOffset? startTime = null)
        => new TimerCycle(interval, startTime).Timer;
    #endregion
    #region 在每天固定的时间触发
    /// <summary>
    /// 创建一个定时器的实现，
    /// 它每天在指定的时间触发定时器
    /// </summary>
    /// <param name="times">每天触发定时器的时间</param>
    /// <returns></returns>
    public static Timer TimerFromTime(IEnumerable<TimeOnly> times)
    {
        var timeOrder = times.Order().ToArray();
        if (timeOrder.Length is 0)
            throw new NotSupportedException("定时器触发的时间必须至少有一个值");
        return cancellationToken =>
        {
            if (cancellationToken.IsCancellationRequested)
                return null;
            var dateTime = DateTimeOffset.Now.DateTime;
            var date = DateOnly.FromDateTime(dateTime);
            #region 用来等待的本地函数
            async Task<bool> Wait(TimeSpan delay)
            {
                using var timer = new PeriodicTimer(delay);
                return await timer.WaitForNextTickAsync(cancellationToken);
            }
            #endregion
            var time = TimeOnly.FromDateTime(dateTime);
            var timeSpan = timeOrder.Select(x =>
            {
                var nextTime = new DateTime(date.AddDays(x < time ? 1 : 0), x);
                return nextTime - dateTime;
            }).Min();
            return new()
            {
                NextTimeState = (NextTimeState.HasNext, dateTime + timeSpan),
                Wait = Wait(timeSpan)
            };
        };
    }
    #endregion
    #region 仅执行一次
    /// <summary>
    /// 创建一个仅执行一次的定时器
    /// </summary>
    /// <param name="startDate">指示开始执行的时间，
    /// 如果它为<see langword="null"/>，或比当前时间要早，则立即执行</param>
    /// <returns></returns>
    public static Timer TimerDisposable(DateTimeOffset? startDate = null)
    {
        var execute = false;
        return cancellationToken =>
        {
            if (execute || cancellationToken.IsCancellationRequested)
                return null;
            execute = true;
            #region 返回立即执行的TimerInfo
            static TimerInfo Immediately()
            => new()
            {
                Wait = Task.FromResult(true),
                NextTimeState = (NextTimeState.NotNext, default)
            };
            #endregion
            if (startDate is { } startDateValue)
            {
                var delay = startDateValue - DateTimeOffset.Now;
                #region 用来等待的本地函数
                async Task<bool> Wait()
                {
                    await Task.Delay(delay, cancellationToken);
                    return true;
                }
                #endregion
                return delay > TimeSpan.Zero ?
                new()
                {
                    NextTimeState = (NextTimeState.NotNext, default),
                    Wait = Wait()
                } : Immediately();
            }
            return Immediately();
        };
    }
    #endregion
    #endregion
}
