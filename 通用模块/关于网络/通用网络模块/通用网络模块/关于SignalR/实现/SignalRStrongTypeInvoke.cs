using System.Linq.Expressions;

namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 这个类型是<see cref="ISignalRStrongTypeInvoke{Hub}"/>的实现，
/// 可以用来封装强类型SignalR调用
/// </summary>
/// <param name="connection">封装的SignalR连接对象</param>
/// <inheritdoc cref="ISignalRStrongTypeInvoke{Hub}"/>
sealed class SignalRStrongTypeInvoke<Hub>(HubConnection connection) : ISignalRStrongTypeInvoke<Hub>
    where Hub : class
{
    #region 公开成员
    #region 有返回值
    public async Task<Ret> Invoke<Ret>(Expression<Func<Hub, Task<Ret>>> invoke, CancellationToken cancellationToken = default)
    {
        var (name, parameter) = AnalysisExpression(invoke);
        return await connection.InvokeCoreAsync<Ret>(name, parameter, cancellationToken);
    }
    #endregion
    #region 无返回值
    public async Task Invoke(Expression<Func<Hub, Task>> invoke, CancellationToken cancellationToken = default)
    {
        var (name, parameter) = AnalysisExpression(invoke);
        await connection.InvokeCoreAsync(name, parameter, cancellationToken);
    }
    #endregion
    #region 返回异步流
    public IAsyncEnumerable<Ret> Invoke<Ret>(Expression<Func<Hub, IAsyncEnumerable<Ret>>> invoke, CancellationToken cancellationToken = default)
    {
        var (name, parameter) = AnalysisExpression(invoke);
        return connection.StreamAsyncCore<Ret>(name, parameter, cancellationToken);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 解析调用的方法和参数
    /// <summary>
    /// 解析表达式，并返回一个元组，
    /// 它的项分别是要调用的方法的名称，
    /// 以及调用方法的参数
    /// </summary>
    /// <param name="invoke">要解析的表达式</param>
    /// <returns></returns>
    private static (string Method, object?[] Parameter) AnalysisExpression(LambdaExpression invoke)
    {
        if (invoke is not
            {
                Body: MethodCallExpression
                {
                    Object: ParameterExpression { },
                    Method.Name: { } name,
                    Arguments: { } parameter
                }
            })
            throw new ArgumentException($"无法识别表达式{invoke}，它的格式不正确");
        var p = parameter.Select(x => x.CalValue()).ToArray();
        return (name, p);
    }
    #endregion
    #endregion
}
