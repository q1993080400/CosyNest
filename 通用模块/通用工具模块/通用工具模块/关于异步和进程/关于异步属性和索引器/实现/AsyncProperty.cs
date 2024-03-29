﻿namespace System.Threading.Tasks;

/// <summary>
/// 该类型是<see cref="IAsyncProperty{Value}"/>的实现，
/// 它使用委托来读写异步属性
/// </summary>
/// <inheritdoc cref="IAsyncProperty{Value}"/>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="getDelegate">用于读取异步属性的委托</param>
/// <param name="setDelegate">用于写入异步属性的委托</param>
sealed class AsyncProperty<Value>(Func<CancellationToken, Task<Value>> getDelegate, Func<Value, CancellationToken, Task> setDelegate) : IAsyncProperty<Value>
{
    #region 读取异步属性
    #region 正式属性
    public Task<Value> Get(CancellationToken cancellation)
         => GetDelegate(cancellation);
    #endregion
    #region 委托
    /// <summary>
    /// 这个委托用于读取异步属性
    /// </summary>
    private Func<CancellationToken, Task<Value>> GetDelegate { get; } = getDelegate;
    #endregion
    #endregion
    #region 写入异步属性
    #region 正式方法
    public Task Set(Value value, CancellationToken cancellation)
        => SetDelegate(value, cancellation);
    #endregion
    #region 委托
    /// <summary>
    /// 这个委托用于写入异步属性
    /// </summary>
    private Func<Value, CancellationToken, Task> SetDelegate { get; } = setDelegate;

    #endregion
    #endregion
}
