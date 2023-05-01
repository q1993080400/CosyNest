namespace System.TimeFrancis.Plan;

/// <summary>
/// 表示一个按照指定<see cref="TimeSpan"/>重复的计划任务触发器
/// </summary>
sealed class PlanTriggerTimeSpan : PlanTriggerTiming, IPlanTriggerTimeSpan
{
    #region 第一次执行的时间
    public DateTimeOffset Begin { get; }
    #endregion
    #region 重复执行的间隔
    public TimeSpan? Interval { get; }
    #endregion
    #region 获取下一次执行计划任务的日期
    public override DateTimeOffset? NextDate(DateTimeOffset? date = null)
    {
        var date2 = date ?? DateTimeOffset.MinValue;
        return date2 <= Begin ? Begin : throw new NotSupportedException("此API尚未实现");
    }
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => Interval == null ?
        $"仅在{Begin}执行一次" :
        $"从{Begin}开始，每{Interval.Value}执行一次，共执行{Count?.ToString() ?? "无限"}次";
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的开始时间，周期间隔和重复次数初始化对象
    /// </summary>
    /// <param name="begin">第一次执行计划任务的时间</param>
    /// <param name="interval">重复执行计划任务的间隔，
    /// 如果为<see langword="null"/>，代表不会重复</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    public PlanTriggerTimeSpan(DateTimeOffset begin, TimeSpan? interval, int? count = null)
        : base(count)
    {
        if (count is { } && count.Value is 1)
        {
            Interval = null;
        }
        else
        {
            ArgumentNullException.ThrowIfNull(interval);
            if (interval.Value <= TimeSpan.Zero)
                throw new ArgumentException("时间间隔不能为负或者零");
            Interval = interval;
        }
        Begin = begin;
    }
    #endregion
}
