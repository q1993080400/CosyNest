using System.Linq.Expressions;

namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个支持调用异步流的强类型调用
/// </summary>
/// <inheritdoc cref="IStrongTypeInvoke{API}"/>
public interface IStrongTypeStreamInvoke<API> : IStrongTypeInvoke<API>
    where API : class
{
    #region 返回异步流
    /// <summary>
    /// 进行一个强类型调用，
    /// 并返回一个异步流
    /// </summary>
    /// <inheritdoc cref="IStrongTypeInvoke{API}.Invoke{Ret}(Expression{Func{API, Task{Ret}}}, CancellationToken)"/>
    IAsyncEnumerable<Ret> Invoke<Ret>(Expression<Func<API, IAsyncEnumerable<Ret>>> invoke, CancellationToken cancellationToken = default);
    #endregion
}
