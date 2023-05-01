namespace System.TimeFrancis.Plan;

/// <summary>
/// 表示一个按星期重复的计划任务触发器
/// </summary>
sealed class PlanTriggerWeek : PlanTriggerDate, IPlanTriggerWeek
{
    #region 获取计划任务在星期几执行
    public IEnumerable<DayOfWeek> Weeks { get; }
    #endregion
    #region 每几周重复一次
    public int IntervalWeek { get; }
    #endregion
    #region 重写的ToString方法
    public override string ToString()
    {
        #region 用于格式化星期的本地函数
        static string Get(DayOfWeek week)
            => "星期" + week switch
            {
                DayOfWeek.Monday => "一",
                DayOfWeek.Tuesday => "二",
                DayOfWeek.Wednesday => "三",
                DayOfWeek.Thursday => "四",
                DayOfWeek.Friday => "五",
                DayOfWeek.Saturday => "六",
                _ => "日"
            };
        #endregion
        return ToStringAided($"每{IntervalWeek}周的{Weeks.Join(Get, "、")}{Time}执行一次，");
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="weeks">枚举计划任务在星期几执行</param>
    /// <param name="time">计划任务在一天内的什么时间执行</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    /// <param name="createDate">计划任务的创建日期，
    /// 如果为<see langword="null"/>，则使用当前日期</param>
    /// <param name="intervalWeek">指定每隔几周重复一次</param>
    public PlanTriggerWeek(IEnumerable<DayOfWeek> weeks, TimeOnly time, int? count = null, DateTimeOffset? createDate = null, int intervalWeek = 1)
        : base(time, count, createDate)
    {
        ExceptionIntervalOut.Check(1, null, intervalWeek);
        Weeks = weeks.ToHashSet();
        weeks.AnyCheck(nameof(weeks));
        IntervalWeek = intervalWeek;
    }
    #endregion
}
