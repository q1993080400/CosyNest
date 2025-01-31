using System.Linq.Expressions;

namespace System.NetFrancis;

/// <summary>
/// 这个类型是<see cref="IStrongTypeInvoke{API}"/>的实现，
/// 它实际上不发起Http请求，
/// 而是通过请求一个内部的服务来实现远程调用
/// </summary>
/// <param name="inlineInvokeServiceFactory">用来提供内联服务的工厂</param>
/// <inheritdoc cref="IStrongTypeInvoke{API}"/>
sealed class InlineStrongTypeInvoke<API>(InlineInvokeServiceFactory<API> inlineInvokeServiceFactory) : IStrongTypeInvoke<API>
    where API : class
{
    #region 公开成员
    #region 无返回值调用
    public async Task Invoke(Expression<Func<API, Task>> invoke, CancellationToken cancellationToken = default)
    {
        var @interface = await inlineInvokeServiceFactory();
        await InvokeServer(invoke, @interface);
    }
    #endregion
    #region 有返回值调用
    public async Task<Ret> Invoke<Ret>(Expression<Func<API, Task<Ret>>> invoke, CancellationToken cancellationToken = default)
    {
        var @interface = await inlineInvokeServiceFactory();
        return await InvokeServer(invoke, @interface);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 调用服务
    /// <summary>
    /// 通过内联服务进行强类型API调用
    /// </summary>
    /// <typeparam name="Return">服务的返回值类型</typeparam>
    /// <param name="invoke">描述调用过程的表达式</param>
    /// <param name="interface">API接口的实例</param>
    /// <returns></returns>
    private static Return InvokeServer<Return>(Expression<Func<API, Return>> invoke, API @interface)
        => invoke.Compile()(@interface);
    #endregion
    #endregion
}
