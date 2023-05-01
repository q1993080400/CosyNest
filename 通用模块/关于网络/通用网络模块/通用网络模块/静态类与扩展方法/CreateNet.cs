using System.Net.Http.Json;
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
        => ServiceProvider.GetRequiredService<IHttpClientFactory>().
        CreateClient().ToHttpClient();
    #endregion
    #region 有关IHttpContent
    #region 使用Json创建
    /// <summary>
    /// 将指定对象序列化，
    /// 然后创建一个包含Json的<see cref="HttpContentRecording"/>
    /// </summary>
    /// <typeparam name="Obj">要序列化的对象的类型</typeparam>
    /// <param name="obj">要序列化的对象</param>
    /// <param name="options">控制序列化过程的选项</param>
    /// <returns></returns>
    public static Task<HttpContentRecording> HttpContentJson<Obj>(Obj? obj, JsonSerializerOptions? options = null)
        => JsonContent.Create(obj, options: options).ToHttpContent()!;
    #endregion
    #endregion
    #region 有关创建ISignalRProvide
    #region 直接创建ISignalRProvide
    /// <summary>
    /// 创建一个<see cref="ISignalRProvide"/>，
    /// 它可以用来提供SignalR连接
    /// </summary>
    /// <inheritdoc cref="SignalRProvide.SignalRProvide(Func{string, Task{HubConnection}}, Func{string, string}?)"/>
    public static ISignalRProvide SignalRProvide(Func<string, Task<HubConnection>>? create = null, Func<string, string>? toAbs = null)
        => new SignalRProvide(create ?? ConfigureSignalRProvide(), toAbs);
    #endregion
    #region 创建ISignalRProvide的工厂
    /// <summary>
    /// 创建一个根据中心的绝对Uri返回<see cref="HubConnection"/>的工厂
    /// </summary>
    /// <param name="configure">这个委托被用于进一步配置<see cref="IHubConnectionBuilder"/>，
    /// 它的参数是预配置的<see cref="IHubConnectionBuilder"/>，返回值是<see cref="IHubConnectionBuilder"/>的最终成品，
    /// 如果为<see langword="null"/>，表示直接使用预配置版本</param>
    /// <returns></returns>
    public static Func<string, Task<HubConnection>> ConfigureSignalRProvide(Func<IHubConnectionBuilder, Task<IHubConnectionBuilder>>? configure = null)
        => async (uri) =>
        {
            var build = new HubConnectionBuilder().
             WithUrl(uri).
             AddJsonProtocol(x => x.AddFormatterJson());
            return (configure is null ? build : await configure(build)).Build();
        };
    #endregion
    #endregion
    #region 创建IUriManager
    /// <summary>
    /// 创建一个<see cref="IUriManager"/>对象，
    /// 它可以用来管理本机Uri
    /// </summary>
    /// <param name="host">本机完整绝对Uri</param>
    /// <returns></returns>
    public static IUriManager UriManager(string host)
        => new UriManager()
        {
            Uri = host
        };
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
