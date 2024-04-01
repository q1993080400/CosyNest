namespace System.TimeFrancis;

/// <summary>
/// 这个类型是定时器的实现，
/// 它每天在指定的时间触发定时器
/// </summary>
/// <param name="times">每天触发定时器的时间</param>
/// <param name="canTrigger">这个委托的第一个参数是当前日期，
/// 第二个参数是已经有多少天没有触发定时器，返回值是那一天是否应该触发定时器，
/// 通过指定它，可以跳过某一天</param>
sealed class TimerFromTime(TimeOnly[] times, Func<DateOnly, int, bool> canTrigger)
{
    #region 公开成员
    #region 等待定时器到期
    /// <inheritdoc cref="TimeFrancis.Timer"/>
    public TimerInfo Timer(CancellationToken cancellationToken = default)
    {
        if (times.Length is 0)
            throw new NotSupportedException("定时器触发的时间必须至少有一个值");
        cancellationToken.ThrowIfCancellationRequested();
        var now = DateTimeOffset.Now;
        var dateTime = now.DateTime;
        var date = DateOnly.FromDateTime(dateTime);
        if (canTrigger(date, 0))
        {
            var time = TimeOnly.FromDateTime(dateTime);
            var timeOrder = times.Order().ToArray();
            var timeSpan = timeOrder.Select(x =>
            {
                var nextTime = x < time ? new DateTime(date.AddDays(1), x) : new(date, x);
                return nextTime - dateTime;
            }).Min();
            return new()
            {
                Next = dateTime + timeSpan,
                Wait = Task.Delay(timeSpan, cancellationToken)
            };
        }
        var notTriggerDay = 1;
        while (true)
        {
            var newDate = date.AddDays(notTriggerDay);
            if (canTrigger(newDate, notTriggerDay))
            {
                var nextDate = new DateTime(newDate, times.Min());
                var timeSpan = nextDate - dateTime;
                return new()
                {
                    Next = nextDate,
                    Wait = Task.Delay(timeSpan, cancellationToken)
                };
            }
            notTriggerDay++;
        }
    }
    #endregion
    #endregion
}
