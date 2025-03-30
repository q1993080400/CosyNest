using System.NetFrancis;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendNet
{
    //这个部分类专门声明有关SignalR的扩展方法

    #region 注入SignalRFactory对象
    /// <summary>
    /// 以瞬间模式注入一个<see cref="ISignalRFactory"/>对象，
    /// 该依赖注入能够自动处理绝对路径和相对路径的转换，
    /// 它依赖于<see cref="IHostProvide"/>服务
    /// </summary>
    /// <param name="services">待注入的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddSignalRFactory(this IServiceCollection services)
        => services.AddTransient<ISignalRFactory, SignalRFactory>();
    #endregion
    #region 注入IConfigurationSignalRFactory对象
    /// <summary>
    /// 以范围模式注入一个<see cref="IConfigurationSignalRFactory"/>对象，
    /// 它可以用来配置SignalR工厂
    /// </summary>
    /// <param name="services">待注入的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddConfigurationSignalRFactoryDefault(this IServiceCollection services)
        => services.AddScoped<IConfigurationSignalRFactory, ConfigurationSignalRFactory>();
    #endregion
    #region 如果HubConnection尚未连接，则发起连接
    /// <summary>
    /// 如果一个<see cref="HubConnection"/>尚未连接，
    /// 则发起连接，并返回这个连接
    /// </summary>
    /// <param name="connection">要连接的<see cref="HubConnection"/></param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    public static async Task<HubConnection> StartSecureAsync(this HubConnection connection, CancellationToken cancellationToken = default)
    {
        if (connection.State is HubConnectionState.Disconnected)
            await connection.StartAsync(cancellationToken);
        return connection;
    }
    #endregion
}
