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
    public HttpStatusCode Status { get; }
    #endregion
    #region 响应的标头
    public IHttpHeaderResponse Header { get; }
    #endregion
    #region 响应的正文
    public IHttpContent Content { get; }
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
    private string RequestUri { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的Http消息初始化对象
    /// </summary>
    /// <param name="message">指定的Http消息</param>
    /// <param name="requestUri">请求的Uri，它只用来排查错误</param>
    public HttpResponse(HttpResponseMessage message)
    {
        Status = message.StatusCode;
        RequestUri = message.RequestMessage?.RequestUri?.AbsoluteUri ??
            $"这条消息不是通过{nameof(IHttpClient)}发送的，所以无法获取请求地址";
        Header = new HttpHeaderResponse(message.Headers);
        Content = new HttpContentRecording(message.Content);
    }
    #endregion
}
