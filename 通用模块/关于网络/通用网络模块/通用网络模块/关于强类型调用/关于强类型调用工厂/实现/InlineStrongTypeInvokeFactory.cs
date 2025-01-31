using Microsoft.Extensions.DependencyInjection;

namespace System.NetFrancis;

/// <summary>
/// 这个类型是<see cref="IStrongTypeInvokeFactory"/>的实现，
/// 它是一个工厂，可以用来创建强类型调用对象，
/// 该对象实际上不发起Http请求，而是通过请求一个内部的服务来实现远程调用
/// </summary>
/// <param name="serviceProvider">一个用来请求服务的对象</param>
sealed class InlineStrongTypeInvokeFactory(IServiceProvider serviceProvider) : IStrongTypeInvokeFactory
{
    #region 创建强类型调用
    public IStrongTypeInvoke<API> StrongType<API>()
        where API : class
    {
        var inlineInvokeServiceFactory = serviceProvider.GetRequiredService<InlineInvokeServiceFactory<API>>();
        return CreateNet.InlineStrongTypeInvoke(inlineInvokeServiceFactory);
    }
    #endregion
}
