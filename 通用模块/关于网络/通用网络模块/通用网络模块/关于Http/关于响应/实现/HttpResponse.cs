using System.Net;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IHttpResponse"/>的实现，
/// 可以视为一个Http请求的响应
/// </summary>
sealed class HttpResponse : IHttpResponse
{
    #region 公开成员
    #region 响应的状态
    public HttpStatusCode Status { get; init; }
    #endregion
    #region 响应的标头
    public IHttpHeaderResponse Header { get; init; }
    #endregion
    #region 响应的正文
    public IHttpContent Content { get; init; }
    #endregion
    #region 如果响应不成功，则抛出异常
    /// <summary>
    /// 如果响应不成功，则抛出异常
    /// </summary>
    public void ThrowIfNotSuccess()
    {
        if (!this.To<IHttpResponse>().IsSuccess)
            throw new HttpRequestException($"""
Http请求未能成功
状态码：{(int)Status}
请求的完整地址：{RequestUri}
""");
    }
    #endregion
    #endregion
    #region 内部成员
    #region 请求的Uri
    /// <summary>
    /// 获取请求的Uri
    /// </summary>
    internal string RequestUri { get; init; }
    #endregion
    #endregion
}
