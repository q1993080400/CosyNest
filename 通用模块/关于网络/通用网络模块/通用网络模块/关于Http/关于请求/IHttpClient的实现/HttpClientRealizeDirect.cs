namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IHttpClient"/>的实现，
/// 可以直接通过Http客户端发起Http请求
/// </summary>
/// <param name="httpClient">指定的Http客户端，
/// 本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="HttpClientBase(HttpRequestTransform?)"/>
sealed class HttpClientRealizeDirect(HttpClient httpClient, HttpRequestTransform? defaultTransform) : HttpClientBase(defaultTransform)
{
    #region 抽象成员实现
    #region 获取Http客户端
    protected override HttpClient HttpClient
        => httpClient;
    #endregion
    #endregion
}
