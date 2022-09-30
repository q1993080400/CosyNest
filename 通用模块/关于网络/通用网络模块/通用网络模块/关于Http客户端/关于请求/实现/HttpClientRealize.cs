using System.IOFrancis.Bit;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IHttpClient"/>的实现，
/// 可以用来发起Http请求
/// </summary>
sealed class HttpClientRealize : IHttpClient
{
    #region 公开成员
    #region 发起Http请求
    #region 返回IHttpResponse
    public async Task<IHttpResponse> Request(IHttpRequest request, CancellationToken cancellationToken = default)
    {
        #region 本地函数
        async Task<IHttpResponse> Fun(IHttpRequest request)      //解决重定向问题
        {
            using var r = request.ToHttpRequestMessage(HttpClient.BaseAddress);
            using var response = await HttpClient.SendAsync(r, cancellationToken);
            return response.StatusCode is HttpStatusCode.Found ?
               await Fun(new HttpRequestRecording(response.Headers.Location!.AbsoluteUri)) :
                new HttpResponse(response);
        }
        #endregion
        var response = await Fun(request);
        if (ThrowException)
            response.ThrowIfNotSuccess();
        return response;
    }
    #endregion
    #region 返回IBitRead
    public async Task<IBitRead> RequestDownload(IHttpRequest request, CancellationToken cancellationToken = default)
    {
        var uri = request.UriComplete;
        var hasHeader = request.Header.Headers().Count > 0 || request.HttpMethod != HttpMethod.Get;     //如果没有携带标头，且不是通过Get请求
        var httpClient = HttpClient;                            //则使用默认的Http客户端
        if (hasHeader)                                          //否则创建一个新的，具有自定义标头的客户端
        {
            httpClient = new();
            request.Header.CopyHeader(httpClient.DefaultRequestHeaders);
        }
        try
        {
            return (await httpClient.GetStreamAsync(uri, cancellationToken)).ToBitPipe().Read;
        }
        catch (HttpRequestException ex) when (ex is { StatusCode: HttpStatusCode.Found })       //解决重定向问题
        {
            var newUri = (await httpClient.GetAsync(uri, cancellationToken)).Headers.Location;
            return (await httpClient.GetStreamAsync(newUri, cancellationToken)).ToBitPipe().Read;
        }
        finally
        {
            if (hasHeader)                      //如果Http客户端具有标头，是新创建的，还会将其释放
                httpClient.Dispose();
        }
    }

    /*说明文档
      问：为什么当具有自定义标头时，要重新创建一个HttpClient？
      答：因为HttpClient有关下载的API只能传入uri，它不支持对标头的自定义，
      因此作者使用了一个变通方法，创建了一个新的HttpClient，并且设置了它的默认标头，
      创建新标头的目的在于，防止对原有的HttpClient的干扰
    
      但是，它也带来了一些隐患，如果大量地执行具有自定义标头的下载，
      会导致HttpClient堆栈耗尽，请尽量避免*/
    #endregion
    #endregion
    #region 发起强类型请求
    #region 正式方法
    public Task<IHttpResponse> Request<API>(Expression<Func<API, object?>> request, CancellationToken cancellationToken = default)
        where API : class
    {
        if (request is not
            {
                Body: MethodCallExpression
                {
                    Object: ParameterExpression
                    {
                        Type: { } interfaceType
                    },
                    Arguments: { } arguments,
                    Method: { } method,
                }
            })
            throw new NotSupportedException($"表达式中只允许包含直接对{typeof(API)}方法的调用");
        var requestPack = CreateRequest(interfaceType, arguments, method);
        return Request(requestPack, cancellationToken);
    }
    #endregion
    #region 生成HTTP请求
    /// <summary>
    /// 辅助方法：根据表达式，
    /// 生成HTTP请求对象
    /// </summary>
    /// <param name="interfaceType">接口的类型，
    /// 该接口会被映射到控制器</param>
    /// <param name="arguments">调用API传入的方法参数</param>
    /// <param name="method">调用的API方法，
    /// 他会被映射到一个控制器操作</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static IHttpRequest CreateRequest(Type interfaceType, IEnumerable<Expression> arguments, MethodInfo method)
    {
        var callPath = CreateCallPath(interfaceType, method);
        var parameter = method.GetParameters().Zip(arguments,
            (p, e) => (p.Name!, e.CalValue()?.ToString()));
        return new HttpRequestRecording(new(callPath)
        {
            UriParameters = parameter.ToDictionary(true)
        });
    }
    #endregion
    #region 生成调用路径
    /// <summary>
    /// 生成对WebApi的调用路径
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CreateRequest"/>
    private static string CreateCallPath(Type interfaceType, MethodInfo method)
    {
        if (method.GetCustomAttribute<RouteDescriptionAttribute>() is { Template: { } methodTemplate })
            return methodTemplate;
        if (!(interfaceType.GetCustomAttribute<RouteDescriptionAttribute>() is { Template: { } interfaceTemplate }))
            throw new NotSupportedException($"在接口和接口的方法上都没有找到{nameof(RouteDescriptionAttribute)}特性，无法确定API路由");
        return interfaceTemplate.Replace("[controller]", interfaceType.Name.TrimStart('I').Trim(false, "Controller")).
            Replace("[action]", method.Name);
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region HttpClient对象
    /// <summary>
    /// 获取封装的<see cref="Net.Http.HttpClient"/>对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private HttpClient HttpClient { get; }
    #endregion
    #region 如果请求失败，是否抛出异常
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则在请求失败时，自动抛出异常
    /// </summary>
    private bool ThrowException { get; set; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的Http客户端初始化对象
    /// </summary>
    /// <param name="httpClient">指定的Http客户端，
    /// 本对象的功能就是通过它实现的</param>
    /// <param name="throwException">如果这个值为<see langword="true"/>，
    /// 则在请求失败时，自动抛出异常</param>
    public HttpClientRealize(HttpClient httpClient, bool throwException)
    {
        HttpClient = httpClient;
        this.ThrowException = throwException;
    }
    #endregion
}
