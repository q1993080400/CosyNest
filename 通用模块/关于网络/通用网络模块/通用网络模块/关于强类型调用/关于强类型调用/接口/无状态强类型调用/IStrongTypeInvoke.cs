using System.Linq.Expressions;

namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个强类型调用
/// </summary>
/// <typeparam name="API">强类型API的类型</typeparam>
public interface IStrongTypeInvoke<API>
    where API : class
{
    #region 强类型调用
    #region 无返回值
    /// <summary>
    /// 发起强类型调用，且不返回结果
    /// </summary>
    /// <param name="invoke">用来描述调用过程的表达式</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Invoke(Expression<Func<API, Task>> invoke, CancellationToken cancellationToken = default);
    #endregion
    #region 有返回值
    /// <summary>
    /// 发起强类型调用，且返回指定的结果
    /// </summary>
    /// <typeparam name="Ret">返回值的类型</typeparam>
    /// <returns></returns>
    /// <inheritdoc cref="Invoke(Expression{Func{API, Task}}, CancellationToken)"/>
    Task<Ret> Invoke<Ret>(Expression<Func<API, Task<Ret>>> invoke, CancellationToken cancellationToken = default);
    #endregion
    #endregion
}
