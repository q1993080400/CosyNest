using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System.NetFrancis;

public static partial class CreateNet
{
    //这个部分类专门用来声明用来创建有关SignalR的对象的方法

    #region 创建ISignalRProvide
    /// <summary>
    /// 创建一个<see cref="ISignalRProvide"/>，
    /// 它可以用来提供SignalR连接
    /// </summary>
    /// <inheritdoc cref="SignalRProvide.SignalRProvide(Func{string, Task{IHubConnectionBuilder}}, Func{string, string}?)"/>
    public static ISignalRProvide SignalRProvide(Func<string, Task<IHubConnectionBuilder>>? create = null, Func<string, string>? toAbs = null)
        => new SignalRProvide(create ??= static uri =>
        {
            var builder = new HubConnectionBuilder();
            ConfigureHubConnectionBuilder(builder, uri);
            return Task.FromResult<IHubConnectionBuilder>(builder);
        }, toAbs ??= static x => x);
    #endregion
    #region 配置IHubConnectionBuilder的默认方法
    /// <summary>
    /// 对一个<see cref="IHubConnectionBuilder"/>进行基本配置
    /// </summary>
    /// <param name="builder">要进行配置的<see cref="IHubConnectionBuilder"/></param>
    /// <param name="uri">Hub中心的绝对Uri</param>
    /// <returns></returns>
    public static void ConfigureHubConnectionBuilder(IHubConnectionBuilder builder, string uri)
    {
        builder.WithUrl(uri, static op =>
        {
            op.UseStatefulReconnect = true;
        }).
        AddMessagePackProtocol().
        WithAutomaticReconnect();
    }
    #endregion
}
