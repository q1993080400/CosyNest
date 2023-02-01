namespace System.TimeFrancis;

/// <summary>
/// 这个类型按照定时器执行后台任务
/// </summary>
sealed class TimedHostedService : BackgroundService
{
    #region 内部成员
    #region 定时器到期触发的委托
    /// <summary>
    /// 获取定时器到期后触发的委托
    /// </summary>
    private Func<CancellationToken, Task> Expire { get; }
    #endregion
    #region 定时器
    /// <summary>
    /// 获取定时器对象，它控制后台任务的触发周期
    /// </summary>
    private Timer Timer { get; }
    #endregion
    #region 抽象类实现
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Timer();
            if (stoppingToken.IsCancellationRequested)
                return;
            await Expire(stoppingToken);
        }
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="timer">定时器对象，它控制后台任务的触发周期</param>
    /// <param name="expire">定时器到期后触发的委托</param>
    public TimedHostedService(Timer timer, Func<CancellationToken, Task> expire)
    {
        Expire = expire;
        Timer = timer;
    }
    #endregion
}
