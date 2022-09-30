namespace System.TimeFrancis.Plan;

/// <summary>
/// 凡是实现这个接口的类型，都可以视为一个定时计划任务触发器，
/// 它的执行时间确定，且可被计算
/// </summary>
public interface IPlanTriggerTiming : IPlanTrigger
{
    #region 获取执行的次数
    /// <summary>
    /// 获取该计划任务执行的次数，
    /// 如果为<see langword="null"/>，
    /// 代表执行无数次
    /// </summary>
    int? Count { get; }
    #endregion
    #region 获取下一次执行计划任务的日期
    /// <summary>
    /// 获取从某个时间点开始，下一次执行计划任务的时间
    /// </summary>
    /// <param name="date">当前时间，如果为<see langword="null"/>，
    /// 默认为<see cref="DateTimeOffset.MinValue"/>，等同于计算第一次执行计划任务的时间</param>
    /// <returns>下一次执行计划任务的时间，如果为<see langword="null"/>，表示计划任务已终止</returns>
    DateTimeOffset? NextDate(DateTimeOffset? date = null);
    #endregion
}
