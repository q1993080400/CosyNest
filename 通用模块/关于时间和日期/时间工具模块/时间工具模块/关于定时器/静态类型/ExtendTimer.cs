using System.TimeFrancis;

using Timer = System.TimeFrancis.Timer;

namespace System;

/// <summary>
/// 有关时间和计划任务的扩展方法全部放在这个类型中
/// </summary>
public static partial class ExtendTimer
{
    #region 返回一个限制次数的定时器
    /// <summary>
    /// 返回一个限制次数的定时器，
    /// 当原始定时器触发到指定的次数时，会自动停止
    /// </summary>
    /// <param name="timer">原始定时器</param>
    /// <param name="maxCount">定时器触发的最大次数</param>
    /// <returns></returns>
    public static Timer LimitFrequency(this Timer timer, int maxCount)
        => cancellationToken =>
        {
            if (maxCount <= 0 || cancellationToken.IsCancellationRequested)
                return null;
            var info = timer(cancellationToken);
            maxCount--;
            return (info, maxCount) switch
            {
                (null, _) => null,
                ({ }, <= 0) => info with
                {
                    NextTimeState = (NextTimeState.NotNext, default)
                },
                _ => info
            };
        };
    #endregion
    #region 返回一个只在指定日期触发的定时器
    #region 可指定任意逻辑
    /// <summary>
    /// 返回一个定时器，
    /// 它只在指定日期触发，
    /// 当在其他日期运行它的时候，
    /// 触发日期会被顺延
    /// </summary>
    /// <param name="timer">原始定时器，函数会先运行它，然后判断它的触发时间是否在指定的日期</param>
    /// <param name="canContinue">这个委托的第一个参数是当前日期，
    /// 第二个参数是当前已经跳过多少天没有触发，从0开始，
    /// 如果返回<see langword="true"/>，表示那一天应该触发，否则表示不应触发</param>
    /// <returns></returns>
    public static Timer OnlyAppointDate(this Timer timer, Func<DateOnly, int, Task<bool>> canContinue)
    {
        var lastDate = DateOnly.FromDateTime(DateTime.Now);
        return cancellationToken =>
        {
            if (cancellationToken.IsCancellationRequested)
                return null;
            var info = timer(cancellationToken);
            if (info is null)
                return null;
            #region 用于等待的本地函数
            async Task<bool> Wait(TimerInfo info)
            {
                if (!await info.Wait())
                    return false;
                var now = DateTime.Now;
                var nowDate = DateOnly.FromDateTime(now);
                if (await canContinue(nowDate, nowDate.DayNumber - lastDate.DayNumber))
                {
                    lastDate = nowDate;
                    return true;
                }
                if (info is { NextTimeState: (NextTimeState.NotNext, _) })
                    return false;
                var delay = nowDate.AddDays(1).ToDateTime(default) - now;
                await Task.Delay(delay, cancellationToken);
                var nextInfo = timer(cancellationToken);
                return nextInfo is null || await Wait(nextInfo);
            }
            #endregion
            return new()
            {
                NextTimeState = (NextTimeState.Ambiguity, default),
                Wait = () => Wait(info)
            };
        };
    }
    #endregion
    #region 只在工作日或节假日触发
    /// <summary>
    /// 返回一个定时器，
    /// 它只在工作日或节假日触发
    /// </summary>
    /// <param name="timer">原始定时器，函数会先运行它，然后判断它的触发时间是否在工作日或节假日</param>
    /// <param name="judgmentHoliday">用于判断节假日的委托</param>
    /// <param name="onlyHoliday">如果这个值为<see langword="true"/>，
    /// 表示仅在假日触发，否则表示仅在工作日触发</param>
    /// <returns></returns>
    public static Timer OnlyWeekdayOrHoliday(this Timer timer, JudgmentHoliday judgmentHoliday, bool onlyHoliday = false)
        => timer.OnlyAppointDate(async (date, _) =>
        {
            var judgment = await judgmentHoliday(date);
            return onlyHoliday ? judgment : !judgment;
        });
    #endregion
    #endregion
}
