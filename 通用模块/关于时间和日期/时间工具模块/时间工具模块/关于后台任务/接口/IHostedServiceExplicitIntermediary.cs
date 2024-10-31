namespace System.TimeFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为中介，
/// 使外界能够显式调用<see cref="IHostedServiceExplicit.ExplicitTriggering(CancellationToken)"/>
/// </summary>
public interface IHostedServiceExplicitIntermediary
{
    #region 注册主机
    /// <summary>
    /// 将主机注册到中介中
    /// </summary>
    /// <param name="host">待注册的主机的实例</param>
    void Register(IHostedServiceExplicit host);
    #endregion
    #region 显式触发主机的后台任务
    /// <summary>
    /// 显式触发主机的后台任务，
    /// 无论该任务是否到期
    /// </summary>
    /// <typeparam name="Host">待注册的主机类型</typeparam>
    /// <param name="stoppingToken">一个用于取消异步操作的令牌</param>
    /// <returns>这个元组的第一个项是是否成功找到指定类型的主机，
    /// 第二个项是用来等待后台任务执行完毕的<see cref="Task"/></returns>
    (bool SuccessfulCall, Task Wait) ExplicitTriggering<Host>(CancellationToken stoppingToken = default)
        where Host : class, IHostedServiceExplicit;
    #endregion
}
