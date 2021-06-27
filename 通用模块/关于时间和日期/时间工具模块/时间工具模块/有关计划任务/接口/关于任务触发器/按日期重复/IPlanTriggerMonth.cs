using System.Collections.Generic;

namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个按照月度重复的计划任务触发器
    /// </summary>
    public interface IPlanTriggerMonth : IPlanTriggerDate
    {
        #region 每个月的第几天重复
        /// <summary>
        /// 枚举在每个月的第几天重复计划任务
        /// </summary>
        IEnumerable<int> Days { get; }
        #endregion
        #region 每几月重复一次
        /// <summary>
        /// 获取每隔几月重复一次
        /// </summary>
        int IntervalMonth { get; }
        #endregion
    }
}
