using System.Time;

namespace System
{
    /// <summary>
    /// 有关时间和日期的扩展方法全部放在这里
    /// </summary>
    public static class ExtenTime
    {
        #region 关于NextDate
        #region 计算当前时间过后，下一次执行定时器的时间
        /// <summary>
        /// 计算当前时间过后，下一次执行定时器的时间
        /// </summary>
        /// <param name="nextDate">用来计算定时器触发时间的委托</param>
        /// <returns>下一次执行定时器的时间，
        /// 它等同向<paramref name="nextDate"/>传入<see cref="DateTimeOffset.Now"/>后的结果</returns>
        public static DateTimeOffset? NextAtNow(this NextDate nextDate)
            => nextDate(DateTimeOffset.Now);
        #endregion
        #endregion
    }
}
