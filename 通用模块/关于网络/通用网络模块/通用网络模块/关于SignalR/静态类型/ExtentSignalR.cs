namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 有关SignalR的扩展方法全部放到本类型中
/// </summary>
public static class ExtendSignalR
{
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
    #region 返回强类型调用封装
    /// <summary>
    /// 返回一个<see cref="HubConnection"/>的强类型封装
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="SignalRStrongTypeInvoke{Hub}.SignalRStrongTypeInvoke(HubConnection)"/>
    public static ISignalRStrongTypeInvoke<Hub> StrongType<Hub>(this HubConnection connection)
        where Hub : class
        => new SignalRStrongTypeInvoke<Hub>(connection);
    #endregion
}
