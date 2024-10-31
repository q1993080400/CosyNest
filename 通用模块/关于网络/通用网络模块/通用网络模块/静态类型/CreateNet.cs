using System.NetFrancis.Http;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System.NetFrancis;

/// <summary>
/// 这个静态类可以用来创建和网络相关的对象
/// </summary>
public static class CreateNet
{
    #region 有关创建ISignalRProvide
    #region 创建ISignalRProvide
    /// <summary>
    /// 创建一个<see cref="ISignalRProvide"/>，
    /// 它可以用来提供SignalR连接
    /// </summary>
    /// <inheritdoc cref="SignalRProvide.SignalRProvide(Func{string, Task{IHubConnectionBuilder}}, Func{string, string}?)"/>
    public static ISignalRProvide SignalRProvide(Func<string, Task<IHubConnectionBuilder>>? create = null, Func<string, string>? toAbs = null)
        => new SignalRProvide(create ??= uri =>
        {
            var builder = new HubConnectionBuilder();
            ConfigureHubConnectionBuilder(builder, uri);
            return Task.FromResult<IHubConnectionBuilder>(builder);
        }, toAbs ??= x => x);
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
        builder.WithUrl(uri, op =>
        {
            op.UseStatefulReconnect = true;
        }).
        AddMessagePackProtocol().
        WithAutomaticReconnect();
    }
    #endregion
    #endregion
    #region 创建IUriManager
    /// <summary>
    /// 创建一个<see cref="IHostProvide"/>对象，
    /// 它可以用来管理本机Uri
    /// </summary>
    /// <param name="host">本机的Host地址</param>
    /// <returns></returns>
    public static IHostProvide HostProvide(string host)
        => new HostProvide()
        {
            Host = host
        };
    #endregion
    #region 创建HttpRequestTransform
    #region 添加一个基路径
    /// <summary>
    /// 创建一个<see cref="HttpRequestTransform"/>，
    /// 如果当前请求没有指定基路径，
    /// 它可以自动为其添加一个
    /// </summary>
    /// <param name="baseUri">请求的基路径，它一般是应用的Host</param>
    /// <returns></returns>
    public static HttpRequestTransform TransformBaseUri(string baseUri)
        => (request) =>
        {
            var uri = request.Uri;
            return uri.UriHost is { } ?
            request :
            request with
            {
                Uri = uri with
                {
                    UriHost = baseUri
                }
            };
        };
    #endregion
    #endregion
}
