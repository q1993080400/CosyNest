namespace System;

/// <summary>
/// 关于时间和日期的扩展方法全部放在这里
/// </summary>
public static class ExtenTime
{
    #region 关于DateTimeOffset
    #region 返回日期部分
    /// <summary>
    /// 返回一个<see cref="DateTimeOffset"/>的日期部分，
    /// 时间部分会被舍去
    /// </summary>
    /// <param name="date">要返回日期的<see cref="DateTimeOffset"/></param>
    /// <returns></returns>
    public static DateTimeOffset ToDay(this DateTimeOffset date)
        => date - date.TimeOfDay;
    #endregion
    #endregion
    #region 关于TimeOnly
    #region 返回DateTimeOffset的时间部分
    /// <summary>
    /// 返回一个<see cref="DateTimeOffset"/>的时间部分
    /// </summary>
    /// <param name="offset">待返回时间部分的<see cref="DateTimeOffset"/></param>
    /// <returns></returns>
    public static TimeOnly Time(this DateTimeOffset offset)
        => new(offset.DateTime.TimeOfDay.Ticks);
    #endregion
    #endregion
    #region 关于TimeSpan
    #region 对TimSpan进行取余运算
    #region 传入任意数字
    /// <summary>
    /// 对一个<see cref="TimeSpan"/>进行取余运算
    /// </summary>
    /// <param name="divided">取余的被除数</param>
    /// <param name="divisor">取余的除数</param>
    /// <returns>取余运算之后的结果</returns>
    public static TimeSpan Modulus(this TimeSpan divided, Num divisor)
    {
        Num ticks = divided.Ticks;
        return TimeSpan.FromTicks(ticks % divisor);
    }
    #endregion
    #region 传入另一个TimeSpan
    /// <summary>
    /// 对一个<see cref="TimeSpan"/>进行取余运算
    /// </summary>
    /// <param name="divided">取余的被除数</param>
    /// <param name="divisor">取余的除数</param>
    /// <returns>取余运算之后的结果</returns>
    public static TimeSpan Modulus(this TimeSpan divided, TimeSpan divisor)
        => divided.Modulus(divisor.Ticks);
    #endregion
    #endregion
    #endregion
    #region 关于DayOfWeek
    #region 计算任意两个星期几间隔的天数
    /// <summary>
    /// 计算任意两个星期几间隔的天数
    /// </summary>
    /// <param name="current">当前星期几</param>
    /// <param name="week">另一个星期几</param>
    /// <returns>一个元组，分别指示<paramref name="current"/>，
    /// 距离上一个和下一个<paramref name="week"/>的天数</returns>
    public static (int Last, int Next) IntervalDay(this DayOfWeek current, DayOfWeek week)
    {
        var poor = (int)current - (int)week;
        return (poor > 0 ? -poor : -7 - poor,
            poor >= 0 ? 7 - poor : -poor);
    }
    #endregion
    #endregion
}
