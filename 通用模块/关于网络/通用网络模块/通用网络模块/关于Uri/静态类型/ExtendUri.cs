using System.NetFrancis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendNet
{
    //这个部分类专门声明有关Uri的扩展方法

    #region 注入IHostProvide
    #region 直接指定Host
    /// <summary>
    /// 以单例模式注入一个<see cref="IHostProvide"/>，
    /// 它可以用于提供本机Host地址
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <param name="baseUri">本地主机的Host地址</param>
    /// <returns></returns>
    public static IServiceCollection AddHostProvide(this IServiceCollection services, string baseUri)
        => services.AddSingleton(_ => CreateNet.HostProvide(baseUri));
    #endregion
    #region 从配置中读取
    /// <summary>
    /// 以单例模式注入一个<see cref="IHostProvide"/>，
    /// 它可以用于提供本机Host地址，
    /// 它通过配置文件中一个叫Host的键提取地址
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddHostProvideFromConfiguration(this IServiceCollection services)
        => services.AddSingleton(services =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var baseUri = configuration.GetValue<string>("Host") ??
            throw new NotSupportedException("没有在配置中设置主机的Host");
            return CreateNet.HostProvide(baseUri);
        });
    #endregion
    #endregion
}
