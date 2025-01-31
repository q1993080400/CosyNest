using System.Linq.Expressions;

namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以以Http请求的形式发起强类型调用
/// </summary>
/// <inheritdoc cref="IStrongTypeInvoke{API}"/>
public interface IHttpStrongTypeInvoke<API> : IStrongTypeInvoke<API>
    where API : class
{
    #region 直接返回Http响应
    /// <summary>
    /// 以Http请求的形式发起强类型调用，
    /// 并直接返回Http响应
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IStrongTypeInvoke{API}.Invoke{Ret}(Expression{Func{API, Task{Ret}}}, CancellationToken)"/>
    Task<HttpResponseMessage> RequestResponse<Ret>(Expression<Func<API, Ret>> invoke, CancellationToken cancellationToken = default);
    #endregion
}
