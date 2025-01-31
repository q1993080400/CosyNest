using System.NetFrancis;

namespace System;

public static partial class ExtendBlazorServer
{
    //这个部分类专门声明有关SignalR的扩展方法

    #region 注入IConfigurationSignalRFactory对象
    /// <summary>
    /// 以范围模式注入一个<see cref="IConfigurationSignalRFactory"/>对象，
    /// 它专门用于Blazor服务器模式，可以用来配置SignalR工厂
    /// </summary>
    /// <param name="services">待注入的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddConfigurationSignalRFactoryServer(this IServiceCollection services)
        => services.AddScoped<IConfigurationSignalRFactory, ConfigurationSignalRFactoryServer>();
    #endregion
}
