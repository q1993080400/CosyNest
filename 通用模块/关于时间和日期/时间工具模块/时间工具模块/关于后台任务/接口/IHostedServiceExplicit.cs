namespace System.TimeFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个支持显式启动后台任务逻辑的后台任务主机
/// </summary>
public interface IHostedServiceExplicit : IHostedService
{
    #region 显式触发后台任务
    /// <summary>
    /// 显式触发后台任务，
    /// 无论该任务是否到期
    /// </summary>
    /// <param name="stoppingToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task ExplicitTriggering(CancellationToken stoppingToken = default);
    #endregion
}
