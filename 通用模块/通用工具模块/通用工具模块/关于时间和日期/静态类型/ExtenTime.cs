using System.Globalization;
using System.MathFrancis;

namespace System;

/// <summary>
/// 关于时间和日期的扩展方法全部放在这里
/// </summary>
public static class ExtenTime
{
    #region 关于DateTime
    #region 返回两个日期之间的年数
    /// <summary>
    /// 返回两个日期之间的年数，
    /// 可能含有小数和负数，
    /// 如果开始日期比结束日期晚，则为负数，
    /// 注意：它只计算日期，会忽略时间部分
    /// </summary>
    /// <param name="start">开始日期</param>
    /// <param name="end">结束日期，
    /// 如果为<see langword="null"/>，则自动获取当前日期</param>
    public static decimal TotalYears(this DateTime start, DateTime? end = null)
    {
        var current = (end ?? DateTime.Now).Date;
        start = start.Date;
        var calendar = new GregorianCalendar();
        var year = current.Year - start.Year;
        var fullYear = calendar.AddYears(start, year);
        var residue = (decimal)(fullYear - current).TotalDays;
        var residueYear = residue / calendar.GetDaysInYear(fullYear.Year);
        return year - residueYear;
    }
    #endregion
    #region 将日期加上指定的年数
    /// <summary>
    /// 将日期加上指定的年数，
    /// 并返回新的日期，
    /// 注意：它只计算日期，会忽略时间部分
    /// </summary>
    /// <param name="dateTime">要加上年数的日期</param>
    /// <param name="years">要增加的年数，可以为小数或负数</param>
    /// <returns></returns>
    public static DateTime AddYears(this DateTime dateTime, decimal years)
    {
        dateTime = dateTime.Date;
        var calendar = new GregorianCalendar();
        var (integer, @decimal, _) = ToolArithmetic.Split(years);
        var newDate = calendar.AddYears(dateTime, (int)integer);
        var days = calendar.GetDaysInYear(newDate.Year + (years > 0 ? 1 : -1));
        return newDate.AddDays(@decimal * days);
    }
    #endregion
    #endregion
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
    #region 返回两个日期之间的年数
    /// <inheritdoc cref="TotalYears(DateTime, DateTime?)"/>
    public static decimal TotalYears(this DateTimeOffset start, DateTimeOffset? end = null)
        => start.DateTime.TotalYears((end ?? DateTimeOffset.Now).DateTime);
    #endregion
    #region 将日期加上指定的年数
    /// <inheritdoc cref="AddYears(DateTime, decimal)"/>
    public static DateTimeOffset AddYears(this DateTimeOffset dateTime, decimal years)
    {
        var newDate = dateTime.DateTime.AddYears(years);
        return new(newDate, dateTime.Offset);
    }
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
