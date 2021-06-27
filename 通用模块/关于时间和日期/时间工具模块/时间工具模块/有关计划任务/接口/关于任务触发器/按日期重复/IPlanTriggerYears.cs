using System.Collections.Generic;

namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个按年度重复的计划任务触发器
    /// </summary>
    public interface IPlanTriggerYears : IPlanTriggerDate
    {
        #region 枚举月份的第几天重复
        /// <summary>
        /// 枚举在月份的第几天重复计划任务
        /// </summary>
        IEnumerable<int> Days { get; }
        #endregion
        #region 枚举在几月重复
        /// <summary>
        /// 枚举在几月份重复计划任务
        /// </summary>
        IEnumerable<Month> Months { get; }
        #endregion
    }
}
