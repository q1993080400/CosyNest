using System;
using System.Collections.Generic;
using System.Design;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 这个类型的公开方法可以包装为<see cref="NamingService{Service}"/>委托
    /// </summary>
    /// <typeparam name="Service">服务的类型</typeparam>
    class NamingServiceCollection<Service> : WithoutRelease, INamingService<Service>
    {
        #region 储存服务的字典
        /// <summary>
        /// 这个字典可以通过名称来检索服务
        /// </summary>
        private IDictionary<string, Func<IServiceProvider, Service>> Services { get; }
        #endregion
        #region 服务请求对象
        /// <summary>
        /// 获取一个对象，
        /// 它可以用来请求服务所依赖的服务
        /// </summary>
        private IServiceProvider ServiceProvider { get; }
        #endregion
        #region 同步获取服务
        public Service GetService(string serviceName)
            => Services[serviceName](ServiceProvider);
        #endregion
        #region 构造函数
        /// <summary>
        /// 通过指定的参数创建对象
        /// </summary>
        /// <param name="serviceProvider">这个字典可以通过名称来检索服务</param>
        /// <param name="services">该对象可以用来请求服务所依赖的服务</param>
        public NamingServiceCollection(IServiceProvider serviceProvider, params (string Name, Func<IServiceProvider, Service> Service)[] services)
        {
            this.Services = services.ToDictionary(true);
            this.ServiceProvider = serviceProvider;
        }
        #endregion
    }
}
