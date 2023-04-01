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
    /// <returns></returns>
    Task<HubConnection> GetConnection(string uri);
    #endregion
    #region 通过业务接口自动获取Uri
    /// <typeparam name="BusinessInterface">Hub中心实现的业务接口，
    /// 如果在注册中心的时候使用默认路由，函数可以通过它推断出中心的Uri</typeparam>
    /// <inheritdoc cref="GetConnection(string)"/>
    Task<HubConnection> GetConnection<BusinessInterface>()
        where BusinessInterface : class
    {
        var uri = "Hub/" + typeof(BusinessInterface).Name.TrimStart('I');
        return GetConnection(uri);
    }
    #endregion
    #endregion
    #region 设置用于配置SignalR连接的委托
    /// <summary>
    /// 设置用于配置SignalR连接的委托，
    /// 这个方法只能调用一次
    /// </summary>
    /// <param name="configuration">用于配置连接的委托，
    /// 它的第一个参数是待配置的连接，第二个参数是中心的绝对Uri，该委托一般用来注册客户端方法</param>
    void SetConfiguration(Action<HubConnection, string> configuration);

    /*问：配置客户端方法似乎不需要Hub中心的Uri，
      那么，为什么需要第二个参数？
      答：这是因为本接口是一个工厂，它缓存了多个SignalR连接，
      需要通过Uri来区分给哪一个连接添加客户端方法*/
    #endregion
}
