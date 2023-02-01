using System.TimeFrancis;

using Timer = System.TimeFrancis.Timer;

namespace System;

/// <summary>
/// 有关时间和计划任务的扩展方法全部放在这个类型中
/// </summary>
public static class ExtenTimer
{
    #region 为服务容器添加后台任务
    /// <summary>
    /// 为服务容器添加一个后台任务，
    /// 它按照指定的时间间隔触发，并执行委托
    /// </summary>
    /// <param name="services">待注册的服务容器</param>
    /// <param name="expire">定时器到期后触发的委托</param>
    /// <returns></returns>
    /// <inheritdoc cref="TimedHostedService(Timer, Func{CancellationToken, Task})"/>
    public static IServiceCollection AddBackgroundService(this IServiceCollection services, Timer timer, Func<IServiceProvider, CancellationToken, Task> expire)
        => services.AddHostedService(x => new TimedHostedService(timer, y => expire(x, y)));
    #endregion
}
