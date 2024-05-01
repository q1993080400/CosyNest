namespace System.TimeFrancis;

/// <summary>
/// 这个记录是用来创建后台任务的参数
/// </summary>
public sealed record StartHostedInfo
{
    #region 定时器
    /// <summary>
    /// 控制在何时触发后台任务的定时器
    /// </summary>
    public required Timer Timer { get; init; }
    #endregion
    #region 定时器到期后触发的委托
    /// <summary>
    /// 定时器到期后触发的委托，
    /// 它的一个参数是服务提供对象，
    /// 第二个参数是用于取消这个后台任务的令牌
    /// </summary>
    public required Func<IServiceProvider, CancellationToken, Task> Expire { get; init; }
    #endregion
    #region 用来配置主机的委托
    /// <summary>
    /// 用于配置后台服务主机的委托，
    /// 返回值是这个主机是否应该启动，返回<see langword="false"/>可以在开发阶段跳过主机启动
    /// </summary>
    public required Func<IHostApplicationBuilder, bool> Configuration { get; init; }
    #endregion
}
