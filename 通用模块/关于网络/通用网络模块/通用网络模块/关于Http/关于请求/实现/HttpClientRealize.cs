using System.IOFrancis.Bit;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.Json;

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
    public async Task<IHttpResponse> Request(HttpRequestRecording request, Func<HttpRequestRecording, HttpRequestRecording>? transformation = null, CancellationToken cancellationToken = default)
    {
        #region 本地函数
        async Task<IHttpResponse> Fun(HttpRequestRecording request)      //解决重定向问题
        {
            var transformationRequest = transformation?.Invoke(request) ?? request;
            using var r = await transformationRequest.ToHttpRequestMessage(HttpClient.BaseAddress);
            using var response = await HttpClient.SendAsync(r, cancellationToken);
            return response.StatusCode is HttpStatusCode.Found ?
               await Fun(new(response.Headers.Location!.AbsoluteUri)) :
               await response.ToHttpResponse();
        }
        #endregion
        var response = await Fun(request);
        if (ThrowException)
            response.ThrowIfNotSuccess();
        return response;
    }
    #endregion
    #region 返回IBitRead
    public async Task<IBitRead> RequestDownload(HttpRequestRecording request, Func<HttpRequestRecording, HttpRequestRecording>? transformation = null, CancellationToken cancellationToken = default)
    {
        var transformationRequest = transformation?.Invoke(request) ?? request;
        if (transformationRequest.HttpMethod != HttpMethod.Get)
            throw new NotSupportedException($"提供下载的Http请求仅支持Get方法");
        var uri = transformationRequest.Uri;
        using var httpClient = new HttpClient();
        transformationRequest.Header.CopyHeader(httpClient.DefaultRequestHeaders);
        try
        {
            return (await httpClient.GetStreamAsync(uri, cancellationToken)).ToBitPipe().Read;
        }
        catch (HttpRequestException ex) when (ex is { StatusCode: HttpStatusCode.Found })       //解决重定向问题
        {
            var newUri = (await httpClient.GetAsync(uri, cancellationToken)).Headers.Location;
            return (await httpClient.GetStreamAsync(newUri, cancellationToken)).ToBitPipe().Read;
        }
    }

    /*说明文档
      问：为什么这个方法要重新创建一个HttpClient？Net规范不推荐这种做法
      答：因为HttpClient有关下载的API只能传入uri，它不支持对标头的自定义，
      因此作者使用了一个变通方法，创建了一个新的HttpClient，并且设置了它的默认标头，
      创建新标头的目的在于，防止对原有的HttpClient的干扰
    
      但是，它也带来了一些隐患，如果大量地执行下载，
      会导致HttpClient堆栈耗尽，请尽量避免*/
    #endregion
    #endregion
    #region 发起强类型请求
    #region 正式方法
    public async Task<IHttpResponse> Request<API>(Expression<Func<API, object?>> request, Func<HttpRequestRecording, HttpRequestRecording>? transformation = null,
        JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
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
        var requestPack = CreateRequest(interfaceType, arguments, method, options);
        return await Request(requestPack, transformation, cancellationToken);
    }
    #endregion
    #region 辅助方法
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
    /// <param name="options">一个用于转换Json的对象</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static HttpRequestRecording CreateRequest(Type interfaceType, IEnumerable<Expression> arguments, MethodInfo method, JsonSerializerOptions? options = null)
    {
        var callPath = CreateCallPath(interfaceType, method);
        var isGet = method.GetCustomAttribute<HttpMethodPostAttribute>() is null;
        if (isGet)
        {
            var parameter = method.GetParameters().Zip(arguments,
            (p, e) => (p.Name!, e.CalValue()?.ToString()!)).
            Where(x => x.Item2 is { });
            return CreateRequestGet(callPath, parameter);
        }
        return CreateRequestPost(callPath, arguments, options);
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
        var interfaceName = interfaceType.Name.TrimStart('I').Trim(false, "Controller");
        var methodName = method.Name;
        if (interfaceType.GetCustomAttribute<RouteDescriptionAttribute>() is { Template: { } interfaceTemplate })
            return interfaceTemplate.Replace("[controller]", interfaceName).
                Replace("[action]", methodName);
        return $"/api/{interfaceName}/{methodName}";        //如果没有任何特性，则使用默认的调用路径
    }
    #endregion
    #region 生成Get请求
    /// <summary>
    /// 生成一个Get请求的请求体
    /// </summary>
    /// <param name="callPath">控制器调用路径</param>
    /// <param name="parameter">控制器的参数</param>
    /// <returns></returns>
    private static HttpRequestRecording CreateRequestGet(string callPath, IEnumerable<(string, string)> parameter)
        => new(new(callPath)
        {
            UriParameter = new(parameter.ToDictionary(true)!)
        });
    #endregion
    #region 生成Post请求
    /// <summary>
    /// 生成一个Post请求的请求体
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CreateRequestGet(string, IEnumerable{ValueTuple{string,string}})"/>
    /// <inheritdoc cref="CreateRequest(Type, IEnumerable{Expression}, MethodInfo, JsonSerializerOptions?)"/>
    private static HttpRequestRecording CreateRequestPost(string callPath, IEnumerable<Expression> parameter, JsonSerializerOptions? options)
    {
        var single = parameter.SingleOrDefault() ?? throw new NotSupportedException($"使用强类型模式调用Post方法的Http接口时，" +
                $"参数数量不能大于一个");
        var value = single.CalValue();
        var json = JsonSerializer.Serialize(value, single.Type, options);
        var content = new HttpContentRecording()
        {
            Header = new()
            {
                ContentType = new(MediaTypeName.Json)
            },
            Content = json.ToBytes().ToBitRead()
        };
        return new(callPath)
        {
            HttpMethod = HttpMethod.Post,
            Content = content
        };
    }
    #endregion
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
        ThrowException = throwException;
    }
    #endregion
}
