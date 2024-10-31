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
    #region 服务提供者对象
    /// <summary>
    /// 获取服务提供者对象
    /// </summary>
    public required IServiceProvider ServiceProvider { get; init; }
    #endregion
    #region 获取是否在出现异常的情况下立即退出
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 在发生异常的时候，会立即退出整个主机，
    /// 否则只会忽略异常，等待下一个循环
    /// </summary>
    public bool ExitImmediately { get; init; }
    #endregion
}
