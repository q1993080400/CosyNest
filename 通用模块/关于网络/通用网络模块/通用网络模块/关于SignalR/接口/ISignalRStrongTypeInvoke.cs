using System.Linq.Expressions;

namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来进行强类型SignalR调用
/// </summary>
/// <typeparam name="Hub">Hub中心的类型</typeparam>
public interface ISignalRStrongTypeInvoke<Hub>
    where Hub : class
{
    #region 有返回值
    /// <summary>
    /// 进行一个强类型SignalR调用，
    /// 并返回返回值
    /// </summary>
    /// <typeparam name="Ret">返回值的类型</typeparam>
    /// <param name="invoke">描述调用过程的表达式，
    /// 它的参数是Hub中心，返回值是Hub方法调用的返回值</param>
    /// <param name="cancellationToken">一个用于取消异步调用的对象</param>
    /// <returns></returns>
    Task<Ret> Invoke<Ret>(Expression<Func<Hub, Task<Ret>>> invoke, CancellationToken cancellationToken = default);
    #endregion
    #region 返回异步流
    /// <summary>
    /// 进行一个强类型SignalR调用，
    /// 并返回一个异步流
    /// </summary>
    /// <typeparam name="Ret">返回的异步流的元素的类型</typeparam>
    /// <param name="invoke">描述调用过程的表达式，
    /// 它的参数是Hub中心，返回值是Hub方法返回的异步流</param>
    /// <param name="cancellationToken">一个用于取消异步调用的对象</param>
    /// <returns></returns>
    IAsyncEnumerable<Ret> Invoke<Ret>(Expression<Func<Hub, IAsyncEnumerable<Ret>>> invoke, CancellationToken cancellationToken = default);
    #endregion
    #region 无返回值
    /// <summary>
    /// 进行一个强类型SignalR调用，
    /// 且不返回值
    /// </summary>
    /// <param name="invoke">描述调用过程的表达式，它的参数是Hub中心</param>
    /// <param name="cancellationToken">一个用于取消异步调用的对象</param>
    /// <returns></returns>
    Task Invoke(Expression<Func<Hub, Task>> invoke, CancellationToken cancellationToken = default);
    #endregion
}
