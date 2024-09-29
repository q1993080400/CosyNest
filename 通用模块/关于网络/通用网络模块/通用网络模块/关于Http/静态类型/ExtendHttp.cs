using System.Net.Http.Json;
using System.NetFrancis.Http;
using System.Text.Json;

namespace System;

/// <summary>
/// 有关Http的扩展方法全部放在这里
/// </summary>
public static class ExtendHttp
{
    #region 将IHttpClientFactory转换为IHttpClient
    /// <summary>
    /// 返回一个<see cref="IHttpClientFactory"/>的<see cref="IHttpClient"/>包装
    /// </summary>
    /// <param name="httpClientFactory">待包装的<see cref="IHttpClientFactory"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="HttpClientRealize(IHttpClientFactory, HttpRequestTransform?)"/>
    public static IHttpClient ToHttpClient(this IHttpClientFactory httpClientFactory, HttpRequestTransform? defaultTransform = null)
        => new HttpClientRealize(httpClientFactory, defaultTransform);
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
}
