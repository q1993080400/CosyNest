namespace System;

/// <summary>
/// 关于时间和日期的扩展方法全部放在这里
/// </summary>
public static class ExtendTime
{
    #region 关于DateTimeOffset
    #region 返回日期部分
    /// <summary>
    /// 返回一个<see cref="DateTimeOffset"/>的日期部分，
    /// 时间部分会被舍去
    /// </summary>
    /// <param name="date">要返回日期的<see cref="DateTimeOffset"/></param>
    /// <returns></returns>
    public static DateTimeOffset Today(this DateTimeOffset date)
        => date - date.TimeOfDay;
    #endregion
    #region 返回TimeOnly
    /// <summary>
    /// 返回一个<see cref="DateTimeOffset"/>的时间部分
    /// </summary>
    /// <param name="dateTimeOffset">待返回时间部分的<see cref="DateTimeOffset"/></param>
    /// <returns></returns>
    public static TimeOnly TimeOnly(this DateTimeOffset dateTimeOffset)
    {
        var (_, time, _) = dateTimeOffset;
        return time;
    }
    #endregion
    #region 返回DateOnly
    /// <summary>
    /// 以<see cref="System.DateOnly"/>的形式，
    /// 返回一个<see cref="DateTimeOffset"/>的日期部分
    /// </summary>
    /// <param name="dateTimeOffset">待返回日期部分的<see cref="DateTimeOffset"/></param>
    /// <returns></returns>
    public static DateOnly DateOnly(this DateTimeOffset dateTimeOffset)
    {
        var (date, _, _) = dateTimeOffset;
        return date;
    }
    #endregion
    #endregion
    #region 关于TimeSpan
    #region 格式化TimeSpan
    /// <summary>
    /// 将时间间隔格式化为常用格式
    /// </summary>
    /// <param name="timeSpan">要格式化的时间间隔</param>
    /// <returns></returns>
    public static string Format(this TimeSpan timeSpan)
    {
        var text = "";
        if (timeSpan.Days != 0)
            text += $"{timeSpan.Days}天";
        if (timeSpan.Hours != 0)
            text += $"{timeSpan.Hours}小时";
        if (timeSpan.Minutes != 0)
            text += $"{timeSpan.Minutes}分";
        if (timeSpan.Seconds != 0)
            text += $"{timeSpan.Seconds}秒";
        return text;
    }
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
