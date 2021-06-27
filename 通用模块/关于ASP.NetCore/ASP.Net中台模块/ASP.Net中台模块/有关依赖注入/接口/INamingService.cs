using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 凡是实现这个接口的类型，都可以根据名称请求服务
    /// </summary>
    /// <typeparam name="Service">服务的类型</typeparam>
    public interface INamingService<Service> : IDisposable
    {
        #region 说明文档
        /*问：该接口具有什么意义？
          答：在依赖注入中，有些服务具有相同的类型和不同的应用场合，
          举例说明：可能有两个服务，它们都是ICryptology，但是包含的密钥不同，
          而ASPNet的依赖注入在请求服务时，只根据类型和服务生存期进行区分，
          这不足以满足需求，因此作者声明了本接口，
          它可以根据名称请求类型相同，但是适用场景不同的服务*/
        #endregion
        #region 同步请求服务
        /// <summary>
        /// 通过服务的名称获取服务
        /// </summary>
        /// <param name="serviceName">服务的名称</param>
        /// <returns></returns>
        Service GetService(string serviceName);
        #endregion
        #region 异步请求服务
        /// <inheritdoc cref="GetService(string)"/>
        Task<Service> GetServiceAsync(string serviceName)
            => Task.Run(() => GetService(serviceName));
        #endregion
    }
}
