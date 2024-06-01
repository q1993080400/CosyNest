using Microsoft.AspNetCore.SignalR;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关依赖注入的扩展方法

    #region 注入SignalR中心服务，且使用MessagePack协议
    /// <summary>
    /// 注入SignalR中心服务，且使用MessagePack协议，
    /// 由于本框架的SignalR客户端只支持MessagePack协议，
    /// 请务必使用本方法来注入SignalR中心
    /// </summary>
    /// <param name="serviceCollection">服务容器</param>
    /// <returns></returns>
    public static ISignalRServerBuilder AddSignalRWithMessagePackProtocol(this IServiceCollection serviceCollection)
        => serviceCollection.AddSignalR().AddMessagePackProtocol();
    #endregion
}
