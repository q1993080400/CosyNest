﻿namespace System.TimeFrancis.Plan;

/// <summary>
/// 这个静态类可以用来创建和计划任务有关的对象
/// </summary>
public static class CreatePlan
{
    #region 创建触发器
    #region 在硬件启动时执行
    /// <summary>
    /// 创建一个触发器，它在硬件启动时执行计划任务
    /// </summary>
    /// <returns></returns>
    public static IPlanTriggerStart TriggerStart { get; } = new PlanTriggerStart();
    #endregion
    #region 仅执行一次
    /// <summary>
    /// 创建一个触发器，它仅在指定时间执行一次
    /// </summary>
    /// <param name="begin">触发器的执行时间</param>
    /// <returns></returns>
    public static IPlanTriggerTimeSpan TriggerDisposable(DateTimeOffset begin)
        => new PlanTriggerTimeSpan(begin, null, 1);
    #endregion
    #region 按照指定时间间隔执行
    /// <summary>
    /// 创建一个触发器，它按照指定的<see cref="TimeSpan"/>间隔重复执行任务
    /// </summary>
    /// <param name="begin">第一次执行计划任务的时间</param>
    /// <param name="interval">重复执行计划任务的间隔，
    /// 如果为<see langword="null"/>，代表不会重复</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    public static IPlanTriggerTimeSpan TriggerTimeSpan(DateTimeOffset begin, TimeSpan? interval, int? count = null)
        => new PlanTriggerTimeSpan(begin, interval, count);
    #endregion
    #region 按照星期执行
    /// <summary>
    /// 创建一个触发器，它按照星期重复执行计划任务
    /// </summary>
    /// <param name="weeks">枚举计划任务在星期几执行</param>
    /// <param name="time">计划任务在一天内的什么时间执行</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    /// <param name="createDate">计划任务的创建日期，
    /// 如果为<see langword="null"/>，则使用当前日期</param>
    /// <param name="intervalWeek">指定每隔几周重复一次</param>
    public static IPlanTriggerWeek TriggerWeek(IEnumerable<DayOfWeek> weeks, TimeOnly time, int? count = null, DateTimeOffset? createDate = null, int intervalWeek = 1)
        => new PlanTriggerWeek(weeks, time, count, createDate, intervalWeek);
    #endregion
    #region 按照月份重复执行
    /// <summary>
    /// 创建一个触发器，它按照月份重复执行计划任务
    /// </summary>
    /// <param name="days">枚举在每个月的第几天重复计划任务</param>
    /// <param name="time">计划任务在一天内的什么时间执行</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    /// <param name="createDate">计划任务的创建日期，
    /// 如果为<see langword="null"/>，则使用当前日期</param>
    /// <param name="intervalMonth">指定每隔几月重复一次</param>
    public static IPlanTriggerMonth TriggerMonth(IEnumerable<int> days, TimeOnly time, int? count = null, DateTimeOffset? createDate = null, int intervalMonth = 1)
        => new PlanTriggerMonth(days, time, count, createDate, intervalMonth);
    #endregion
    #region 按照年度执行
    /// <summary>
    /// 创建一个触发器，它按照年度重复执行计划任务
    /// </summary>
    /// <param name="time">计划任务在一天内的什么时间执行</param>
    /// <param name="months">枚举在几月份重复计划任务</param>
    /// <param name="days">枚举在月份的第几天重复计划任务</param>
    /// <param name="count">重复执行计划任务的次数，
    /// 如果为<see langword="null"/>，代表执行无数次</param>
    /// <param name="createDate">计划任务的创建日期，
    /// 如果为<see langword="null"/>，则使用当前日期</param>
    /// <returns></returns>
    public static IPlanTriggerYears TriggerYears(TimeOnly time, IEnumerable<Month> months, IEnumerable<int> days, int? count = null, DateTimeOffset? createDate = null)
        => new PlanTriggerYears(time, months, days, count, createDate);
    #endregion
    #endregion
}
