using System.Collections.Generic;

namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个按照星期重复的计划任务触发器
    /// </summary>
    public interface IPlanTriggerWeek : IPlanTriggerDate
    {
        #region 获取计划任务在星期几执行
        /// <summary>
        /// 枚举计划任务在星期几执行
        /// </summary>
        IEnumerable<DayOfWeek> Weeks { get; }
        #endregion
        #region 每几周重复一次
        /// <summary>
        /// 获取每隔几周重复一次
        /// </summary>
        int IntervalWeek { get; }
        #endregion
    }
}
