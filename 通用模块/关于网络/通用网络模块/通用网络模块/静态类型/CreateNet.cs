using System.NetFrancis.Http;
using System.Text.Json;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System.NetFrancis;

/// <summary>
/// 这个静态类可以用来创建和网络相关的对象
/// </summary>
public static class CreateNet
{
    #region 返回公用的IHttpClient对象
    private static IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 返回一个公用的<see cref="IHttpClient"/>对象，
    /// 它在内部通过<see cref="IHttpClientFactory"/>进行池化，可以放心使用
    /// </summary>
    public static IHttpClient HttpClientShared
        => ServiceProvider.GetRequiredService<IHttpClientFactory>().ToHttpClient();
    #endregion
    #region 创建IObjectHeaderValue
    #region 直接封装Json文本
    /// <summary>
    /// 创建一个<see cref="IObjectHeaderValue"/>，
    /// 它在Http标头中封装了一个Json文本
    /// </summary>
    /// <param name="json">要封装的Json文本</param>
    /// <returns></returns>
    public static IObjectHeaderValue ObjectHeaderValue(string json)
        => new ObjectHeaderValue(json);
    #endregion
    #region 封装对象
    /// <summary>
    /// 创建一个<see cref="IObjectHeaderValue"/>，
    /// 它将对象序列化为Json，并封装到Http标头中
    /// </summary>
    /// <typeparam name="Obj">要封装的对象类型</typeparam>
    /// <param name="obj">要封装的对象</param>
    /// <param name="options">用于序列化的配置选项</param>
    /// <returns></returns>
    public static IObjectHeaderValue ObjectHeaderValue<Obj>(Obj obj, JsonSerializerOptions? options = null)
    {
        var json = JsonSerializer.Serialize(obj, options);
        return ObjectHeaderValue(json);
    }
    #endregion 
    #endregion
    #region 有关创建ISignalRProvide
    #region 正式方法
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
    #region 辅助方法：配置IHubConnectionBuilder
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
    #region 静态构造函数
    static CreateNet()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
    #endregion
}
