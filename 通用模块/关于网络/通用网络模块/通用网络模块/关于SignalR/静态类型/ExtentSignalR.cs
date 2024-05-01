using System.Linq.Expressions;

namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 有关SignalR的扩展方法全部放到本类型中
/// </summary>
public static class ExtendSignalR
{
    #region 强类型调用中心方法
    #region 说明文档
    /*问：强类型调用中心方法是什么意思？
      答：SignalR支持在服务器通过强类型的方式调用客户端方法，
      即使用Hub<T>的形式，而本设计模式是对它的反向操作，
      支持在客户端以强类型的方式调用服务器上的方法*/
    #endregion
    #region 基础方法
    /// <summary>
    /// 基础方法，它调用Hub中心方法，然后返回返回值
    /// </summary>
    /// <exception cref="ArgumentException">无法识别表达式</exception>
    /// <inheritdoc cref="InvokeAsync{Hub, Ret}(HubConnection, Expression{Func{Hub, Ret}}, CancellationToken)"/>
    private static async Task<Ret> InvokeBase<Ret>(HubConnection hub, LambdaExpression invoke, CancellationToken cancellation = default)
    {
        if (invoke is
            {
                Body: MethodCallExpression
                {
                    Object: ParameterExpression { },
                    Method.Name: { } name,
                    Arguments: { } parameter
                }
            })
        {
            var p = parameter.Select(x => x.CalValue()).ToArray();
            if (typeof(Ret).IsGenericRealize(typeof(IAsyncEnumerable<>)))       //当返回类型为异步流时，调用流式方法
            {
                var method = typeof(HubConnection).GetTypeData().FindMethod(nameof(HubConnection.StreamAsyncCore)).
                    MakeGenericMethod(typeof(Ret).GenericTypeArguments[0]);
                return (Ret)method.Invoke<object>(hub, name, p, cancellation)!;
            }
            return (Ret)(await hub.InvokeCoreAsync(name, typeof(Ret), p, cancellation))!;       //否则调用普通方法
        }
        throw new ArgumentException($"无法识别表达式{invoke}");
    }
    #endregion
    #region 有返回值
    #region 为异步方法优化
    /// <summary>
    /// 以强类型的方式，在客户端调用Hub中心的方法，并返回方法的返回值，
    /// 如果返回值是<see cref="IAsyncEnumerable{T}"/>，则在底层会调用流式方法
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <typeparam name="Hub">强类型Hub中心</typeparam>
    /// <param name="hub">Hub客户端</param>
    /// <param name="invoke">该表达式用于确定调用Hub中心的哪一个方法，以及传入什么参数</param>
    /// <param name="cancellation">一个用于取消操作的令牌</param>
    /// <returns></returns>
    public static Task<Ret> InvokeAsync<Hub, Ret>(this HubConnection hub, Expression<Func<Hub, Task<Ret>>> invoke, CancellationToken cancellation = default)
        => InvokeBase<Ret>(hub, invoke, cancellation);
    #endregion
    #region 为同步方法优化
    /// <inheritdoc cref="InvokeAsync{Hub,Ret}(HubConnection, Expression{Func{Hub, Task{Ret}}}, CancellationToken)"/>
    public static Task<Ret> InvokeAsync<Hub, Ret>(this HubConnection hub, Expression<Func<Hub, Ret>> invoke, CancellationToken cancellation = default)
        => InvokeBase<Ret>(hub, invoke, cancellation);
    #endregion
    #endregion
    #region 无返回值
    #region 为异步方法优化
    /// <summary>
    /// 以强类型的方式，在客户端调用Hub中心的方法，不返回值
    /// </summary>
    /// <inheritdoc cref="InvokeAsync{Hub,Ret}(HubConnection, Expression{Func{Hub, Task{Ret}}}, CancellationToken)"/>
    public static Task InvokeVoidAsync<Hub>(this HubConnection hub, Expression<Func<Hub, Task>> invoke, CancellationToken cancellation = default)
        => InvokeBase<object>(hub, invoke, cancellation);
    #endregion
    #region 为同步方法优化
    /// <inheritdoc cref="InvokeVoidAsync{Hub}(HubConnection, Expression{Func{Hub, Task}}, CancellationToken)"/>
    public static Task InvokeVoidAsync<Hub>(this HubConnection hub, Expression<Action<Hub>> invoke, CancellationToken cancellation = default)
        => InvokeBase<object>(hub, invoke, cancellation);
    #endregion
    #endregion
    #endregion
    #region 按接口注册客户端方法
    /// <summary>
    /// 将一个接口中的所有方法注册为Hub连接的客户端方法
    /// </summary>
    /// <typeparam name="Interface">待注册客户端方法的接口</typeparam>
    /// <param name="connection">要注册客户端方法的Hub连接</param>
    /// <param name="instance">接口的实例，函数会从这个实例调用它的方法</param>
    public static void OnStrong<Interface>(this HubConnection connection, Interface instance)
        where Interface : class
    {
        var methods = typeof(Interface).GetTypeData().Methods;
        foreach (var method in methods)
        {
            connection.On(method.Name, method.GetParameterTypes(),
                async (parameter, _) =>
            {
                var task = method.Invoke(instance, parameter);
                switch (task)
                {
                    case Task t:
                        await t;
                        break;
                    case ValueTask valueTask:
                        await valueTask;
                        break;
                }
            }, new());
        }
    }
    #endregion
}
