using System.Linq;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace System
{
    /// <summary>
    /// 有关ASP.Net前后端通用的扩展方法全部放在这个类型中
    /// </summary>
    public static class ExtenASP
    {
        #region 有关依赖注入
        #region 注入NamingService
        #region 复杂模式
        /// <summary>
        /// 注入一个<see cref="NamingService{Service}"/>，
        /// 它是获取其他服务的跳板，能够进一步根据名称获取服务
        /// </summary>
        /// <typeparam name="Service">服务的类型</typeparam>
        /// <param name="serviceCollection">用来获取服务的容器</param>
        /// <param name="serviceLifetime">服务的生存期</param>
        /// <param name="services">这个元组指定服务的名称，以及用来获取服务的委托</param>
        /// <returns></returns>
        public static IServiceCollection AddNamingService<Service>(this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime,
            params (string Name, Func<IServiceProvider, Service> Service)[] services)
        {
            serviceCollection.Add(new(typeof(INamingService<Service>),
               x => new NamingServiceCollection<Service>(x, services), serviceLifetime));
            return serviceCollection;
        }
        #endregion
        #region 简洁模式
        /// <summary>
        /// 以单例模式注入一个<see cref="NamingService{Service}"/>，
        /// 它是获取其他服务的跳板，能够进一步根据名称获取服务
        /// </summary>
        /// <typeparam name="Service">服务的类型</typeparam>
        /// <param name="serviceCollection">用来获取服务的容器</param>
        /// <param name="services">这个元组指定服务的名称，以及服务的单一实例</param>
        /// <returns></returns>
        public static IServiceCollection AddNamingService<Service>(this IServiceCollection serviceCollection, params (string Name, Service Service)[] services)
            => serviceCollection.AddNamingService(ServiceLifetime.Singleton, services.Select(x =>
              {
                  Service Fun(IServiceProvider _) => x.Service;
                  return (x.Name, (Func<IServiceProvider, Service>)Fun);
              }).ToArray());
        #endregion
        #endregion
        #endregion
        #region 关于SignalR
        #region 添加常用Json支持
        /// <summary>
        /// 为SignalR中心或连接添加常用Json序列化支持
        /// </summary>
        /// <param name="options">一个用于配置SignalR中心或连接的对象</param>
        /// <returns></returns>
        public static JsonHubProtocolOptions AddFormatterJson(this JsonHubProtocolOptions options)
        {
            options.PayloadSerializerOptions.
                Converters.Add(CreateASP.SerializationCommon);
            return options;
        }
        #endregion
        #endregion
    }
}
