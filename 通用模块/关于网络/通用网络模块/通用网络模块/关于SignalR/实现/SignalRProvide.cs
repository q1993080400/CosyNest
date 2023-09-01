using System.NetFrancis.Http;

namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 该类型是<see cref="ISignalRProvide"/>的实现
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="create">该委托传入中心的绝对Uri，
/// 然后创建一个新的<see cref="HubConnection"/>，如果为<see langword="null"/>，则使用默认方法</param>
/// <param name="toAbs">这个对象可以将相对Uri转换为绝对Uri，
/// 如果为<see langword="null"/>，则不进行转换</param>
sealed class SignalRProvide(Func<string, Task<HubConnection>> create, Func<string, string>? toAbs) : ISignalRProvide
{
    #region 有关获取连接
    #region 缓存
    /// <summary>
    /// 该属性按照Uri缓存已经创建的连接
    /// </summary>
    private Dictionary<string, Task<HubConnection>> Cache { get; } = new();
    #endregion
    #region 用于创建连接的委托
    /// <summary>
    /// 该委托传入中心的绝对Uri，
    /// 然后创建一个新的<see cref="HubConnection"/>
    /// </summary>
    private Func<string, Task<HubConnection>> Create { get; } = create;
    #endregion
    #region 用于转换Uri的对象
    /// <summary>
    /// 这个对象可以将相对Uri转换为绝对Uri
    /// </summary>
    private Func<string, string> ToAbs { get; } = toAbs ??= x => x;
    #endregion
    #region 正式方法
    public async Task<HubConnection> GetConnection(string uri, bool newBuilt = false)
    {
        var newUri = ToAbs(uri);
        var uriExtend = new UriComplete(newUri).UriExtend!;
        if (newBuilt)
        {
            var newConnection = await Create(newUri);
            Configuration?.Invoke(newConnection, uriExtend);
            await newConnection.StartAsync();
            return newConnection;
        }
        var (exist, task) = Cache.TrySetValue(newUri, Create);
        var value = await task;
        if (!exist)
            Configuration?.Invoke(value, uriExtend);
        if (value.State is HubConnectionState.Disconnected)
            await value.StartAsync();
        return value;
    }
    #endregion
    #endregion
    #region 用于注册客户端方法的委托
    private Action<HubConnection, string>? Configuration { get; set; }

    public void SetConfiguration(Action<HubConnection, string> configuration)
    {
        Configuration ??= configuration;
    }
    #endregion
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        foreach (var item in Cache.Values)
        {
            var value = await item;
            await value.DisposeAsync();
        }
        Cache.Clear();
    }

    #endregion
}
