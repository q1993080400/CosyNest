using System.Net;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IHttpClient"/>的实现，
/// 可以用来发起Http请求
/// </summary>
/// <param name="defaultTransform">用来转换Http请求的默认函数，
/// 它可以用来改变Http请求的默认值，并作为管道的第一个输入，
/// 如果为<see langword="null"/>，则不进行转换</param>
abstract class HttpClientBase(HttpRequestTransform? defaultTransform) : IHttpClient
{
    #region 接口实现
    #region 发起Http请求
    #region 返回HttpResponseMessage
    public async Task<HttpResponseMessage> Request(HttpRequestRecording request, Func<HttpRequestTransform, HttpRequestTransform>? transformation = null, CancellationToken cancellationToken = default)
    {
        var requestMiddle = transformation is null ? DefaultTransform : transformation(DefaultTransform);
        #region 本地函数
        async Task<HttpResponseMessage> Fun(HttpRequestRecording request)      //解决重定向问题
        {
            var transformationRequest = requestMiddle(request);
            using var bclRequest = transformationRequest.ToHttpRequestMessage();
            var response = await HttpClient.SendAsync(bclRequest, cancellationToken);
            if (response.StatusCode is HttpStatusCode.Found)
            {
                var redirect = response.Headers.Location!.AbsoluteUri;
                response.Dispose();
                return await Fun(request with
                {
                    Uri = redirect
                });
            }
            return response;
        }
        #endregion
        var response = await Fun(request);
        return response;
    }
    #endregion
    #region 返回Stream
    public async Task<Stream> RequestStream(string uri, bool simulateBrowser = false, CancellationToken cancellationToken = default)
    {
        if (!simulateBrowser)
            return await HttpClient.GetStreamAsync(uri, cancellationToken);
        var httpRequest = new HttpRequestRecording()
        {
            Uri = uri,
            Header = new()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36 Edg/129.0.0.0"
            }
        };
        var response = await Request(httpRequest, cancellationToken: cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
    #endregion
    #endregion
    #region 发起强类型请求
    public IHttpStrongTypeRequest<API> StrongType<API>()
        where API : class
        => new HttpStrongTypeRequest<API>(this);
    #endregion
    #endregion
    #region 内部成员
    #region 转换函数
    /// <summary>
    /// 这个函数用来在请求的时候，
    /// 对<see cref="HttpRequestRecording"/>进行转换
    /// </summary>
    private HttpRequestTransform DefaultTransform { get; } = defaultTransform ??= static x => x;
    #endregion
    #endregion
    #region 抽象成员
    #region 获取Http客户端
    /// <summary>
    /// 获取Http客户端，
    /// 它可以用于发起请求
    /// </summary>
    protected abstract HttpClient HttpClient { get; }
    #endregion
    #endregion
}
