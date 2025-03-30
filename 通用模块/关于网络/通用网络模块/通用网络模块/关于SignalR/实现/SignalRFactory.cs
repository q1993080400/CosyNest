using System.Collections.Concurrent;

using Microsoft.AspNetCore.SignalR.Client;

namespace System.NetFrancis;

/// <summary>
/// 该类型是<see cref="ISignalRFactory"/>的实现，
/// 可以用来创建一个<see cref="HubConnection"/>连接
/// </summary>
/// <param name="configurationSignalRFactory">用于配置连接的接口</param>
sealed class SignalRFactory(IConfigurationSignalRFactory configurationSignalRFactory) : ISignalRFactory
{
    #region 公开成员
    #region 获取SignalR连接
    #region 直接指定Uri
    public async Task<HubConnection> Create(string uri, Func<HubConnection, Task>? configuration, bool cacheConnection)
    {
        configuration ??= static _ => Task.CompletedTask;
        #region 用于创建HubConnection的本地函数
        async Task<HubConnection> Create()
        {
            var builder = await configurationSignalRFactory.Builder(uri);
            return builder.Build();
        }
        #endregion
        if (!cacheConnection)
        {
            var newConnection = await Create();
            await configuration(newConnection);
            await newConnection.StartAsync();
            return newConnection;
        }
        var absoluteUri = configurationSignalRFactory.ToAbsoluteUri(uri);
        #region 用于创建新连接的本地函数
        async Task<HubConnection> CreateNewConnection()
        {
            var connection = await Create();
            Cache[absoluteUri] = connection;
            await configuration(connection);
            return connection;
        }
        #endregion
        var connection = Cache.GetValueOrDefault(absoluteUri) ?? await CreateNewConnection();
        await connection.StartSecureAsync();
        return connection;
    }
    #endregion
    #region 通过业务接口自动获取Uri
    public Task<HubConnection> Create<BusinessInterface>(Func<HubConnection, Task>? configuration = null, bool cacheConnection = true)
         where BusinessInterface : class
    {
        var uri = ToolSignalR.GetHubDefaultUri<BusinessInterface>();
        return Create(uri, configuration, cacheConnection);
    }
    #endregion
    #endregion
    #region 通过业务接口自动获取Uri，且返回它的强类型封装
    #region 立即连接
    public async Task<IStrongTypeSignalRInvoke<BusinessInterface>> StrongType<BusinessInterface>
        (Func<HubConnection, Task>? configuration = null, bool cacheConnection = true)
        where BusinessInterface : class
    {
        var connection = await Create<BusinessInterface>(configuration, cacheConnection);
        return new StrongTypeSignalRInvoke<BusinessInterface>(() => connection.StartSecureAsync());
    }
    #endregion
    #region 延迟连接
    public IStrongTypeSignalRInvoke<BusinessInterface> StrongTypeLazy<BusinessInterface>
        (Func<HubConnection, Task>? configuration = null, bool cacheConnection = true)
        where BusinessInterface : class
        => new StrongTypeSignalRInvoke<BusinessInterface>(() => Create<BusinessInterface>(configuration, cacheConnection));
    #endregion 
    #endregion
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        var values = Cache.Values.ToArray();
        Cache.Clear();
        foreach (var item in values)
        {
            await item.DisposeAsync();
        }
    }
    #endregion
    #endregion
    #region 内部成员
    #region 缓存
    /// <summary>
    /// 该属性按照Uri缓存已经创建的连接
    /// </summary>
    private ConcurrentDictionary<string, HubConnection> Cache { get; } = [];
    #endregion
    #endregion
}
