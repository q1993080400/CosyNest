namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录封装了提交Http请求所需要的信息
/// </summary>
public sealed record HttpRequestRecording
{
    #region 获取请求的Uri
    /// <summary>
    /// 获取请求的目标Uri
    /// </summary>
    public required UriComplete Uri { get; init; }
    #endregion
    #region 获取请求头
    /// <summary>
    /// 获取Http请求头
    /// </summary>
    public HttpHeaderRequest Header { get; init; } = new();
    #endregion
    #region 获取Http方法
    /// <summary>
    /// 获取请求的Http方法
    /// </summary>
    public HttpMethod HttpMethod { get; init; } = HttpMethod.Get;
    #endregion
    #region Http请求内容
    /// <summary>
    /// 获取Http请求的内容，
    /// 如果为<see langword="null"/>，
    /// 视为没有任何内容，仅使用查询参数
    /// </summary>
    public HttpContent? Content { get; init; }
    #endregion
    #region 如果响应不成功，是否抛出异常
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 且响应不成功，则抛出异常
    /// </summary>
    public bool ThrowIfNotSuccess { get; init; }
    #endregion
    #region 转换为HttpRequestMessage
    /// <summary>
    /// 将本对象转换为等效的<see cref="HttpRequestMessage"/>
    /// </summary>
    /// <returns></returns>
    public HttpRequestMessage ToHttpRequestMessage()
    {
        var requestMessage = new HttpRequestMessage()
        {
            RequestUri = new(Uri),
            Method = HttpMethod,
            Content = Content,
        };
        Header.CopyHeader(requestMessage.Headers);
        return requestMessage;
    }
    #endregion
}
