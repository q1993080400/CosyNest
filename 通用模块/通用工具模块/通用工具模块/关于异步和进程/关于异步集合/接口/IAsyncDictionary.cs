namespace System.Collections.Generic;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个异步字典
/// </summary>
/// <typeparam name="Key">字典的键类型</typeparam>
/// <typeparam name="Value">字典的值类型</typeparam>
public interface IAsyncDictionary<Key, Value> : IAsyncCollection<KeyValuePair<Key, Value>>
    where Key : notnull
{
    #region 关于键值对集合
    #region 获取键集合
    /// <summary>
    /// 获取异步字典的所有键
    /// </summary>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    IAsyncEnumerable<Key> KeysAsync(CancellationToken cancellation = default)
        => this.Fit().Select(x => x.Key).Fit();
    #endregion
    #region 获取值集合
    /// <summary>
    /// 获取异步字典的所有值
    /// </summary>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    IAsyncEnumerable<Value> ValuesAsync(CancellationToken cancellation = default)
        => this.Fit().Select(x => x.Value).Fit();
    #endregion
    #endregion
    #region 关于添加，移除和检查键值对
    #region 添加键值对
    /// <summary>
    /// 向字典中异步添加键值对
    /// </summary>
    /// <param name="key">待添加的键</param>
    /// <param name="value">待添加的值</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    Task AddAsync(Key key, Value value, CancellationToken cancellation = default)
        => AddAsync(new(key, value), cancellation);
    #endregion
    #region 移除指定的键
    /// <summary>
    /// 以异步的方式移除具有指定键的键值对，
    /// 并返回是否移除成功
    /// </summary>
    /// <param name="key">待移除的键</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    Task<bool> RemoveAsync(Key key, CancellationToken cancellation = default);
    #endregion
    #region 检查是否存在指定的键
    /// <summary>
    /// 检查异步字典中是否存在指定的键
    /// </summary>
    /// <param name="key">待检查的键</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    async Task<bool> ContainsKeyAsync(Key key, CancellationToken cancellation = default)
     => (await TryGetValueAsync(key, cancellation)).Exist;
    #endregion
    #endregion
    #region 关于读取或写入键值对
    #region 获取值且不引发异常
    /// <summary>
    /// 尝试通过键获取值，
    /// 如果键不存在，不会引发异常
    /// </summary>
    /// <param name="key">用来获取值的键</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns>一个元组，它的项分别是是否存在指定的键，
    /// 以及获取到的值，如果键不存在，则为默认值</returns>
    Task<(bool Exist, Value? Value)> TryGetValueAsync(Key key, CancellationToken cancellation = default);
    #endregion
    #region 读取或写入值（异步索引器）
    /// <summary>
    /// 通过该异步索引器，
    /// 可以通过键来异步读取或写入值
    /// </summary>
    IAsyncIndex<string, string> IndexAsync { get; }
    #endregion
    #endregion
}
