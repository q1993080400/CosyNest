namespace System.Threading.Tasks;

/// <summary>
/// 这个类型是<see cref="IAsyncIndex{P1, Value}"/>的实现，
/// 可以视为一个只有一个参数的异步索引器
/// </summary>
/// <inheritdoc cref="IAsyncIndex{P1, Value}"/>
sealed class AsyncIndexP1<P1, Value> : IAsyncIndex<P1, Value>
{
    #region 读取索引器
    #region 正式方法
    public Task<Value> Get(P1 p1, CancellationToken cancellation)
        => GetDelegate(p1, cancellation);
    #endregion
    #region 委托
    /// <summary>
    /// 这个委托用于读取异步索引器
    /// </summary>
    private Func<P1, CancellationToken, Task<Value>> GetDelegate { get; }
    #endregion
    #endregion
    #region 写入索引器
    #region 正式方法
    public Task Set(P1 p1, Value value, CancellationToken cancellation)
        => SetDelegate(p1, value, cancellation);
    #endregion
    #region 委托
    /// <summary>
    /// 这个委托用于写入异步索引器
    /// </summary>
    private Func<P1, Value, CancellationToken, Task> SetDelegate { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的读取和写入委托初始化异步索引器
    /// </summary>
    /// <param name="getDelegate">用于读取异步索引器的委托</param>
    /// <param name="setDelegate">用于写入异步索引器的委托</param>
    public AsyncIndexP1(Func<P1, CancellationToken, Task<Value>> getDelegate, Func<P1, Value, CancellationToken, Task> setDelegate)
    {
        this.GetDelegate = getDelegate;
        this.SetDelegate = setDelegate;
    }
    #endregion
}
