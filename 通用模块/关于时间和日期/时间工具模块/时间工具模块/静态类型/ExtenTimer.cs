using System.TimeFrancis;

using Timer = System.TimeFrancis.Timer;

namespace System;

/// <summary>
/// 有关时间和计划任务的扩展方法全部放在这个类型中
/// </summary>
public static class ExtenTimer
{
    #region 返回一个限制次数的定时器
    /// <summary>
    /// 返回一个新的定时器，
    /// 当原始定时器触发一定次数后，自动停止它
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="TimerLimitFrequency(Timer, int)"/>
    public static Timer LimitFrequency(this Timer timer, int maxCount)
        => new TimerLimitFrequency(timer, maxCount).Timer;
    #endregion
}
