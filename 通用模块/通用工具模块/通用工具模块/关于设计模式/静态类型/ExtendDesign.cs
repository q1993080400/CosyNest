using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关设计模式的扩展方法全部放在这里
/// </summary>
public static partial class ExtendDesign
{
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
