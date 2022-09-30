namespace System.Threading.Tasks;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个异步属性
/// </summary>
/// <typeparam name="Value">异步属性的值的类型</typeparam>
public interface IAsyncProperty<Value>
{
    #region 获取值
    /// <summary>
    /// 获取异步属性的值
    /// </summary>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    Task<Value> Get(CancellationToken cancellation = default);
    #endregion
    #region 写入值
    #region 传入Task<T>
    /// <summary>
    /// 写入异步属性的值
    /// </summary>
    /// <param name="value">待写入的值</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    async Task Set(Task<Value> value, CancellationToken cancellation = default)
         => await Set(await value, cancellation);
    #endregion
    #region 传入ValueTask<T>
    /// <inheritdoc cref="Set(Task{Value}, CancellationToken)"/>
    Task Set(ValueTask<Value> value, CancellationToken cancellation = default)
        => Set(value.AsTask(), cancellation);
    #endregion
    #region 传入同步对象
    /// <inheritdoc cref="Set(Task{Value}, CancellationToken)"/>
    Task Set(Value value, CancellationToken cancellation = default);
    #endregion
    #endregion
}
