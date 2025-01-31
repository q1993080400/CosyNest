using System.Design;

using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关设计模式的扩展方法全部放在这里
/// </summary>
public static class ExtendDesign
{
    #region 注入事件总线工厂
    /// <summary>
    /// 以单例模式注入一个<see cref="IEventBusFactory"/>，
    /// 它可以用来创建事件总线上下文
    /// </summary>
    /// <param name="services">要注入服务的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusFactory(this IServiceCollection services)
        => services.AddSingleton<IEventBusFactory, EventBusFactory>();
    #endregion
    #region 投影对象
    /// <summary>
    /// 将一个对象集合投影为另一种对象
    /// </summary>
    /// <param name="projections">要投影的对象集合</param>
    /// <returns></returns>
    /// <inheritdoc cref="IProjection{Obj}"/>
    public static IEnumerable<Obj> Projection<Obj>(this IEnumerable<IProjection<Obj>> projections)
        => projections.Select(x => x.Projection());
    #endregion
    #region 以指定的生存期注入服务
    /// <summary>
    /// 以指定的生存期注入服务
    /// </summary>
    /// <typeparam name="TService">服务的类型</typeparam>
    /// <param name="services">要注入的服务容器</param>
    /// <param name="lifetime">服务的生存期</param>
    /// <param name="factory">用来创建服务的工厂</param>
    /// <returns></returns>
    public static IServiceCollection AddService<TService>(this IServiceCollection services, ServiceLifetime lifetime, Func<IServiceProvider, TService> factory)
        where TService : class
    {
        var serviceDescriptor = new ServiceDescriptor(typeof(TService), factory, lifetime);
        services.Add(serviceDescriptor);
        return services;
    }
    #endregion
}
