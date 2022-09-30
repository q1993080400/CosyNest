namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录封装了提交Http请求所需要的信息
/// </summary>
public sealed record HttpRequestRecording : IHttpRequest
{
    #region 公开成员
    #region 静态成员
    #region 创建HttpRequestRecording
    /// <summary>
    /// 通过Uri和参数创建一个<see cref="HttpRequestRecording"/>
    /// </summary>
    /// <param name="uri">请求的Uri</param>
    /// <param name="parameters">这个元组的项分别是参数的名称和值</param>
    /// <returns></returns>
    public static HttpRequestRecording Create(string uri, (string Parameter, string? Value)[]? parameters = null)
        => new(new UriAbsolute(uri)
        {
            UriParameters = parameters?.ToDictionary(true) ?? CreateCollection.EmptyDictionary<string, string?>()
        });
    #endregion
    #endregion
    #region 实例成员
    #region 获取请求的Uri
    string IHttpRequest.UriComplete => Uri.UriComplete;

    private readonly UriAbsolute UriCompleteField;

    /// <summary>
    /// 获取请求的目标Uri
    /// </summary>
    public UriAbsolute Uri
    {
        get => UriCompleteField;
        init
        {
            if (HttpMethod != HttpMethod.Get && value.UriParameters.Any())
                throw new NotSupportedException($"只有Get方法才可以在Uri中设置参数");
            UriCompleteField = value;
        }
    }
    #endregion
    #region 获取请求头
    IHttpHeaderRequest IHttpRequest.Header => Header;

    /// <inheritdoc cref="IHttpRequest.Header"/>
    public HttpHeaderRequest Header { get; init; } = new();
    #endregion
    #region 获取Http方法
    public HttpMethod HttpMethod { get; init; } = HttpMethod.Get;
    #endregion
    #region Http请求内容
    IHttpContent? IHttpRequest.Content => Content;

    /// <inheritdoc cref="IHttpRequest.Content"/>
    public HttpContentRecording? Content { get; init; }
    #endregion
    #endregion 
    #endregion
    #region 构造函数
#pragma warning disable CS8618
    #region 指定Uri
    /// <summary>
    /// 使用指定的Uri初始化对象
    /// </summary>
    /// <param name="uri"></param>
    public HttpRequestRecording(UriAbsolute uri)
    {
        this.Uri = uri;
    }
    #endregion
    #region 无参数构造函数
    public HttpRequestRecording()
    {
        Uri = new();
    }
    #endregion
#pragma warning restore
    #endregion
}
