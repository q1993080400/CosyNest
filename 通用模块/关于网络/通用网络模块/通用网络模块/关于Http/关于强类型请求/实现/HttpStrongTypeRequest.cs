﻿using System.Linq.Expressions;
using System.Net.Http.Headers;
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
    #region 接口实现
    #region 发起强类型请求
    public async Task<HttpResponseMessage> RequestResponse<Ret>
        (Expression<Func<API, Ret>> request, HttpRequestTransform? transformation = null,
        JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (request is not
            {
                Body: MethodCallExpression
                {
                    Object: ParameterExpression,
                    Arguments: { } arguments,
                    Method: { } method,
                }
            })
            throw new NotSupportedException($"表达式中只允许包含直接对{typeof(API)}方法的调用");
        var requestPack = GetRequest(method, arguments, options);
        return await httpClient.Request(requestPack, transformation, cancellationToken);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 生成HTTP请求
    /// <summary>
    /// 辅助方法：根据表达式，
    /// 生成HTTP请求对象
    /// </summary>
    /// <param name="method">调用的API方法，
    /// 他会被映射到一个控制器操作</param>
    /// <param name="arguments">调用API传入的方法参数</param>
    /// <param name="options">一个用于转换Json的对象</param>
    /// <returns></returns>
    private static HttpRequestRecording GetRequest(MethodInfo method, IEnumerable<Expression> arguments, JsonSerializerOptions? options)
    {
        var callPath = GetCallUri(method);
        var prameterInfos = method.GetParameters().
            Zip(arguments).
            Select(x => new HttpStrongTypeRequestParameterInfo(x.First, x.Second.CalValue())).
            ToArray();
        var httpMethod = GetRequestMethod(method, prameterInfos);
        var uriParameter = GetRequestUriParameter(method, httpMethod, prameterInfos);
        var httpContent = GetRequestContent(method, httpMethod, prameterInfos, options);
        return new()
        {
            Uri = callPath with
            {
                UriParameter = uriParameter
            },
            HttpMethod = httpMethod,
            Content = httpContent,
        };
    }
    #endregion
    #region 生成调用Uri
    /// <summary>
    /// 生成对WebApi的调用Uri
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetRequest"/>
    private static UriComplete GetCallUri(MethodInfo method)
    {
        if (method.GetCustomAttribute<RouteDescriptionAttribute>() is { Template: { } methodTemplate })
            return methodTemplate;
        var interfaceType = typeof(API);
        var interfaceName = interfaceType.Name.TrimStart('I').Trim(false, "Controller");
        var methodName = method.Name;
        if (interfaceType.GetCustomAttribute<RouteDescriptionAttribute>() is { Template: { } interfaceTemplate })
            return interfaceTemplate.Replace("[controller]", interfaceName).
                Replace("[action]", methodName);
        return $"/api/{interfaceName}/{methodName}";        //如果没有任何特性，则使用默认的调用路径
    }
    #endregion
    #region 判断应该使用哪个Http方法
    /// <summary>
    /// 判断该强类型Http请求应该使用哪个Http方法
    /// </summary>
    /// <param name="method">强类型Http请求的方法对象</param>
    /// <param name="prameterInfos">描述请求参数的对象</param>
    /// <returns></returns>
    private static HttpMethod GetRequestMethod(MethodInfo method, IEnumerable<HttpStrongTypeRequestParameterInfo> prameterInfos)
    {
        var appoint = method.GetCustomAttribute<HttpRequestMethodAttribute>()?.
            Method ?? HttpRequestMethod.Auto;
        switch (appoint)
        {
            case HttpRequestMethod.Auto:
                var isPost = prameterInfos.Any(x =>
                x is { ParameterSource: not (HttpRequestParameterSource.Auto or HttpRequestParameterSource.FromQuery) } or
                { IsCommonType: false });
                return isPost ? HttpMethod.Post : HttpMethod.Get;
            case HttpRequestMethod.Get:
                return HttpMethod.Get;
            case HttpRequestMethod.Post:
                return HttpMethod.Post;
            default:
                throw appoint.Unrecognized();
        }
    }
    #endregion
    #region 生成Uri参数
    /// <summary>
    /// 生成请求中Uri的参数部分
    /// </summary>
    /// <param name="methodInfo">强类型Http请求的方法</param>
    /// <param name="httpMethod">请求所使用的Http方法</param>
    /// <param name="prameterInfos">描述请求参数的对象</param>
    /// <returns></returns>
    private static UriParameter GetRequestUriParameter(MethodInfo methodInfo, HttpMethod httpMethod, IEnumerable<HttpStrongTypeRequestParameterInfo> prameterInfos)
    {
        #region 用来枚举参数名称和值的本地函数
        IEnumerable<(string Name, string? Value)> AllUriParameter()
        {
            foreach (var prameterInfo in prameterInfos)
            {
                var parameterSource = prameterInfo.ParameterSource;
                if (parameterSource is not (HttpRequestParameterSource.Auto or HttpRequestParameterSource.FromQuery))
                    continue;
                if (httpMethod == HttpMethod.Get || parameterSource is HttpRequestParameterSource.FromQuery)
                {
                    var name = prameterInfo.Parameter.Name!;
                    if (prameterInfo.IsCommonType)
                    {
                        var value = prameterInfo.Value;
                        if (value is { })
                            yield return (name, value.ToString());
                    }
                    else
                        throw new NotSupportedException($"{typeof(API)}.{methodInfo.Name}的{name}参数是一个复杂的类型，" +
                            $"它不能放在HttpGet方法中，或被指定为来自查询Uri参数");
                }
            }
        }
        #endregion
        var parameter = AllUriParameter().ToArray();
        return new(parameter);
    }
    #endregion
    #region 生成请求内容
    /// <summary>
    /// 生成请求内容，并返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetRequestUriParameter(MethodInfo, HttpMethod, IEnumerable{HttpStrongTypeRequestParameterInfo})"/>
    /// <inheritdoc cref="GetRequest(MethodInfo, IEnumerable{Expression}, JsonSerializerOptions?)"/>
    private static HttpContent? GetRequestContent(MethodInfo methodInfo, HttpMethod httpMethod, IEnumerable<HttpStrongTypeRequestParameterInfo> prameterInfos, JsonSerializerOptions? options)
    {
        if (httpMethod == HttpMethod.Get)
            return null;
        var filterParameter = prameterInfos.
            Where(x => x is { IsCommonType: false } or { ParameterSource: HttpRequestParameterSource.Auto or HttpRequestParameterSource.FromBody }).ToArray();
        return filterParameter switch
        {
            null or [] => null,
            [var info] => GetRequestContentFinal(info, options),
            _ => throw new NotSupportedException($"{typeof(API)}.{methodInfo.Name}方法存在多个应该被封装进请求体的参数，无法生成请求内容"),
        };
    }
    #endregion
    #region 生成最终请求内容
    /// <summary>
    /// 生成最终的请求内容
    /// </summary>
    /// <param name="info">请求参数的信息</param>
    /// <returns></returns>
    /// <inheritdoc cref="GetRequest(MethodInfo, IEnumerable{Expression}, JsonSerializerOptions?)"/>
    private static HttpContent GetRequestContentFinal(HttpStrongTypeRequestParameterInfo info, JsonSerializerOptions? options)
    {
        #region 用来返回上传正文的本地函数
        static MultipartFormDataContent GetContent(IEnumerable<IUploadFile> uploadFiles)
        {
            var content = new MultipartFormDataContent();
            foreach (var file in uploadFiles)
            {
                var fileContent = new StreamContent(file.FileStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "\"files\"", file.FileName);
            }
            return content;
        }
        #endregion
        return info.Value switch
        {
            IUploadFile uploadFile => GetContent([uploadFile]),
            IEnumerable<IUploadFile> uploadFiles => GetContent(uploadFiles),
            var value => JsonContent.Create(value, info.Parameter.ParameterType, options: options)
        };
    }
    #endregion
    #endregion
}
