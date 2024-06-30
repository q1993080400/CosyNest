using System.TimeFrancis.Plan;

namespace System.TimeFrancis;

/// <summary>
/// 这个静态类可以用来创建与时间和计划任务有关的对象
/// </summary>
public static class CreateTimer
{
    #region 创建定时器
    #region 创建周期定时器
    #region 按照时间周期
    /// <summary>
    /// 创建一个<see cref="Timer"/>，
    /// 它从指定的时间开始，按照指定的周期触发
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="TimeFrancis.TimerCycle(TimeSpan, DateTimeOffset, bool)"/>
    public static Timer TimerCycle(TimeSpan interval, DateTimeOffset startTime, bool immediately = false)
        => new TimerCycle(interval, startTime, immediately).Timer;
    #endregion
    #region 按照小时分钟触发，且可指定天数间隔
    /// <summary>
    /// 创建一个<see cref="Timer"/>，
    /// 它在指定的时间触发，并按照指定的天数进行重复
    /// </summary>
    /// <param name="time">定时器触发的时间</param>
    /// <param name="intervalDays">定时器按照这个天数间隔进行重复</param>
    /// <returns></returns>
    /// <inheritdoc cref="TimeFrancis.TimerCycle(TimeSpan, DateTimeOffset, bool)"/>
    public static Timer TimerFromTime(TimeOnly time, int intervalDays = 1, bool immediately = false)
    {
        ExceptionIntervalOut.Check(1, null, intervalDays);
        var timeSpan = time.ToTimeSpan();
        var startTime = DateTimeOffset.Now.ToDay() + timeSpan;
        var finalStartTime =
            startTime < DateTimeOffset.Now ? startTime.AddDays(1) : startTime;
        return TimerCycle(TimeSpan.FromDays(intervalDays), finalStartTime, immediately);
    }
    #endregion
    #region 按照小时分钟触发，且可通过委托跳过天数
    /// <summary>
    /// 创建一个定时器的实现，
    /// 它每天在指定的时间触发定时器
    /// </summary>
    /// <returns></returns>
    /// <param name="canTrigger">这个委托的第一个参数是当前日期，
    /// 第二个参数是已经有多少天没有触发定时器，返回值是那一天是否应该触发定时器，
    /// 通过指定它，可以跳过某一天，如果为<see langword="null"/>，表示每天触发</param>
    /// <inheritdoc cref="TimerFromTime.TimerFromTime(TimeOnly[], Func{DateOnly, int, bool})"/>
    public static Timer TimerFromTime(IEnumerable<TimeOnly> times, Func<DateOnly, int, bool>? canTrigger = null)
        => new TimerFromTime(times.ToArray(), canTrigger ??= (_, _) => true).Timer;
    #endregion
    #endregion
    #region 创建一个立即执行，且仅执行一次的定时器
    /// <summary>
    /// 创建一个立即执行，且仅执行一次的定时器
    /// </summary>
    /// <returns></returns>
    public static Timer TimerDisposable()
    {
        var execute = false;
        return cancellationToken =>
        {
            if (execute || cancellationToken.IsCancellationRequested)
                return null;
            execute = true;
            return new()
            {
                Wait = Task.FromResult(true),
                Next = null
            };
        };
    }
    #endregion
    #endregion
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
    #region 启动后台服务
    /// <summary>
    /// 启动一个后台服务
    /// </summary>
    /// <param name="info">创建后台任务的参数</param>
    public static async void StartHostedService(StartHostedInfo info)
    {
        #region 用来创建主机的本地函数
        async Task StartHost()
        {
            var builder = Host.CreateApplicationBuilder();
            var start = info.Configuration(builder);
            if (!start)
                return;
            using var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            #region 停止主机的本地函数
            async Task Stop()
            {
                await cancellationTokenSource.CancelAsync();
            }
            #endregion
            builder.Services.AddHostedService(x => new TimedHostedService(info, x, Stop));
            var host = builder.Build();
            await host.RunAsync(token);
        }
        #endregion
        await Task.Factory.StartNew(StartHost, TaskCreationOptions.LongRunning);
    }
    #endregion
}
