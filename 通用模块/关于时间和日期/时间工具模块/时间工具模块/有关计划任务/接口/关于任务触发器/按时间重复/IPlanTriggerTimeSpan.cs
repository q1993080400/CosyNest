namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视为一个按照指定<see cref="TimeSpan"/>重复的计划任务触发器
    /// </summary>
    public interface IPlanTriggerTimeSpan : IPlanTriggerTiming
    {
        #region 重复执行的间隔
        /// <summary>
        /// 获取重复执行计划任务的间隔，
        /// 如果为<see langword="null"/>，代表不会重复
        /// </summary>
        TimeSpan? Interval { get; }
        #endregion
        #region 第一次执行的时间
        /// <summary>
        /// 获取第一次执行计划任务的时间
        /// </summary>
        DateTimeOffset Begin { get; }
        #endregion
    }
}
