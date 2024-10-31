using System.Collections.Immutable;

namespace System.TimeFrancis;

/// <summary>
/// 这个类型是<see cref="IHostedServiceExplicitIntermediary"/>的实现，
/// 可以视为一个后台任务主机中介
/// </summary>
sealed class HostedServiceExplicitIntermediary : IHostedServiceExplicitIntermediary
{
    #region 接口实现
    #region 注册主机
    public void Register(IHostedServiceExplicit host)
    {
        CacheHost = CacheHost.SetItem(host.GetType(), host);
    }
    #endregion
    #region 显式触发主机的后台任务
    public (bool SuccessfulCall, Task Wait) ExplicitTriggering<Host>(CancellationToken stoppingToken)
        where Host : class, IHostedServiceExplicit
    {
        var hasHost = CacheHost.TryGetValue(typeof(Host), out var host);
        return (hasHost, host?.ExplicitTriggering(stoppingToken) ?? Task.CompletedTask);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 缓存主机实例的字典
    /// <summary>
    /// 用来缓存主机实例的字典
    /// </summary>
    private ImmutableDictionary<Type, IHostedServiceExplicit> CacheHost { get; set; }
        = ImmutableDictionary<Type, IHostedServiceExplicit>.Empty;
    #endregion
    #endregion
}
