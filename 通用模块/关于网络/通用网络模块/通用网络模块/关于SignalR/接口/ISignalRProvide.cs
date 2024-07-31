namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来提供SignalR连接
/// </summary>
public interface ISignalRProvide : IAsyncDisposable
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
    /// <param name="newBuilt">如果这个值为<see langword="true"/>，
    /// 则始终新建连接，不缓存连接，新建的连接需要调用者自己想办法释放</param>
    /// <returns></returns>
    Task<HubConnection> GetConnection(string uri, Action<HubConnection>? configuration = null, bool newBuilt = false);
    #endregion
    #region 通过业务接口自动获取Uri
    /// <typeparam name="BusinessInterface">Hub中心实现的业务接口，
    /// 如果在注册中心的时候使用默认路由，函数可以通过它推断出中心的Uri</typeparam>
    /// <inheritdoc cref="GetConnection(string, Action{HubConnection}?, bool)"/>
    Task<HubConnection> GetConnection<BusinessInterface>(Action<HubConnection>? configuration = null, bool newBuilt = false)
        where BusinessInterface : class
    {
        var uri = ToolSignalR.GetHubDefaultUri<BusinessInterface>();
        return GetConnection(uri, configuration, newBuilt);
    }
    #endregion
    #region 通过业务接口自动获取Uri，且返回它的强类型封装
    /// <summary>
    /// 获取指定Hub中心的SignalR连接的强类型封装，
    /// 当返回的时候，它已经连接完毕，可以直接使用
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetConnection{BusinessInterface}(Action{HubConnection}?, bool)"/>
    async Task<ISignalRStrongTypeInvoke<BusinessInterface>> GetConnectionStrongType<BusinessInterface>
        (Action<HubConnection>? configuration = null, bool newBuilt = false)
        where BusinessInterface : class
    {
        var connection = await GetConnection<BusinessInterface>(configuration, newBuilt);
        return connection.StrongType<BusinessInterface>();
    }
    #endregion
    #endregion
}
