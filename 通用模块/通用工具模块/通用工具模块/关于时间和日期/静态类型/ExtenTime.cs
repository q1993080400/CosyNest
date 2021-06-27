namespace System
{
    /// <summary>
    /// 关于时间和日期的扩展方法全部放在这里
    /// </summary>
    public static class ExtenTime
    {
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
        #region 关于DateTime
        #region 返回一个日期的时间
        /// <summary>
        /// 返回一个日期的时间部分
        /// </summary>
        /// <param name="date">待返回时间部分的日期</param>
        /// <returns>该日期的时间部分</returns>
        public static TimeOfDay TimeOfDay(this DateTime date)
            => new(date.Hour, date.Minute, date.Second, date.Millisecond);
        #endregion
        #region 替换时间
        /// <summary>
        /// 替换一个<see cref="DateTime"/>的时间，
        /// </summary>
        /// <param name="date">待替换时间的<see cref="DateTime"/></param>
        /// <param name="newTime">新替换的时间</param>
        /// <returns>时间被替换后的新<see cref="DateTime"/>，
        /// 除了一天内的时间部分不同以外，其他部分与原始对象相同</returns>
        public static DateTime ReplaceTime(this DateTime date, TimeOfDay newTime)
            => date.Date + newTime.DistanceMidnight;
        #endregion
        #endregion
        #region 关于DateTimeOffset
        #region 返回一个日期的时间
        /// <summary>
        /// 返回一个日期的时间部分
        /// </summary>
        /// <param name="date">待返回时间部分的日期</param>
        /// <returns>该日期的时间部分，注意，它代表当前时区的时间</returns>
        public static TimeOfDay TimeOfDay(this DateTimeOffset date)
            => date.DateTime.TimeOfDay();
        #endregion
        #region 解构DateTimeOffset
        /// <summary>
        /// 将<see cref="DateTimeOffset"/>解构为日期和时区偏移量
        /// </summary>
        /// <param name="date">待解构的<see cref="DateTimeOffset"/></param>
        /// <param name="dateTime">用来接收日期的对象</param>
        /// <param name="offset">用来接收时区偏移量的对象</param>
        public static void Deconstruct(this DateTimeOffset date, out DateTime dateTime, out TimeSpan offset)
        {
            dateTime = date.DateTime;
            offset = date.Offset;
        }
        #endregion
        #region 替换时间
        /// <summary>
        /// 替换一个<see cref="DateTimeOffset"/>的时间
        /// </summary>
        /// <param name="date">待替换时间的<see cref="DateTimeOffset"/></param>
        /// <param name="newTime">新替换的时间</param>
        /// <returns>时间被替换后的新<see cref="DateTimeOffset"/>，
        /// 除了一天内的时间部分不同以外，其他部分与原始对象相同</returns>
        public static DateTimeOffset ReplaceTime(this DateTimeOffset date, TimeOfDay newTime)
            => new(date.Date.ReplaceTime(newTime), date.Offset);
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
}
