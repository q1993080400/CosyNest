using System.TimeFrancis;

namespace System;

public static partial class ExtendTimer
{
    //这个部分类专门声明和后台任务有关的扩展方法

    #region 添加后台任务主机中介
    /// <summary>
    /// 以单例模式添加一个<see cref="IHostedServiceExplicitIntermediary"/>，
    /// 它可以用来显式调用后台任务主机
    /// </summary>
    /// <param name="services">待添加服务的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddHostedServiceExplicitIntermediary(this IServiceCollection services)
        => services.AddSingleton<IHostedServiceExplicitIntermediary, HostedServiceExplicitIntermediary>();
    #endregion
    #region 显式触发后台任务
    /// <summary>
    /// 通过依赖注入请求后台任务，
    /// 并显式触发它
    /// </summary>
    /// <typeparam name="BackgroundService">后台任务的类型</typeparam>
    /// <param name="serviceProvider">服务提供者对象</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    public static Task BackgroundServiceTriggering<BackgroundService>
        (this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        where BackgroundService : class, IHostedServiceExplicit
    {
        var intermediary = serviceProvider.GetRequiredService<IHostedServiceExplicitIntermediary>();
        var (successfulCall, wait) = intermediary.ExplicitTriggering<BackgroundService>(cancellationToken);
        return successfulCall ?
            wait :
            throw new NotSupportedException($"没有找到{typeof(BackgroundService)}类型的后台任务主机");
    }
    #endregion
}
