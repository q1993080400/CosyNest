namespace System.TimeFrancis.Plan;

/// <summary>
/// 表示一个按照年度重复的计划任务触发器
/// </summary>
sealed class PlanTriggerYears : PlanTriggerDate, IPlanTriggerYears
{
    #region 枚举月份的第几天重复
    public IEnumerable<int> Days { get; }
    #endregion
    #region 枚举在几月重复
    public IEnumerable<Month> Months { get; }
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => ToStringAided($"在{Months.Select(x => (int)x).Join("、")}月的{Days.Join("、")}日{Time}执行一次，");
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="time">计划任务在一天内的什么时间执行</param>
    /// <param name="months">枚举在几月份重复计划任务</param>
    /// <param name="days">枚举在月份的第几天重复计划任务</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    /// <param name="createDate">计划任务的创建日期，
    /// 如果为<see langword="null"/>，则使用当前日期</param>
    public PlanTriggerYears(TimeOnly time, IEnumerable<Month> months, IEnumerable<int> days, int? count = null, DateTimeOffset? createDate = null)
        : base(time, count, createDate)
    {
        Months = months.ToHashSet();
        Days = days.ToHashSet();
        days.AnyCheck(nameof(days));
        months.AnyCheck(nameof(months));
        days.ForEach(x => ExceptionIntervalOut.Check(1, 31, x));
    }
    #endregion
}
