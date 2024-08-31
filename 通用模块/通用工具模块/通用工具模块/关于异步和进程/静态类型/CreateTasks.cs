namespace System.Threading.Tasks;

/// <summary>
/// 这个静态类可以用来创建和多线程有关的对象
/// </summary>
public static class CreateTasks
{
    #region 创建异步属性
    /// <summary>
    /// 使用指定的读取和写入委托创建异步属性
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IAsyncProperty{Value}"/>
    /// <inheritdoc cref="AsyncProperty{Value}.AsyncProperty(Func{CancellationToken, Task{Value}}, Func{Value, CancellationToken, Task})"/>
    public static IAsyncProperty<Value> AsyncProperty<Value>(Func<CancellationToken, Task<Value>> getDelegate, Func<Value, CancellationToken, Task> setDelegate)
        => new AsyncProperty<Value>(getDelegate, setDelegate);
    #endregion
    #region 创建异步索引器
    /// <summary>
    /// 使用指定的读取和写入委托创建只有一个参数的异步索引器
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IAsyncIndex{P1, Value}"/>
    /// <inheritdoc cref="AsyncIndexP1{P1, Value}.AsyncIndexP1(Func{P1, CancellationToken, Task{Value}}, Func{P1, Value, CancellationToken, Task})"/>
    public static IAsyncIndex<P1, Value> AsyncIndex<P1, Value>(Func<P1, CancellationToken, Task<Value>> getDelegate, Func<P1, Value, CancellationToken, Task> setDelegate)
        => new AsyncIndexP1<P1, Value>(getDelegate, setDelegate);
    #endregion
}
