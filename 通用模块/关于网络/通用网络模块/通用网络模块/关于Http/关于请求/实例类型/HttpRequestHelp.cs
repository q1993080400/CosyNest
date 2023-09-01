namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型封装了一个Http转换函数和<see cref="IHttpClient"/>对象
/// 它可以作为服务被注入到容器中
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="httpClient">封装的Http客户端对象，本类型使用它执行请求</param>
public sealed class HttpRequestHelp(IHttpClient httpClient)
{
    #region 公开成员
    #region Http转换函数
    /// <summary>
    /// 获取Http转换函数，
    /// 它可以用于改变Http请求的默认值
    /// </summary>
    public Func<HttpRequestRecording, HttpRequestRecording>? Transformation { get; set; }
    #endregion
    #region 执行请求
    /// <summary>
    /// 执行Http请求，并返回结果
    /// </summary>
    /// <typeparam name="Obj">Http请求的返回值</typeparam>
    /// <param name="request">用来执行Http请求的委托，
    /// 它的第一个参数是发起请求的<see cref="IHttpClient"/>对象，
    /// 第二个参数是一个Http转换函数，它可以用于改变Http请求的默认值</param>
    /// <returns></returns>
    public Obj Request<Obj>(Func<IHttpClient, Func<HttpRequestRecording, HttpRequestRecording>?, Obj> request)
        => request(HttpClient, Transformation);
    #endregion
    #endregion
    #region 内部成员
    #region Http客户端对象
    /// <summary>
    /// 获取封装的Http客户端对象，
    /// 本类型使用它执行请求
    /// </summary>
    private IHttpClient HttpClient { get; } = httpClient;

    #endregion
    #endregion
}
