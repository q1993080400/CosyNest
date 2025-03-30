using System.Net.Http.Json;
using System.NetFrancis;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendNet
{
    //这个部分类专门声明有关Http的扩展方法

    #region 公开成员
    #region 关于依赖注入
    #region 注入IHttpClient
    #region 直接注入
    /// <summary>
    /// 以范围模式注入一个<see cref="IHttpClient"/>，
    /// 它可以用来发起Http请求，
    /// 如果服务容器中注册了<see cref="HttpRequestTransform"/>，
    /// 那么还会将它作为<see cref="IHttpClient"/>的默认请求转换函数
    /// </summary>
    /// <param name="services">要注入的服务集合</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClient(this IServiceCollection services)
        => services.AddScoped(serviceProvider =>
        {
            var requestTransform = serviceProvider.GetService<HttpRequestTransform>();
            return serviceProvider.GetRequiredService<IHttpClientFactory>().ToHttpClient(requestTransform);
        });
    #endregion
    #region 适用于非浏览器应用
    /// <summary>
    /// 以范围模式注入一个<see cref="IHttpClient"/>，
    /// 它可以用来发起Http请求，
    /// 这个方法不会携带Cookie，它不适合浏览器应用
    /// </summary>
    /// <param name="services">要注入的服务集合</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClientCommon(this IServiceCollection services)
    {
        services.AddHttpClient(CreateNet.HttpClientName);
        services.AddIHttpClient();
        return services;
    }
    #endregion
    #endregion
    #region 注入HttpRequestTransform
    /// <summary>
    /// 以范围模式注入一个<see cref="HttpRequestTransform"/>，
    /// 它可以自动处理相对请求路径，将其视为请求本站，
    /// 本服务依赖于<see cref="IHostProvide"/>
    /// </summary>
    /// <param name="services">要注入的服务集合</param>
    /// <returns></returns>
    public static IServiceCollection AddHttpRequestTransformUri(this IServiceCollection services)
        => services.AddScoped(static x =>
        {
            var hostProvide = x.GetRequiredService<IHostProvide>();
            return CreateNet.TransformBaseUri(hostProvide.Host);
        });
    #endregion
    #endregion
    #region 关于IHttpClient
    #region 从HttpClient转换
    /// <summary>
    /// 返回一个<see cref="IHttpClient"/>，
    /// 它直接封装了一个<see cref="HttpClient"/>，
    /// 除非出于测试用途，不方便使用依赖注入，
    /// 否则不要使用它
    /// </summary>
    /// <param name="httpClient">待包装的Http客户端</param>
    /// <returns></returns>
    /// <inheritdoc cref="HttpClientRealizeDirect(HttpClient, HttpRequestTransform)"/>
    public static IHttpClient ToHttpClient(this HttpClient httpClient, HttpRequestTransform? defaultTransform = null)
        => new HttpClientRealizeDirect(httpClient, defaultTransform ?? HttpRequestTransformDefault);
    #endregion
    #region 从IHttpClientFactory转换
    /// <summary>
    /// 返回一个<see cref="IHttpClientFactory"/>的<see cref="IHttpClient"/>包装
    /// </summary>
    /// <param name="httpClientFactory">待包装的<see cref="IHttpClientFactory"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="HttpClientRealize(IHttpClientFactory, HttpRequestTransform)"/>
    public static IHttpClient ToHttpClient(this IHttpClientFactory httpClientFactory, HttpRequestTransform? defaultTransform = null)
        => new HttpClientRealize(httpClientFactory, defaultTransform ?? HttpRequestTransformDefault);
    #endregion
    #endregion 
    #region 将HttpContent序列化为Json或String
    /// <summary>
    /// 将<see cref="HttpContent"/>转换为对象或文本形式
    /// </summary>
    /// <typeparam name="Ret">返回值类型，
    /// 如果它是<see cref="string"/>，则返回<see cref="HttpContent"/>的文本形式，
    /// 否则将其反序列化成对象</typeparam>
    /// <param name="content">要转换的<see cref="HttpContent"/></param>
    /// <param name="options">用于反序列化的配置选项</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    public static async Task<Ret> ReadFromJsonOrStringAsync<Ret>(this HttpContent content,
        JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (typeof(Ret) == typeof(string))
        {
            var text = await content.ReadAsStringAsync(cancellationToken);
            return text.To<Ret>();
        }
        return (await content.ReadFromJsonAsync<Ret>(options, cancellationToken))!;
    }
    #endregion
    #endregion
    #region 内部成员
    #region HttpRequestTransform委托的默认值
    /// <summary>
    /// 这个方法是<see cref="HttpRequestTransform"/>委托的默认值，
    /// 它不做任何转换
    /// </summary>
    /// <inheritdoc cref="HttpRequestTransform"/>
    private static Task<HttpRequestRecording> HttpRequestTransformDefault(HttpRequestRecording old)
        => Task.FromResult(old);
    #endregion
    #endregion
}
