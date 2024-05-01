namespace System.TimeFrancis;

/// <summary>
/// 这个类型按照定时器执行后台任务
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="timer">定时器对象，它控制后台任务的触发周期</param>
/// <param name="serviceProvider">服务提供者对象</param>
/// <param name="expire">定时器到期后触发的委托</param>
/// <param name="stop">用来正常停止主机的委托</param>
sealed class TimedHostedService
    (Timer timer,
    IServiceProvider serviceProvider,
    Func<IServiceProvider, CancellationToken, Task> expire,
    Func<Task> stop) : BackgroundService
{
    #region 抽象类实现
    #region 启动服务
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var info = timer(stoppingToken);
            if (info is null)
                break;
            await info.Wait;
            if (stoppingToken.IsCancellationRequested)
                break;
            using var scope = serviceProvider.CreateScope();
            await expire(scope.ServiceProvider, stoppingToken);
        }
        await stop();
    }
    #endregion
    #endregion
}
