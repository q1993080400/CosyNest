using Microsoft.AspNetCore.SignalR.Client;

namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来作为提供SignalR连接的工厂
/// </summary>
public interface ISignalRFactory : IAsyncDisposable
{
    #region 获取SignalR连接
    #region 直接指定Uri
    /// <summary>
    /// 获取连接到指定Uri的SignalR连接，
    /// 当<see cref="Task{TResult}"/>被等待完毕时，
    /// 连接已经启动完成，可以直接使用
    /// </summary>
    /// <param name="uri">SignalR中心的Uri，
    /// 它可以是相对的，也可以是绝对的</param>
    /// <param name="configuration">这个委托可以用来配置新创建的<see cref="HubConnection"/>，
    /// 它最常见的用途是为<see cref="HubConnection"/>注册客户端方法</param>
    /// <param name="cacheConnection">如果这个值为<see langword="true"/>，则缓存每个连接，
    /// 否则始终新建连接，新建的连接需要调用者自己想办法释放</param>
    /// <returns></returns>
    Task<HubConnection> Create(string uri, Func<HubConnection, Task>? configuration = null, bool cacheConnection = true);
    #endregion
    #region 通过业务接口自动获取Uri
    /// <typeparam name="BusinessInterface">Hub中心实现的业务接口，
    /// 如果在注册中心的时候使用默认路由，函数可以通过它推断出中心的Uri</typeparam>
    /// <inheritdoc cref="Create(string, Func{HubConnection, Task}?, bool)"/>
    Task<HubConnection> Create<BusinessInterface>(Func<HubConnection, Task>? configuration = null, bool cacheConnection = true)
        where BusinessInterface : class;
    #endregion
    #endregion
    #region 通过业务接口自动获取Uri，且返回它的强类型封装
    #region 立即连接
    /// <summary>
    /// 获取指定Hub中心的SignalR连接的强类型封装，
    /// 当返回的时候，它已经连接完毕，可以直接使用
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Create{BusinessInterface}(Func{HubConnection, Task}?, bool)"/>
    Task<IStrongTypeStreamInvoke<BusinessInterface>> StrongType<BusinessInterface>
       (Func<HubConnection, Task>? configuration = null, bool cacheConnection = true)
       where BusinessInterface : class;
    #endregion
    #region 延迟连接
    /// <summary>
    /// 获取指定Hub中心的SignalR连接的强类型封装，
    /// 它在第一次请求Hub中心的时候延迟建立连接
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Create{BusinessInterface}(Func{HubConnection, Task}?, bool)"/>
    IStrongTypeStreamInvoke<BusinessInterface> StrongTypeLazy<BusinessInterface>
       (Func<HubConnection, Task>? configuration = null, bool cacheConnection = true)
       where BusinessInterface : class;
    #endregion
    #endregion 
}
