namespace System.TimeFrancis;

/// <summary>
/// 这个类型按照定时器执行后台任务
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="startHostedInfo">用来创建主机的参数</param>
/// <param name="serviceProvider">服务提供者对象</param>
/// <param name="stop">用来正常停止主机的委托</param>
sealed class TimedHostedService
    (StartHostedInfo startHostedInfo, IServiceProvider serviceProvider, Func<Task> stop) : BackgroundService
{
    #region 抽象类实现
    #region 启动服务
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = startHostedInfo.Timer;
        var expire = startHostedInfo.Expire;
        while (!stoppingToken.IsCancellationRequested)
        {
            var info = timer(stoppingToken);
            if (info is null)
                break;
            await info.Wait;
            if (stoppingToken.IsCancellationRequested)
                break;
            try
            {
                using var scope = serviceProvider.CreateScope();
                await expire(scope.ServiceProvider, stoppingToken);
            }
            catch (Exception ex)
            {
                ex.Log(serviceProvider);
                if (startHostedInfo.ExitImmediately)
                    break;
            }
            if (info.Next is null)
                break;
        }
        await stop();
    }
    #endregion
    #endregion
}
