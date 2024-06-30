using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IHttpStrongTypeRequest{API}"/>的实现，
/// 可以用来发起强类型请求
/// </summary>
/// <param name="httpClient">封装的<see cref="IHttpClient"/>对象，
/// 本对象通过它发起请求</param>
/// <inheritdoc cref="IHttpStrongTypeRequest{API}"/>
sealed class HttpStrongTypeRequest<API>(IHttpClient httpClient) : IHttpStrongTypeRequest<API>
    where API : class
{
    #region 接口实现
    #region 发起强类型请求
    #region 正式方法
    public async Task<IHttpResponse> RequestResponse<Ret>
        (Expression<Func<API, Ret>> request, HttpRequestTransform? transformation = null,
        JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
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
        return await httpClient.Request(requestPack, transformation, cancellationToken);
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
        var single = parameter.SingleOrDefaultSecure() ?? throw new NotSupportedException($"使用强类型模式调用Post方法的Http接口时，" +
                $"参数数量不能大于一个");
        var value = single.CalValue();
        var json = JsonSerializer.Serialize(value, single.Type, options);
        var content = new HttpContentRecording()
        {
            Header = new()
            {
                ContentType = new(MediaTypeName.TextJson)
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
}
