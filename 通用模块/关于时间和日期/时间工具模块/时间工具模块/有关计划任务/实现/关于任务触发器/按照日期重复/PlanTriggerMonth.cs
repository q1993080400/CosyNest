
using static System.ExceptionIntervalOut;

namespace System.TimeFrancis.Plan;

/// <summary>
/// 表示一个按月份重复的计划任务触发器
/// </summary>
sealed class PlanTriggerMonth : PlanTriggerDate, IPlanTriggerMonth
{
    #region 每几月重复一次
    public int IntervalMonth { get; }
    #endregion
    #region 每个月的第几天重复
    public IEnumerable<int> Days { get; }
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => ToStringAided($"每{IntervalMonth}月的{Days.Join("、")}日{Time}执行一次，");
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="days">枚举在每个月的第几天重复计划任务</param>
    /// <param name="time">计划任务在一天内的什么时间执行</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    /// <param name="createDate">计划任务的创建日期，
    /// 如果为<see langword="null"/>，则使用当前日期</param>
    /// <param name="intervalMonth">指定每隔几月重复一次</param>
    public PlanTriggerMonth(IEnumerable<int> days, TimeOnly time, int? count = null, DateTimeOffset? createDate = null, int intervalMonth = 1)
        : base(time, count, createDate)
    {
        Check(1, null, intervalMonth);
        this.IntervalMonth = intervalMonth;
        this.Days = days.ToHashSet();
        days.AnyCheck(nameof(days));
        this.Days.ForEach(x => Check(1, 31, x));
    }
    #endregion
}
