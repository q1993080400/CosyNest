using System.Collections.Concurrent;

namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 该类型是<see cref="ISignalRProvide"/>的实现，
/// 可以用来创建一个<see cref="HubConnection"/>连接
/// </summary>
/// <param name="create">该委托传入中心的绝对Uri，
/// 然后创建一个新的<see cref="IHubConnectionBuilder"/></param>
/// <param name="toAbs">这个对象可以将相对Uri转换为绝对Uri</param>
sealed class SignalRProvide(Func<string, Task<IHubConnectionBuilder>> create, Func<string, string> toAbs) : ISignalRProvide
{
    #region 有关获取连接
    #region 缓存
    /// <summary>
    /// 该属性按照Uri缓存已经创建的连接
    /// </summary>
    private ConcurrentDictionary<string, HubConnection> Cache { get; } = [];
    #endregion
    #region 正式方法
    public async Task<HubConnection> GetConnection(string uri, Action<HubConnection>? configuration = null, bool newBuilt = false)
    {
        var newUri = toAbs(uri);
        #region 用于创建HubConnection的本地函数
        async Task<HubConnection> Create()
        {
            var builder = await create(newUri);
            return builder.Build();
        }
        #endregion
        HubConnection connection;
        if (newBuilt)
        {
            connection = await Create();
            configuration?.Invoke(connection);
            await connection.StartAsync();
            return connection;
        }
        var needConfiguration = false;
        if (Cache.GetValueOrDefault(newUri) is { } oldConnection)
        {
            connection = oldConnection;
        }
        else
        {
            connection = await Create();
            Cache[newUri] = connection;
            needConfiguration = true;
        }
        if (needConfiguration)
            configuration?.Invoke(connection);
        if (connection.State is HubConnectionState.Disconnected)
            await connection.StartAsync();
        return connection;
    }
    #endregion
    #endregion
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        foreach (var item in Cache.Values)
        {
            await item.DisposeAsync();
        }
        Cache.Clear();
    }
    #endregion
}
