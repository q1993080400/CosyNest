using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.SignalR.Client;

namespace System.NetFrancis;

/// <summary>
/// 这个类型是<see cref="IStrongTypeStreamInvoke{API}"/>的实现，
/// 可以用来封装强类型SignalR调用
/// </summary>
/// <param name="createConnection">用来创建SignalR连接的委托，
/// 当连接被创建时，需要保证它已经处于正常连接状态</param>
/// <inheritdoc cref="IStrongTypeStreamInvoke{API}"/>
sealed class SignalRStrongTypeInvoke<API>(Func<Task<HubConnection>> createConnection) : IStrongTypeStreamInvoke<API>
    where API : class
{
    #region 公开成员
    #region 有返回值
    public async Task<Ret> Invoke<Ret>(Expression<Func<API, Task<Ret>>> invoke, CancellationToken cancellationToken = default)
    {
        var (name, parameter) = AnalysisExpression(invoke);
        var connection = await createConnection();
        return await connection.InvokeCoreAsync<Ret>(name, parameter, cancellationToken);
    }
    #endregion
    #region 无返回值
    public async Task Invoke(Expression<Func<API, Task>> invoke, CancellationToken cancellationToken = default)
    {
        var (name, parameter) = AnalysisExpression(invoke);
        var connection = await createConnection();
        await connection.InvokeCoreAsync(name, parameter, cancellationToken);
    }
    #endregion
    #region 返回异步流
    public async IAsyncEnumerable<Ret> Invoke<Ret>(Expression<Func<API, IAsyncEnumerable<Ret>>> invoke, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var (name, parameter) = AnalysisExpression(invoke);
        var connection = await createConnection();
        var list = connection.StreamAsyncCore<Ret>(name, parameter, cancellationToken);
        await foreach (var item in list)
        {
            yield return item;
        }
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
        var p = parameter.Select(static x => x.CalValue()).ToArray();
        return (name, p);
    }
    #endregion
    #endregion
}
