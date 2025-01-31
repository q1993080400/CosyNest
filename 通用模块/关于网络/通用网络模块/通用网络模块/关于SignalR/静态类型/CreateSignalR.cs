using System.Reflection;

using Microsoft.AspNetCore.SignalR.Client;

namespace System.NetFrancis;

public static partial class CreateNet
{
    //这个部分类专门用来声明用来创建有关SignalR的对象的方法

    #region 创建ISignalRProvide
    /// <summary>
    /// 创建一个<see cref="ISignalRFactory"/>，
    /// 它可以用来提供SignalR连接
    /// </summary>
    /// <inheritdoc cref="SignalRFactory.SignalRFactory(IConfigurationSignalRFactory)"/>
    public static ISignalRFactory SignalRFactory(IConfigurationSignalRFactory configurationSignalRFactory)
        => new SignalRFactory(configurationSignalRFactory);
    #endregion
    #region 注册SignalR客户端
    /// <summary>
    /// 注册一个SignalR客户端中的所有方法，
    /// 使Hub中心可以调用它
    /// </summary>
    /// <typeparam name="Interface">待注册客户端方法的接口</typeparam>
    /// <param name="connection">要注册客户端方法的Hub连接</param>
    /// <param name="instance">接口的实例，函数会从这个实例调用它的方法</param>
    /// <returns>一个集合，它的元素是一个元组，
    /// 第一个项是一个可以用来取消注册的<see cref="IDisposable"/>，
    /// 第二个项它所对应的方法</returns>
    public static IReadOnlyCollection<(IDisposable Disposable, MethodInfo Method)> RegisterSignalRClient<Interface>(HubConnection connection, Interface instance)
        where Interface : class
    {
        #region 用来注册的本地函数
        IEnumerable<(IDisposable Disposable, MethodInfo Method)> Register()
        {
            #region 用来获取封闭泛型类型的本地函数
            Type GetCloseGenericType()
            {
                var type = typeof(Interface);
                return type.ContainsGenericParameters ?
                    instance.GetType().GetInterfaces().Single(x => x.IsGenericRealize(type)) :
                    type;
            }
            #endregion
            var methods = GetCloseGenericType().GetMethodInfoRecursion(BindingFlags.Public | BindingFlags.Instance);
            var onMethods = typeof(HubConnectionExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static).
                Where(x => x.Name is nameof(HubConnectionExtensions.On) && x.GetParameters().Length is 3).ToArray();
            foreach (var method in methods)
            {
                var name = method.Name;
                #region 获取方法和委托类型的方法
                (MethodInfo Method, Type DelegateType) Fun()
                {
                    var parameters = method.GetParameters();
                    var parameterTypes = parameters.Select(x => x.ParameterType).ToArray();
                    var returnType = method.ReturnType;
                    var isVoid = returnType == typeof(void);
                    var (isAsyncReturnType, asyncReturnType) = returnType.GetAsyncInfo();
                    var finalReturnType = asyncReturnType ?? (isAsyncReturnType || isVoid ? null : returnType);
                    var genericParameters = finalReturnType is null ? parameterTypes : [.. parameterTypes, finalReturnType];
                    var invokeMethod = onMethods.Single(x =>
                    {
                        var genericArguments = x.GetGenericArguments();
                        if (genericArguments.Length != genericParameters.Length)
                            return false;
                        var delegateParameterMethod = x.GetParameterTypes()[^1].GetDelegateMethod();
                        var delegateParameterMethodReturnParameter = delegateParameterMethod.ReturnParameter.ParameterType;
                        return (isAsyncReturnType, finalReturnType) switch
                        {
                            (false, null) => delegateParameterMethodReturnParameter == typeof(void),
                            (false, { }) => delegateParameterMethodReturnParameter == genericArguments[^1],
                            (true, null) => delegateParameterMethodReturnParameter == typeof(Task),
                            (true, { }) => delegateParameterMethodReturnParameter == typeof(Task<>).MakeGenericType(genericArguments[^1])
                        };
                    });
                    var makeMethod = invokeMethod.MakeGenericMethod(genericParameters);
                    var delegateType = makeMethod.GetParameterTypes()[^1];
                    return (makeMethod, delegateType);
                }
                #endregion
                var (invokeMethod, delegateType) = Fun();
                var @delegate = method.CreateDelegate(delegateType, instance);
                var disposable = invokeMethod.Invoke<IDisposable>(null, connection, name, @delegate);
                yield return (disposable, method);
            }
        }
        #endregion
        return Register().ToArray();
    }
    #endregion
    #region 返回用来注册客户端的高阶函数
    /// <summary>
    /// 返回一个高阶函数，
    /// 它可以注册一个SignalR客户端中的所有方法，
    /// 使Hub中心可以调用它，
    /// 这个函数一般用于配置<see cref="HubConnection"/>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="RegisterSignalRClient{Interface}(HubConnection, Interface)"/>
    public static Func<HubConnection, Task> ConnectionSignalRClient<Interface>(Interface instance)
        where Interface : class
        => connection =>
        {
            RegisterSignalRClient(connection, instance);
            return Task.CompletedTask;
        };
    #endregion
}
