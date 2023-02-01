using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace System;

/// <summary>
/// 有关ASP.Net前后端通用的扩展方法全部放在这个类型中
/// </summary>
public static class ExtenASP
{
    #region 有关依赖注入
    #region 注入NamingService
    #region 说明文档
    /*问：“新服务可以请求新旧服务提供的所有服务”是什么意思？
      答：举例说明，假设第一次调用AddNamingService，
      注入了一个INamingService，它可以用来请求服务a，
      那么第二次调用该方法后，又注入了一个可以请求服务b的INamingService，
      则实际注入的INamingService可以同时请求服务a和b

      问：为什么需要这样设计？它有什么好处？
      答：这是因为作者希望把前端和后端的依赖注入分开，
      换句话来说，经过这样的设计，前端和后端可以注入两次服务，
      然后结果是它们的并集，这样可以减少不必要的耦合*/
    #endregion
    #region 复杂方法
    /// <summary>
    /// 以指定的服务生存期注册一个命名服务容器，
    /// 如果服务已被注册，则替换服务，且新服务可以请求新旧服务提供的所有服务，
    /// 根据服务生存期的不同，实际注册的服务类型分别是：
    /// <see cref="INamingServiceSingleton{Service}"/>，
    /// <see cref="INamingServiceScoped{Service}"/>，
    /// <see cref="INamingServiceTransient{Service}"/>
    /// </summary>
    /// <param name="services">服务容器</param>
    /// <param name="lifetime">指定服务的生存期</param>
    /// <returns></returns>
    /// <inheritdoc cref="NamingServiceDefault{Service}.NamingServiceDefault(IServiceProvider, Func{string, IServiceProvider, Service}, INamingService{Service}?)"/>
    /// <inheritdoc cref="INamingService{Service}"/>
    public static IServiceCollection AddNamingService<Service>(this IServiceCollection services,
        ServiceLifetime lifetime,
        Func<string, IServiceProvider, Service?> serviceProvider)
        where Service : class
    {
        var serviceType = lifetime switch
        {
            ServiceLifetime.Singleton => typeof(INamingServiceSingleton<Service>),
            ServiceLifetime.Scoped => typeof(INamingServiceScoped<Service>),
            ServiceLifetime.Transient => typeof(INamingServiceTransient<Service>),
            var l => throw new Exception($"无法识别{l}类型的服务生存期")
        };
        var factory = services.FirstOrDefault(x => x.ServiceType == serviceType)?.ImplementationFactory;
        #region 本地函数
        INamingServiceUnite<Service> Fun(IServiceProvider provider)
        {
            var old = factory?.Invoke(provider) as INamingService<Service>;
            return new NamingServiceDefault<Service>(provider, serviceProvider, old);
        }
        #endregion
        return services.Replace(new(serviceType, Fun, lifetime));
    }
    #endregion
    #region 简单方法
    /// <summary>
    /// 以单例模式注册一个<see cref="INamingServiceSingleton{Service}"/>，
    /// 它直接通过键值对提取对应名称的服务实例，
    /// 如果服务已被注册，则替换服务，且新服务可以请求新旧服务提供的所有服务
    /// </summary>
    /// <param name="serviceDictionary">这个委托的参数是服务请求对象，
    /// 返回值是一个字典，它的键值对分别是服务的名称和实例</param>
    /// <returns></returns>
    /// <inheritdoc cref="AddNamingService{Service}(IServiceCollection, ServiceLifetime, Func{string, IServiceProvider, Service})"/>
    public static IServiceCollection AddNamingServiceSimple<Service>(this IServiceCollection services,
        Func<IServiceProvider, Dictionary<string, Service>> serviceDictionary)
        where Service : class
    {
        Dictionary<string, Service>? dictionary = null;
        return services.AddNamingService(ServiceLifetime.Singleton,
                (name, provider) =>
                {
                    if (dictionary is null)
                    {
                        dictionary = serviceDictionary(provider);
                        serviceDictionary = null!;
                    }
                    return dictionary.TryGetValue(name).Value;
                });
    }
    #endregion
    #endregion
    #endregion
}
