using System.IOFrancis.Bit;
using System.Net;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IHttpClient"/>的实现，
/// 可以用来发起Http请求
/// </summary>
/// <param name="httpClientFactory">指定的Http工厂，
/// 本对象的功能就是通过它实现的</param>
/// <param name="defaultTransform">用来转换Http请求的函数，
/// 它可以用来改变Http请求的默认值，并具有较低的优先级，
/// 如果为<see langword="null"/>，则不做转换</param>
sealed class HttpClientRealize(IHttpClientFactory httpClientFactory, HttpRequestTransform? defaultTransform) : IHttpClient
{
    #region 接口实现
    #region 发起Http请求
    #region 返回HttpResponseMessage
    public async Task<HttpResponseMessage> Request(HttpRequestRecording request, HttpRequestTransform? transformation = null, CancellationToken cancellationToken = default)
    {
        #region 本地函数
        async Task<HttpResponseMessage> Fun(HttpRequestRecording request)      //解决重定向问题
        {
            var requestMiddle = defaultTransform?.Invoke(request) ?? request;
            var transformationRequest = transformation?.Invoke(requestMiddle) ?? requestMiddle;
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
        return await Fun(request);
    }
    #endregion
    #region 返回IBitRead
    public async Task<IBitRead> RequestBitRead(string uri, CancellationToken cancellationToken = default)
    {
        var stream = await HttpClient.GetStreamAsync(uri, cancellationToken);
        return stream.ToBitPipe().Read;
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
    #region Http客户端
    /// <summary>
    /// 获取Http客户端，
    /// 它可以用于发起请求
    /// </summary>
    private HttpClient HttpClient
        => httpClientFactory.CreateClient();
    #endregion
    #endregion
}
