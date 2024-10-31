using System.NetFrancis;
using System.NetFrancis.Http;
using System.Reflection;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendNet
{
    //这个部分类专门声明有关SignalR的扩展方法

    #region 注入SignalRProvide对象
    /// <summary>
    /// 以瞬间模式注入一个<see cref="ISignalRProvide"/>对象，
    /// 该依赖注入能够自动处理绝对路径和相对路径的转换，
    /// 它依赖于<see cref="IHostProvide"/>服务
    /// </summary>
    /// <param name="services">待注入的容器</param>
    /// <param name="create">该委托传入中心的绝对Uri，以及一个用来提供服务的对象，
    /// 然后创建一个新的<see cref="IHubConnectionBuilder"/>，如果为<see langword="null"/>，则使用默认方法</param>
    /// <returns></returns>
    public static IServiceCollection AddSignalRProvide(this IServiceCollection services, Func<string, IServiceProvider, Task<IHubConnectionBuilder>>? create = null)
        => services.AddTransient(server =>
        {
            var navigation = server.GetRequiredService<IHostProvide>();
            return CreateNet.SignalRProvide(create is null ? null : uri => create(uri, server),
                uri => navigation.Convert(uri, true));
        });
    #endregion
    #region 按接口注册客户端方法
    /// <summary>
    /// 将一个接口中的所有方法注册为Hub连接的客户端方法
    /// </summary>
    /// <typeparam name="Interface">待注册客户端方法的接口</typeparam>
    /// <param name="connection">要注册客户端方法的Hub连接</param>
    /// <param name="instance">接口的实例，函数会从这个实例调用它的方法</param>
    public static void OnStrong<Interface>(this HubConnection connection, Interface instance)
        where Interface : class
    {
        var methods = typeof(Interface).GetMethods(CreateReflection.BindingFlagsAllVisibility | BindingFlags.Instance);
        foreach (var method in methods)
        {
            connection.On(method.Name, method.GetParameterTypes(),
                async (parameter, _) =>
            {
                var task = method.Invoke(instance, parameter);
                switch (task)
                {
                    case Task t:
                        await t;
                        break;
                    case ValueTask valueTask:
                        await valueTask;
                        break;
                }
            }, new());
        }
    }
    #endregion
    #region 返回强类型调用封装
    /// <summary>
    /// 返回一个<see cref="HubConnection"/>的强类型封装
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="SignalRStrongTypeInvoke{Hub}.SignalRStrongTypeInvoke(HubConnection)"/>
    public static ISignalRStrongTypeInvoke<Hub> StrongType<Hub>(this HubConnection connection)
        where Hub : class
        => new SignalRStrongTypeInvoke<Hub>(connection);
    #endregion
}
