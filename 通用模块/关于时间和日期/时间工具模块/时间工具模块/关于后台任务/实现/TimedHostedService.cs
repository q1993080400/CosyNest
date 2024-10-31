namespace System.TimeFrancis;

/// <summary>
/// 这个类型按照定时器执行后台任务
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
public abstract class TimedHostedService : BackgroundService, IHostedServiceExplicit
{
    #region 公开成员
    #region 用来创建主机的参数
    /// <summary>
    /// 用来创建主机的参数
    /// </summary>
    public required StartHostedInfo StartHostedInfo { get; init; }
    #endregion
    #endregion
    #region 接口实现
    #region 显式触发后台任务
    public Task ExplicitTriggering(CancellationToken stoppingToken = default)
        => CreateTask.LongTask(StartHostedInfo.ServiceProvider, async serviceProvider =>
        {
            if (IsRun)
                return;
            IsRun = true;
            try
            {
                await OnTrigger(serviceProvider, true, stoppingToken);
            }
            finally
            {
                IsRun = false;
            }
        });
    #endregion
    #endregion 
    #region 抽象成员
    #region 触发定时器时执行的方法
    /// <summary>
    /// 当触发定时器的时候，执行这个方法，
    /// 它是后台任务实际的逻辑
    /// </summary>
    /// <param name="serviceProvider">服务提供者对象</param>
    /// <param name="explicitTriggering">如果这个值为<see langword="true"/>，
    /// 表示这个方法为显式手动触发，否则表示为定时器到期自动触发</param>
    /// <param name="stoppingToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    protected abstract Task OnTrigger(IServiceProvider serviceProvider, bool explicitTriggering, CancellationToken stoppingToken = default);
    #endregion
    #endregion
    #region 抽象类实现
    #region 重写的StartAsync方法
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        StartHostedInfo.ServiceProvider.
            GetService<IHostedServiceExplicitIntermediary>()?.
            Register(this);
        return base.StartAsync(cancellationToken);
    }
    #endregion
    #region 启动服务
    protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = StartHostedInfo.Timer;
        while (!stoppingToken.IsCancellationRequested)
        {
            var info = timer(stoppingToken);
            if (info is null)
                break;
            if (!await info.Wait)
                break;
            if (IsRun)
                continue;
            IsRun = true;
            using var scope = StartHostedInfo.ServiceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            try
            {
                await OnTrigger(serviceProvider, false, stoppingToken);
            }
            catch (Exception ex)
            {
                ex.Log(serviceProvider);
                if (StartHostedInfo.ExitImmediately)
                    break;
            }
            finally
            {
                IsRun = false;
            }
            if (info.NextTimeState is (NextTimeState.NotNext, _))
                break;
        }
    }
    #endregion
    #endregion
    #region 内部成员
    #region 是否正在运行
    /// <summary>
    /// 获取这个后台任务是否正在运行，
    /// 通过判断和修改它，可以防止同时运行两个后台任务
    /// </summary>
    private bool IsRun { get; set; }
    #endregion
    #endregion
}
