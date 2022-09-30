namespace System.TimeFrancis;

/// <summary>
/// 这个静态类可以用来创建定时器
/// </summary>
public static class CreateTimer
{
    #region 创建定时器
    #region 按照时间周期等待
    /// <summary>
    /// 创建一个定时器
    /// </summary>
    /// <param name="cycle">定时器的时间周期</param>
    /// <param name="goNow">如果这个值为<see langword="true"/>，
    /// 则在第一次触发定时器时，无需等待，立即触发该周期</param>
    /// <returns></returns>
    public static ITimer Timer(TimeSpan cycle, bool goNow =false)
        => new TimerDefault()
        {
            Cycle = cycle,
            GoNow = goNow,
        };
    #endregion
    #region 按毫秒等待
    /// <param name="cycle">定时器的时间周期，按毫秒计算</param>
    /// <inheritdoc cref="Timer(TimeSpan, bool)"/>
    public static ITimer Timer(int cycle, bool goNow = false)
        => Timer(TimeSpan.FromMilliseconds(cycle),goNow);
    #endregion
    #endregion
}
