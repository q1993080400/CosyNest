using System.Linq.Expressions;
using System.Net.Http.Json;
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
    #region 发起强类型请求
    #region 正式方法
    public async Task<HttpResponseMessage> RequestResponse<Ret>
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
        var methodParameters = method.GetParameters();
        var hasUpload = methodParameters.Select(x => x.ParameterType).Any(IsUpload);
        var isGet = (hasUpload, method.GetCustomAttribute<HttpMethodPostAttribute>()) is (false, null);
        if (isGet)
        {
            var parameter = methodParameters.Zip(arguments,
            (p, e) => (p.Name!, e.CalValue()?.ToString()!)).
            Where(x => x.Item2 is { });
            return CreateRequestGet(callPath, parameter);
        }
        return hasUpload ?
            CreateRequestPostHasUpload(callPath, arguments, options) :
            CreateRequestPost(callPath, arguments, options);
    }
    #endregion
    #region 判断一个参数类型是否为上传文件
    /// <summary>
    /// 判断一个参数类型是否为上传文件
    /// </summary>
    /// <param name="parameterType">待判断的参数类型</param>
    /// <returns></returns>
    private static bool IsUpload(Type parameterType)
        => parameterType == typeof(UploadFile) || parameterType == typeof(IEnumerable<UploadFile>);
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
        => new()
        {
            Uri = new(callPath)
            {
                UriParameter = new(parameter!)
            }
        };
    #endregion
    #region 生成Post请求
    /// <summary>
    /// 生成一个Post请求的请求体
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CreateRequestGet(string, IEnumerable{ValueTuple{string,string}})"/>
    /// <inheritdoc cref="CreateRequest(Type, IEnumerable{Expression}, MethodInfo, JsonSerializerOptions?)"/>
    private static HttpRequestRecording CreateRequestPost
        (string callPath, IEnumerable<Expression> parameter, JsonSerializerOptions? options)
    {
        var single = parameter.SingleOrDefaultSecure() ?? throw new NotSupportedException($"使用强类型模式调用Post方法的Http接口时，" +
                $"参数数量不能大于一个");
        var value = single.CalValue();
        var content = JsonContent.Create(value, value?.GetType() ?? single.Type, null, options);
        return new()
        {
            Uri = new(callPath),
            HttpMethod = HttpMethod.Post,
            Content = content
        };
    }
    #endregion
    #region 生成包含上传文件的Post请求
    /// <summary>
    /// 生成一个包含上传文件的Post请求的请求体
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CreateRequestPost(string, IEnumerable{Expression}, JsonSerializerOptions?)"/>
    private static HttpRequestRecording CreateRequestPostHasUpload
        (string callPath, IEnumerable<Expression> parameter, JsonSerializerOptions? options)
    {
        var (uploadParameter, notUploadParameter) = parameter.Split(x => IsUpload(x.Type));
        if ((uploadParameter.Count, notUploadParameter.Count) is not (1, <= 1))
            throw new NotSupportedException($"强类型Http上传请求中，上传文件参数和普通对象参数都不能多于一个");
        using var content = new MultipartFormDataContent();
        throw new NotSupportedException("未实现这个功能");
    }
    #endregion
    #endregion
    #endregion
}
