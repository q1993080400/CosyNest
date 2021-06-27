namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视为一个按日期重复的计划任务触发器
    /// </summary>
    public interface IPlanTriggerDate : IPlanTriggerTiming
    {
        #region 计划任务的创建日期
        /// <summary>
        /// 获取计划任务的创建日期，
        /// 计划任务的执行时间必然晚于它
        /// </summary>
        DateTimeOffset CreateDate { get; }
        #endregion
        #region 计划任务的执行时间
        /// <summary>
        /// 获取计划任务在一天内的什么时间执行
        /// </summary>
        TimeOfDay Time { get; }
        #endregion
    }
}
