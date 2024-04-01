using System.Diagnostics.CodeAnalysis;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录封装了提交Http请求所需要的信息
/// </summary>
public sealed record HttpRequestRecording
{
    #region 公开成员
    #region 获取请求的Uri
    private readonly UriComplete? UriCompleteField;

    /// <summary>
    /// 获取请求的目标Uri
    /// </summary>
    public required UriComplete Uri
    {
        get => UriCompleteField ?? throw new ArgumentNullException($"未初始化{nameof(UriComplete)}");
        init
        {
            if (HttpMethod != HttpMethod.Get && (value.UriParameter?.Parameter.Any() ?? false))
                throw new NotSupportedException($"只有Get方法才可以在Uri中设置参数");
            UriCompleteField = value;
        }
    }
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
    /// 获取Http请求的内容
    /// </summary>
    public IHttpContent? Content { get; init; }
    #endregion
    #region 如果响应不成功，是否抛出异常
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 且响应不成功，则抛出异常
    /// </summary>
    public bool ThrowIfNotSuccess { get; set; }
    #endregion
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpRequestRecording()
    {

    }
    #endregion
    #region 指定Uri
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="uri">请求的目标Uri</param>
    [SetsRequiredMembers]
    public HttpRequestRecording(UriComplete uri)
    {
        Uri = uri;
    }
    #endregion
    #region 指定Uri和参数
    /// <summary>
    /// 使用指定的Uri和参数初始化对象
    /// </summary>
    /// <param name="uri">指定的Uri</param>
    /// <param name="parameters">Uri的参数，如果它为<see langword="null"/>，
    /// 且<paramref name="uri"/>已经具有参数，则不会改变<paramref name="uri"/>的参数</param>
    [SetsRequiredMembers]
    public HttpRequestRecording(string uri, (string Parameter, string? Value)[]? parameters)
    {
        var uriComplete = new UriComplete(uri);
        Uri = parameters.AnyAndNotNull() ?
            uriComplete with
            {
                UriParameter = new(parameters)
            } :
            uriComplete;
    }
    #endregion
    #endregion
}
