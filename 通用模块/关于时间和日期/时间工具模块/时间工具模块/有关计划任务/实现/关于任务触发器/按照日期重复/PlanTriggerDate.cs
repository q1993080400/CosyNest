namespace System.TimeFrancis.Plan;

/// <summary>
/// 表示一个按照日期重复的触发器
/// </summary>
abstract class PlanTriggerDate : PlanTriggerTiming, IPlanTriggerDate
{
    #region 计划任务的创建日期
    public DateTimeOffset CreateDate { get; }
    #endregion
    #region 计划任务的执行时间
    public TimeOnly Time { get; }
    #endregion
    #region 获取下一次执行计划任务的日期
    public override DateTimeOffset? NextDate(DateTimeOffset? date = null)
        => throw new NotImplementedException("此API尚未实现");
    #endregion
    #region 重写ToString的辅助方法
    /// <summary>
    /// 重写<see cref="object.ToString(string)"/>的辅助方法，
    /// 封装了派生类文本化中不变的部分
    /// </summary>
    /// <param name="middle">字符串的中间部分</param>
    /// <returns></returns>
    protected string ToStringAided(string middle)
        => $"从{CreateDate}开始，" + middle +
        $"共执行{Count?.ToString() ?? "无限"}次";
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="time">计划任务在一天内的什么时间执行</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    /// <param name="createDate">计划任务的创建日期，
    /// 如果为<see langword="null"/>，则使用当前日期</param>
    public PlanTriggerDate(TimeOnly time, int? count = null, DateTimeOffset? createDate = null)
        : base(count)
    {
        this.Time = time;
        this.CreateDate = createDate ?? DateTime.Now;
    }
    #endregion
}
